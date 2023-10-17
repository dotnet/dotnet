#!/bin/bash

set -euo pipefail

# https://scancode-toolkit.readthedocs.io/en/latest/getting-started/install.html#installation-as-a-library-via-pip

pyEnvPath="/tmp/scancode-env"
python3 -m venv $pyEnvPath
source $pyEnvPath/bin/activate
pip install scancode-toolkit
deactivate

# Setup a script which executes scancode in the virtual environment
cat > /usr/local/bin/scancode << EOF
#!/bin/bash
set -euo pipefail
source $pyEnvPath/bin/activate
scancode "\$@"
deactivate
EOF

chmod +x /usr/local/bin/scancode
