#!/bin/sh
set -eu

package_type="$1"
host_package="$2"

# Exercise both package installation orders against the repository-selected .NET 10 SDK. Each
# scenario also removes the SDK before the host to prove that the host keeps the public dnx entries
# alive, then removes the host to prove that its final-removal script cleans them up.

assert_dotnet_dnx()
{
  /usr/share/dotnet/dotnet dnx --help >/dev/null
}

assert_sdk_dnx()
{
  # Accept a future SDK package that already contains the repaired layout. For the released legacy
  # layout, require the exact broken executable and exit code that the host package must repair.
  if test -L /usr/bin/dnx; then
    test "$(readlink /usr/bin/dnx)" = "../share/dotnet/dnx"
    /usr/bin/dnx --help >/dev/null
  else
    test -f /usr/bin/dnx
    test -x /usr/bin/dnx
    test "$(cat /usr/bin/dnx)" = "../share/dotnet/dnx"

    set +e
    (cd / && /usr/bin/dnx --help >/tmp/dnx-direct.out 2>&1)
    result="$?"
    set -e
    test "$result" -eq 127
  fi

  assert_dotnet_dnx
}

assert_repaired_entries()
{
  test -L /usr/bin/dnx
  test "$(readlink /usr/bin/dnx)" = "../share/dotnet/dnx"
  test -x /usr/share/dotnet/dnx
  test "$(cat /usr/share/dotnet/dnx)" = "$(cat /usr/share/dotnet/dnx.dispatcher)"
}

assert_repaired_dnx()
{
  assert_repaired_entries
  /usr/bin/dnx --help >/dev/null
  assert_dotnet_dnx
}

assert_cleaned_dnx()
{
  test ! -e /usr/bin/dnx
  test ! -e /usr/share/dotnet/dnx
  test ! -e /usr/share/dotnet/dnx.dispatcher
}

if test "$package_type" = "rpm"; then
  host_name="$(rpm -qp --qf '%{NAME}' "$host_package")"

  install_sdk()
  {
    tdnf install -qy dotnet-sdk-10.0
  }

  reinstall_sdk()
  {
    tdnf reinstall -qy dotnet-sdk-10.0
  }

  install_host()
  {
    rpm -U --replacepkgs --nodeps "$host_package"
  }

  remove_sdk()
  {
    tdnf remove -qy dotnet-sdk-10.0
  }

  remove_host()
  {
    rpm -e --nodeps "$host_name"
  }

  wipe()
  {
    rpm -qa | while read -r package; do
      case "$package" in
        dotnet-*|aspnetcore-*|netstandard-*|"$host_name"-*)
          rpm -e --nodeps "$package" >/dev/null 2>&1 || true
          ;;
      esac
    done
    rm -f /usr/bin/dnx /usr/share/dotnet/dnx /usr/share/dotnet/dnx.dispatcher
    rm -rf /usr/share/dotnet/sdk
  }

  # SDK first: installing the host must take ownership of the public entries and repair them.
  wipe
  install_sdk
  assert_sdk_dnx
  install_host
  assert_repaired_dnx
  rpm -qf /usr/bin/dnx | grep -q "$host_name"
  test -z "$(rpm -V "$host_name")"
  remove_sdk
  assert_repaired_entries
  remove_host
  assert_cleaned_dnx

  # Host first: SDK installation and reinstallation must trigger repair after overwriting the
  # public entries. Reinstalling the host must also remain safe and idempotent.
  wipe
  install_host
  install_sdk
  assert_repaired_dnx
  reinstall_sdk
  assert_repaired_dnx
  install_host
  assert_repaired_dnx
  test -z "$(rpm -V "$host_name")"
  remove_sdk
  assert_repaired_entries
  remove_host
  assert_cleaned_dnx
elif test "$package_type" = "deb"; then
  host_name="$(dpkg-deb -f "$host_package" Package)"

  install_sdk()
  {
    apt-get install -y -qq dotnet-sdk-10.0
  }

  reinstall_sdk()
  {
    apt-get install --reinstall -y -qq dotnet-sdk-10.0
  }

  install_host()
  {
    dpkg -i "$host_package"
  }

  remove_sdk()
  {
    apt-get purge -y -qq dotnet-sdk-10.0
  }

  remove_host()
  {
    dpkg --purge --force-depends "$host_name"
  }

  wipe()
  {
    dpkg-query -W -f='${binary:Package}\n' 2>/dev/null | while read -r package; do
      case "$package" in
        dotnet-*|aspnetcore-*|netstandard-*|"$host_name")
          dpkg --purge --force-depends "$package" >/dev/null 2>&1 || true
          ;;
      esac
    done
    rm -f /usr/bin/dnx /usr/share/dotnet/dnx /usr/share/dotnet/dnx.dispatcher
    rm -rf /usr/share/dotnet/sdk
  }

  # SDK first: installing the host must take ownership of the public entries and repair them.
  wipe
  install_sdk
  assert_sdk_dnx
  install_host
  assert_repaired_dnx
  dpkg-query -S /usr/bin/dnx | grep -q "$host_name"
  test -z "$(dpkg --verify "$host_name")"
  remove_sdk
  assert_repaired_entries
  remove_host
  assert_cleaned_dnx

  # Host first: SDK installation and reinstallation must trigger repair after overwriting the
  # public entries. Reinstalling the host must also remain safe and idempotent.
  wipe
  install_host
  install_sdk
  assert_repaired_dnx
  reinstall_sdk
  assert_repaired_dnx
  install_host
  assert_repaired_dnx
  test -z "$(dpkg --verify "$host_name")"
  remove_sdk
  assert_repaired_entries
  remove_host
  assert_cleaned_dnx
else
  echo "Unknown package type '$package_type'." >&2
  exit 1
fi
