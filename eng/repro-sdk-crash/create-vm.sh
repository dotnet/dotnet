#!/usr/bin/env bash
set -euo pipefail

usage() {
  cat <<'EOF'
Usage: bash create-vm.sh --resource-group GROUP --name NAME --image IMAGE \
  --admin-username USER --ssh-public-key FILE --ssh-source-cidr CIDR \
  --confirm-create

Creates billable resources in the CURRENT az subscription. Authenticate/select
the intended subscription first. Defaults: centralus, Standard_D8as_v4.

--image must be an accessible Azure image resource ID or Marketplace URN.
The CI image was build.azurelinux.3.amd64.open version 2026.0831.210244;
this is a custom 1ES image label, NOT a Marketplace URN. Ask the pool owner
for its image resource ID to match it, or supply another Docker-capable
Linux host image and treat the kernel/image difference as a limitation.

SSH is allowed only from --ssh-source-cidr. Use an existing public key.
Install Docker on the host if needed, copy the prebuilt SDK bundle and these
scripts using scp, then run run-native.sh with a fresh output directory.
No resource cleanup is automatic. Delete only this VM and its associated
resources when finished, or delete the group if it was dedicated to this repro.
EOF
}

group= name= image= user= key= cidr= confirm=false
while [[ $# -gt 0 ]]; do
  case "$1" in
    --resource-group) group=${2:?}; shift 2 ;;
    --name) name=${2:?}; shift 2 ;;
    --image) image=${2:?}; shift 2 ;;
    --admin-username) user=${2:?}; shift 2 ;;
    --ssh-public-key) key=${2:?}; shift 2 ;;
    --ssh-source-cidr) cidr=${2:?}; shift 2 ;;
    --confirm-create) confirm=true; shift ;;
    -h|--help) usage; exit 0 ;;
    *) echo "Unknown option: $1" >&2; usage >&2; exit 2 ;;
  esac
done
if [[ -z "$group" || -z "$name" || -z "$image" || -z "$user" || -z "$key" || -z "$cidr" || "$confirm" != true ]]; then
  usage >&2
  exit 2
fi
[[ -f "$key" ]] || { echo "Public key file not found: $key" >&2; exit 2; }
python3 -c 'import ipaddress,sys
network=ipaddress.ip_network(sys.argv[1], strict=False)
if network.version != 4 or network.prefixlen == 0:
    sys.exit("Provide a restricted IPv4 SSH source CIDR, not 0.0.0.0/0")' "$cidr"
az account show --query '{subscription:name,id:id}' --output table
az group create --name "$group" --location centralus --output none
az vm create --resource-group "$group" --name "$name" \
  --location centralus --size Standard_D8as_v4 --image "$image" \
  --admin-username "$user" --ssh-key-values "$key" \
  --os-disk-size-gb 128 --public-ip-sku Standard \
  --nsg "${name}-nsg" --nsg-rule NONE --output none
az network nsg rule create --resource-group "$group" --nsg-name "${name}-nsg" \
  --name ReproSSH --priority 1000 --direction Inbound --access Allow \
  --protocol Tcp --source-address-prefixes "$cidr" --source-port-ranges '*' \
  --destination-address-prefixes '*' --destination-port-ranges 22 --output none
az vm show --resource-group "$group" --name "$name" --show-details \
  --query '{name:name,size:hardwareProfile.vmSize,location:location,publicIP:publicIps}' \
  --output table
