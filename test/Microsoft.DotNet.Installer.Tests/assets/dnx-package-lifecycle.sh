#!/bin/sh
set -eu

package_type="$1"
host_package="$2"
fixture_root="/tmp/dnx-package-lifecycle"

rm -rf "$fixture_root"
mkdir -p "$fixture_root"

assert_legacy_dnx()
{
  test ! -L /usr/bin/dnx
  test "$(cat /usr/bin/dnx)" = "../share/dotnet/dnx"

  set +e
  (cd / && /usr/bin/dnx --help >/tmp/dnx-direct.out 2>&1)
  result="$?"
  set -e
  test "$result" -eq 127

  /usr/share/dotnet/dnx --help | grep -q "SDK-DNX"
}

assert_repaired_dnx()
{
  test -L /usr/bin/dnx
  test "$(readlink /usr/bin/dnx)" = "../share/dotnet/dnx"
  cmp /usr/share/dotnet/dnx /usr/share/dotnet/dnx.dispatcher
  test -x /usr/share/dotnet/dnx
}

assert_cleaned_dnx()
{
  test ! -e /usr/bin/dnx
  test ! -e /usr/share/dotnet/dnx
}

if test "$package_type" = "rpm"; then
  top="$fixture_root/rpmbuild"
  mkdir -p "$top"/BUILD "$top"/BUILDROOT "$top"/RPMS "$top"/SOURCES "$top"/SPECS "$top"/SRPMS

  create_rpm_sdk()
  {
    package_name="$1"
    version="$2"
    owns_dnx="$3"
    spec="$top/SPECS/$package_name-$version.spec"

    if test "$owns_dnx" = "true"; then
      install_dnx='
install -d %{buildroot}/usr/bin %{buildroot}/usr/share/dotnet
printf '"'"'#!/bin/sh\necho "SDK-DNX $*"\n'"'"' > %{buildroot}/usr/share/dotnet/dnx
chmod 0755 %{buildroot}/usr/share/dotnet/dnx
printf '"'"'../share/dotnet/dnx'"'"' > %{buildroot}/usr/bin/dnx
chmod 0755 %{buildroot}/usr/bin/dnx'
      files_dnx='
/usr/bin/dnx
/usr/share/dotnet/dnx'
    else
      install_dnx=""
      files_dnx=""
    fi

    cat > "$spec" <<EOF
%global debug_package %{nil}
%global _unpackaged_files_terminate_build 0
%global _build_id_links none
%global __brp_mangle_shebangs %{nil}
Name: $package_name
Version: $version
Release: 1
Summary: dnx lifecycle fixture
License: MIT
BuildArch: noarch
AutoReqProv: no
%description
dnx lifecycle fixture
%install
install -d %{buildroot}/usr/share/dotnet/sdk/$version
touch %{buildroot}/usr/share/dotnet/sdk/$version/sdk-marker
$install_dnx
%files
/usr/share/dotnet/sdk/$version/sdk-marker
$files_dnx
EOF

    rpmbuild -bb --quiet --define "_topdir $top" "$spec" >/dev/null
    find "$top/RPMS" -name "$package_name-$version-1.noarch.rpm" -print -quit
  }

  sdk_301="$(create_rpm_sdk dotnet-sdk-10.0 10.0.301 true)"
  sdk_302="$(create_rpm_sdk dotnet-sdk-10.0 10.0.302 true)"
  sdk_11="$(create_rpm_sdk dotnet-sdk-11.0 11.0.100 false)"
  host_name="$(rpm -qp --qf '%{NAME}' "$host_package")"

  wipe()
  {
    for package in dotnet-sdk-11.0 dotnet-sdk-10.0 "$host_name"; do
      rpm -e "$package" >/dev/null 2>&1 || true
    done
    rm -f /usr/bin/dnx /usr/share/dotnet/dnx /usr/share/dotnet/dnx.dispatcher
    rm -rf /usr/share/dotnet/sdk
  }

  wipe
  rpm -i "$sdk_301"
  assert_legacy_dnx
  rpm -i "$host_package"
  assert_repaired_dnx
  rpm -qf /usr/bin/dnx | grep -q "$host_name"
  test -z "$(rpm -V "$host_name")"

  wipe
  rpm -i "$host_package"
  rpm -i "$sdk_301"
  assert_repaired_dnx
  rpm -i --replacepkgs "$sdk_301"
  assert_repaired_dnx
  rpm -U "$sdk_302"
  assert_repaired_dnx
  rpm -U --replacepkgs "$host_package"
  assert_repaired_dnx
  rpm -i "$sdk_11"
  rpm -e dotnet-sdk-10.0
  assert_repaired_dnx
  rpm -e dotnet-sdk-11.0
  assert_repaired_dnx
  test -z "$(rpm -V "$host_name")"
  rpm -e "$host_name"
  assert_cleaned_dnx

  wipe
  rpm -i "$sdk_301" "$host_package"
  assert_repaired_dnx
  rpm -i "$sdk_11"
  rpm -e dotnet-sdk-11.0
  assert_repaired_dnx
  rpm -e dotnet-sdk-10.0
  assert_repaired_dnx
  rpm -e "$host_name"
  assert_cleaned_dnx
