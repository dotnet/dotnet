#!/bin/sh
# Idempotently restore the robust dnx dispatcher shipped by dotnet-host and ensure the
# public /usr/bin/dnx entry point is a symlink to it.
#
# This is shared by:
#   - the Debian 'postinst' maintainer script (invoked on configure and on trigger activation),
#   - the RPM '%post' scriptlet (invoked on install and upgrade), and
#   - the RPM file trigger that fires when another package touches the dnx paths.
#
# Background (dotnet/sdk#55303): released dotnet-sdk-10.0 packages shipped /usr/bin/dnx as a
# regular file whose only content was the text "../share/dotnet/dnx" (so direct execution
# failed), and an older dispatcher at /usr/share/dotnet/dnx. dotnet-host owns the robust
# versions; this script (re)installs them from a private, non-conflicting source copy so that
# co-installing or reinstalling an SDK cannot leave a broken dnx behind.
#
# The script is intentionally best-effort: a failure to repair dnx must never abort an
# unrelated package transaction (for example, an SDK install that activated the file trigger).

dispatcher_source="/usr/share/dotnet/dnx.dispatcher"
dispatcher_target="/usr/share/dotnet/dnx"
public_entry="/usr/bin/dnx"

if [ -f "$dispatcher_source" ]; then
    install -m 0755 "$dispatcher_source" "$dispatcher_target" 2>/dev/null || :
    ln -sfn ../share/dotnet/dnx "$public_entry" 2>/dev/null || :
fi

exit 0
