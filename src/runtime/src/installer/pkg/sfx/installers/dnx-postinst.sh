#!/bin/sh
set -e

# Idempotently restore the robust dnx dispatcher shipped by dotnet-host and ensure the
# public /usr/bin/dnx entry point is a symlink to it.
#
# This is shared by:
#   - the Debian 'postinst' maintainer script (invoked on configure and on trigger activation),
#   - the RPM '%post' scriptlet (invoked on install and upgrade), and
#   - the RPM transaction file trigger that fires when an SDK changes its install tree.
#
# Background (dotnet/sdk#55303): released dotnet-sdk-10.0 packages shipped /usr/bin/dnx as a
# regular file whose only content was the text "../share/dotnet/dnx" (so direct execution
# failed), and an older dispatcher at /usr/share/dotnet/dnx. dotnet-host owns the robust
# versions; this script (re)installs them from a private, non-conflicting source copy so that
# co-installing or reinstalling an SDK cannot leave a broken dnx behind.
#
dispatcher_source="/usr/share/dotnet/dnx.dispatcher"
dispatcher_target="/usr/share/dotnet/dnx"
public_entry="/usr/bin/dnx"

if [ ! -f "$dispatcher_source" ]; then
  echo "dotnet-host cannot repair dnx because $dispatcher_source is missing." >&2
  exit 1
fi

install -m 0755 "$dispatcher_source" "$dispatcher_target"
ln -sfn ../share/dotnet/dnx "$public_entry"
