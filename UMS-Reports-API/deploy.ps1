# ==============================================================================
# deploy.ps1 — DocXReports API — Windows Build → Linux Deploy
#
# USAGE:
#   .\deploy.ps1                         # build + save only (no transfer)
#   .\deploy.ps1 -Transfer               # build + save + SCP to Linux server
#   .\deploy.ps1 -Transfer -RunOnServer  # build + save + SCP + load + run
#
# PREREQUISITES on Windows:
#   - Docker Desktop running in Linux container mode
#   - PowerShell 5.1 or later
#   - OpenSSH client (built into Windows 10/11) — only needed for -Transfer
#
# PREREQUISITES on Linux server:
#   - Docker Engine installed (curl -fsSL https://get.docker.com | sh)
#   - SSH access from this Windows machine
# ==============================================================================

param(
    [switch]$Transfer,          # SCP the .tar to the Linux server
    [switch]$RunOnServer,       # SSH into Linux and load + run the container
    [string]$ImageName    = "ums-reports-docx",
    [string]$ImageTag     = "latest",
    [string]$TarFile      = "ums-reports-docx.tar",
    [string]$LinuxHost    = "192.168.1.50",       # <-- change to your Linux server IP
    [string]$LinuxUser    = "ubuntu",              # <-- change to your Linux SSH user
    [string]$LinuxPath    = "/home/ubuntu",        # <-- where to upload the .tar on Linux
    [int]   $HostPort     = 5013,
    [int]   $ContainerPort= 5013,
    [string]$ContainerName= "docx-api"
)

# ── Helpers ───────────────────────────────────────────────────────────────────
function Write-Step([string]$msg) {
    Write-Host ""
    Write-Host "──────────────────────────────────────────" -ForegroundColor Cyan
    Write-Host "  $msg" -ForegroundColor Cyan
    Write-Host "──────────────────────────────────────────" -ForegroundColor Cyan
}

function Invoke-Step([string]$desc, [scriptblock]$cmd) {
    Write-Host "  → $desc" -ForegroundColor Yellow
    & $cmd
    if ($LASTEXITCODE -ne 0) {
        Write-Host ""
        Write-Host "  ✗ FAILED: $desc (exit code $LASTEXITCODE)" -ForegroundColor Red
        exit $LASTEXITCODE
    }
    Write-Host "  ✓ Done" -ForegroundColor Green
}

# ── Resolve script directory (always run from UMS-Reports-API\) ───────────────
$ProjectRoot = $PSScriptRoot
Set-Location $ProjectRoot

Write-Host ""
Write-Host "  DocXReports API — Deploy Script" -ForegroundColor White
Write-Host "  Image  : ${ImageName}:${ImageTag}" -ForegroundColor Gray
Write-Host "  Tar    : $TarFile" -ForegroundColor Gray
if ($Transfer) {
    Write-Host "  Target : ${LinuxUser}@${LinuxHost}:${LinuxPath}" -ForegroundColor Gray
}
Write-Host ""

# ==============================================================================
# STEP 1 — Verify Docker Desktop is running and in Linux mode
# ==============================================================================
Write-Step "Step 1/4 — Checking Docker"

$dockerInfo = docker info 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "  ✗ Docker is not running. Please start Docker Desktop." -ForegroundColor Red
    exit 1
}

$osType = docker info --format "{{.OSType}}" 2>&1
if ($osType -ne "linux") {
    Write-Host "  ✗ Docker Desktop is in Windows container mode." -ForegroundColor Red
    Write-Host "    Right-click the Docker tray icon → Switch to Linux containers." -ForegroundColor Yellow
    exit 1
}

Write-Host "  ✓ Docker is running in Linux container mode" -ForegroundColor Green

# ==============================================================================
# STEP 2 — Build the image targeting linux/amd64
# ==============================================================================
Write-Step "Step 2/4 — Building image (linux/amd64)"

Write-Host "  This will take 5-15 minutes on first build (LibreOffice is large)." -ForegroundColor Gray
Write-Host "  Subsequent builds are much faster thanks to layer caching." -ForegroundColor Gray
Write-Host ""

Invoke-Step "docker build --platform linux/amd64 -t ${ImageName}:${ImageTag}" {
    docker build `
        --platform linux/amd64 `
        -t "${ImageName}:${ImageTag}" `
        --progress=plain `
        $ProjectRoot
}

# Show the image size
$imageSize = docker images "${ImageName}:${ImageTag}" --format "{{.Size}}"
Write-Host "  Image size: $imageSize" -ForegroundColor Gray

# ==============================================================================
# STEP 3 — Save image to .tar file
# ==============================================================================
Write-Step "Step 3/4 — Saving image to $TarFile"

