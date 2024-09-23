param (
    [Parameter(Mandatory=$true)]
    [Alias("d", "destination-directory")]
    [string]$destDir
)

try {
    if ($IsWindows) {
        Start-Service docker
    } elseif ($IsLinux) {
        systemctl start docker
    } else {
        Write-Host "Unsupported OS"
        exit 1
    }

    if ($LASTEXITCODE -ne 0) {
        throw "Failed to start Docker service. Ensure it is installed and the docker CLI is available via PATH."
    }
} catch {
    Write-Error "An error occurred: $_"
    exit 1
}

$azuriteMcr = "mcr.microsoft.com/azure-storage/azurite"
$containerName = "azurite_data_generator"
$volumeMountPoint = "/data"

try {
    docker run -d --name $containerName -p 10000:10000 -p 10001:10001 -p 10002:10002 $azuriteMcr
    docker cp "$containerName`:$volumeMountPoint" "$destDir"
} finally {
    docker stop $containerName
    docker rm $containerName
}
