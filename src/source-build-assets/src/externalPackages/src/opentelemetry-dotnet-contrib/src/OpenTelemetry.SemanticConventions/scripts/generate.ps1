$SCRIPT_DIR = $PSScriptRoot
$ROOT_DIR = "${SCRIPT_DIR}/../"

# freeze the spec version to make SemanticAttributes generation reproducible
$SEMCONV_VERSION="1.39.0"
$GENERATOR_VERSION="v0.20.0"

Set-Location $SCRIPT_DIR

Remove-Item -r -fo -ErrorAction SilentlyContinue semantic-conventions
mkdir semantic-conventions
Set-Location semantic-conventions

git init
git remote add origin https://github.com/open-telemetry/semantic-conventions.git
git fetch origin v$SEMCONV_VERSION
git reset --hard FETCH_HEAD
Set-Location ${SCRIPT_DIR}

docker run --rm `
  -v ${SCRIPT_DIR}/semantic-conventions/model:/source `
  -v ${SCRIPT_DIR}/templates:/templates `
  -v ${ROOT_DIR}/Attributes/:/output `
  otel/weaver:$GENERATOR_VERSION `
  registry `
  generate `
  --registry=/source `
  --templates=/templates `
  "./" `
  "/output/./"`
