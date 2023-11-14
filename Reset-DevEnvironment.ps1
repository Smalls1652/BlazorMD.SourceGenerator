[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [switch]$NoRestore
)

$scriptRoot = $PSScriptRoot

$srcDirPath = Join-Path -Path $scriptRoot -ChildPath "src"
$slnPath = Join-Path -Path $scriptRoot -ChildPath "BlazorMD.SourceGenerator.sln"

$devDirs = Get-ChildItem -Path $srcDirPath -Directory -Recurse -Depth 2 | Where-Object { $PSItem.Name -in @( "bin", "obj" ) }

foreach ($dirItem in $devDirs) {
    Write-Warning "Removing '$($dirItem.Name)/' in '$($dirItem.Parent.FullName)'"
    Remove-Item -Path $dirItem.FullName -Recurse -Force
}

if (!$NoRestore) {
    Write-Warning "Running 'dotnet restore `"$($slnPath)`"'"
    dotnet restore "$($slnPath)"
}