elif test "$package_type" = "deb"; then
  architecture="$(dpkg --print-architecture)"

  create_deb_sdk()
  {
    package_name="$1"
    version="$2"
    owns_dnx="$3"
    package_root="$fixture_root/$package_name-$version"
    package_path="$fixture_root/$package_name-$version.deb"

    rm -rf "$package_root"
    mkdir -p "$package_root/DEBIAN" "$package_root/usr/share/dotnet/sdk/$version"
    touch "$package_root/usr/share/dotnet/sdk/$version/sdk-marker"

    cat > "$package_root/DEBIAN/control" <<EOF
Package: $package_name
Version: $version
Architecture: $architecture
Maintainer: .NET Foundation
Description: dnx lifecycle fixture
EOF

    if test "$owns_dnx" = "true"; then
      mkdir -p "$package_root/usr/bin" "$package_root/usr/share/dotnet"
      printf '#!/bin/sh\necho "SDK-DNX $*"\n' > "$package_root/usr/share/dotnet/dnx"
      chmod 0755 "$package_root/usr/share/dotnet/dnx"
      printf '../share/dotnet/dnx' > "$package_root/usr/bin/dnx"
      chmod 0755 "$package_root/usr/bin/dnx"
    fi

    dpkg-deb --build "$package_root" "$package_path" >/dev/null
    echo "$package_path"
  }

  sdk_301="$(create_deb_sdk dotnet-sdk-10.0 10.0.301 true)"
  sdk_302="$(create_deb_sdk dotnet-sdk-10.0 10.0.302 true)"
  sdk_out_of_range="$(create_deb_sdk dotnet-sdk-10.0 10.1.0 true)"
  sdk_11="$(create_deb_sdk dotnet-sdk-11.0 11.0.100 false)"

  wipe()
  {
    for package in dotnet-sdk-11.0 dotnet-sdk-10.0 dotnet-host; do
      dpkg --purge "$package" >/dev/null 2>&1 || true
    done
    rm -f /usr/bin/dnx /usr/share/dotnet/dnx /usr/share/dotnet/dnx.dispatcher
    rm -rf /usr/share/dotnet/sdk
  }

  wipe
  dpkg -i "$sdk_301"
  assert_legacy_dnx
  dpkg -i "$host_package"
  assert_repaired_dnx
  dpkg -S /usr/bin/dnx | grep -q "dotnet-host"
  test -z "$(dpkg --verify dotnet-host)"

  wipe
  dpkg -i "$host_package"
  dpkg -i "$sdk_301"
  assert_repaired_dnx
  dpkg -i "$sdk_301"
  assert_repaired_dnx
  dpkg -i "$sdk_302"
  assert_repaired_dnx
  dpkg -i "$host_package"
  assert_repaired_dnx
  dpkg -i "$sdk_11"
  dpkg --purge dotnet-sdk-10.0
  assert_repaired_dnx
  dpkg --purge dotnet-sdk-11.0
  assert_repaired_dnx
  test -z "$(dpkg --verify dotnet-host)"
  dpkg --purge dotnet-host
  assert_cleaned_dnx

  wipe
  dpkg -i "$sdk_301" "$host_package"
  assert_repaired_dnx
  dpkg -i "$sdk_11"
  dpkg --purge dotnet-sdk-11.0
  assert_repaired_dnx
  dpkg --purge dotnet-sdk-10.0
  assert_repaired_dnx
  dpkg --purge dotnet-host
  assert_cleaned_dnx

  wipe
  dpkg -i "$sdk_out_of_range"
  if dpkg -i "$host_package"; then
    echo "The host unexpectedly replaced an out-of-range SDK package." >&2
    exit 1
  fi
  host_status="$(dpkg-query -W -f='${db:Status-Status}' dotnet-host 2>/dev/null || true)"
  test "$host_status" != "installed"
  assert_legacy_dnx
  wipe
else
  echo "Unknown package type '$package_type'." >&2
  exit 1
fi
