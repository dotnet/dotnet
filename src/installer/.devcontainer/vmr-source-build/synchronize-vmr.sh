#!/bin/bash

(cd /workspaces/installer \
    && ./eng/vmr-sync.sh --vmr /workspaces/dotnet --tmp /workspaces/tmp $*)