$tarPath = Join-Path $ProjectRoot $TarFile

Invoke-Step "docker save -o $TarFile ${ImageName}:${ImageTag}" {
    docker save -o $tarPath "${ImageName}:${ImageTag}"
}

$tarSize = (Get-Item $tarPath).Length / 1MB
Write-Host ("  Tar file size: {0:N0} MB — {1}" -f $tarSize, $tarPath) -ForegroundColor Gray

if (-not $Transfer) {
    Write-Host ""
    Write-Host "  ✓ Build complete. Image saved to: $tarPath" -ForegroundColor Green
    Write-Host ""
    Write-Host "  To transfer manually, run:" -ForegroundColor White
    Write-Host "    scp $TarFile ${LinuxUser}@${LinuxHost}:${LinuxPath}/" -ForegroundColor Gray
    Write-Host ""
    Write-Host "  Then on the Linux server:" -ForegroundColor White
    Write-Host "    docker load -i ${LinuxPath}/$TarFile" -ForegroundColor Gray
    Write-Host "    docker run -d --restart always -p ${HostPort}:${ContainerPort} --name $ContainerName ${ImageName}:${ImageTag}" -ForegroundColor Gray
    Write-Host ""
    exit 0
}

# ==============================================================================
# STEP 4 — Transfer to Linux server and optionally load + run
# ==============================================================================
Write-Step "Step 4/4 — Transferring to ${LinuxUser}@${LinuxHost}"

Invoke-Step "scp $TarFile → ${LinuxUser}@${LinuxHost}:${LinuxPath}/" {
    scp $tarPath "${LinuxUser}@${LinuxHost}:${LinuxPath}/$TarFile"
}

Write-Host "  ✓ Transfer complete" -ForegroundColor Green

if (-not $RunOnServer) {
    Write-Host ""
    Write-Host "  To load and run on the Linux server, SSH in and run:" -ForegroundColor White
    Write-Host "    docker load -i ${LinuxPath}/$TarFile" -ForegroundColor Gray
    Write-Host "    docker run -d --restart always -p ${HostPort}:${ContainerPort} --name $ContainerName ${ImageName}:${ImageTag}" -ForegroundColor Gray
    Write-Host ""
    exit 0
}

# ── Load and run via SSH ──────────────────────────────────────────────────────
Write-Host ""
Write-Host "  → Loading image and starting container on Linux server..." -ForegroundColor Yellow

$remoteCommands = @"
set -e

echo '--- Loading Docker image ---'
docker load -i ${LinuxPath}/$TarFile

echo '--- Stopping existing container (if any) ---'
docker stop $ContainerName 2>/dev/null || true
docker rm   $ContainerName 2>/dev/null || true

echo '--- Starting container ---'
docker run -d \
  --restart always \
  -p ${HostPort}:${ContainerPort} \
  --name $ContainerName \
  ${ImageName}:${ImageTag}

echo '--- Waiting for app to start ---'
sleep 5

echo '--- Container status ---'
docker ps --filter name=$ContainerName

echo '--- Last 20 log lines ---'
docker logs --tail 20 $ContainerName

echo ''
echo '✓ DocXReports API is running at http://\$(hostname -I | awk "{print \$1}"):${HostPort}/swagger'
"@

Invoke-Step "SSH: load + run on ${LinuxHost}" {
    ssh "${LinuxUser}@${LinuxHost}" $remoteCommands
}

# ==============================================================================
# Summary
# ==============================================================================
Write-Host ""
Write-Host "══════════════════════════════════════════" -ForegroundColor Green
Write-Host "  ✓ Deployment complete" -ForegroundColor Green
Write-Host "══════════════════════════════════════════" -ForegroundColor Green
Write-Host ""
Write-Host "  Swagger UI  : http://${LinuxHost}:${HostPort}/swagger" -ForegroundColor White
Write-Host "  Render API  : http://${LinuxHost}:${HostPort}/api/DocXReports/Render" -ForegroundColor White
Write-Host "  Render V1   : http://${LinuxHost}:${HostPort}/api/DocXReports/RenderV1" -ForegroundColor White
Write-Host ""
Write-Host "  Useful commands on the Linux server:" -ForegroundColor Gray
Write-Host "    docker logs -f $ContainerName          # live logs" -ForegroundColor Gray
Write-Host "    docker restart $ContainerName          # restart" -ForegroundColor Gray
Write-Host "    docker stop $ContainerName             # stop" -ForegroundColor Gray
Write-Host "    docker stats $ContainerName            # CPU / memory usage" -ForegroundColor Gray
Write-Host ""
