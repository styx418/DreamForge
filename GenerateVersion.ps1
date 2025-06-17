# PowerShell - Script auto-versioning
$buildFile = ".build"
$template = "version.txt.template"
$output = "version.txt"

# Lire le numéro de build actuel
if (Test-Path $buildFile) {
    $build = [int](Get-Content $buildFile)
} else {
    $build = 0
}

# Incrémenter
$build++

# Générer la date actuelle
$date = Get-Date -Format "yyyy-MM-dd HH:mm"

# Remplacer dans le template
$templateContent = Get-Content $template
$templateContent = $templateContent -replace '\{BUILD_NUMBER\}', "$build"
$templateContent = $templateContent -replace '\{DATE\}', "$date"

# Écrire dans le fichier version.txt
$templateContent | Set-Content $output
$build | Set-Content $buildFile

Write-Output "✔ version.txt généré : 0.0.$build - $date"
