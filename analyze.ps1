<#

// <QualityGate Name="You Touched it Last - Method Length" Unit="methods" />
failif count > 0 methods
from  m in Methods
where m.NbLinesOfCode > 3
where m.CodeWasChanged()
select new { m, m.NbLinesOfCode }

#>

$root = pwd
$nDepend = ".\NDepend.Console.exe"
$targetFile =  "$root\TestSolution\TestSolution.ndproj"
$projectFolder = Split-Path -Path $targetFile
$outputFolder = "nDepend.Reports"

$previous = ""
Clear-Host
Write-Output "Analysing: - $targetFile"
Write-Output "ProjectFolder: - $projectFolder"

if (Test-Path $projectFolder\$outputFolder\*.ndar)
{
	Write-Output "Backing up previous NDAR report"
	Copy-Item $projectFolder\$outputFolder\*.ndar $projectFolder\previous.ndar

	$previous = ".\previous.ndar"
}

#The output path appears to be relative to the .ndproj file
& $nDepend $targetFile /Silent /OutDir .\$outputFolder /AnalysisResultToCompareWith .\previous.ndar