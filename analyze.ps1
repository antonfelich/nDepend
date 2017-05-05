<#

// <QualityGate Name="You Touched it Last - Method Length" Unit="methods" />
failif count > 1 methods
from  m in Methods
where m.NbLinesOfCode > 10
where m.WasChanged()
select new { m, m.NbLinesOfCode }

#>

$nDepend = "C:\_DEVELOPMENT\nDepend\NDepend.Console.exe"
$targetFile = "C:\_DEVELOPMENT\AssemblyToTest\CodeChallenge.Domain.ndproj"
$projectFolder = Split-Path -Path $targetFile
$outputFolder = "nDepend.Reports"

$previous = ""
Clear-Host

# See if we already have a .ndar file in the output folder, if we do back it up so we can do a comparison
if (Test-Path $projectFolder\$outputFolder\*.ndar)
{
	Write-Output "Backing up previous NDAR report"
	Copy-Item $projectFolder\$outputFolder\*.ndar $projectFolder\previous.ndar
	$previous = ".\previous.ndar"
}

#The output path appears to be relative to the .ndproj file
& $nDepend $targetFile /Silent /OutDir .\$outputFolder /AnalysisResultToCompareWith .\previous.ndar