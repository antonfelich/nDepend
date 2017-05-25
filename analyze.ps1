<#

// <QualityGate Name="You Touched it Last - Method Length" Unit="methods" />
failif count > 1 methods
from  m in Methods
where m.NbLinesOfCode > 10
where m.WasChanged()
select new { m, m.NbLinesOfCode }

#>

$nDepend = "C:\Users\jimp\Downloads\ndepend\NDepend.Console.exe"
$targetFile =  "C:\Users\jimp\dev\nDepend\TestSolution\TestSolution.ndproj"
$projectFolder = Split-Path -Path $targetFile
$outputFolder = "nDepend.Reports"

$previous = ""
Clear-Host


if (Test-Path $projectFolder\$outputFolder\*.ndar)
{
	Write-Output "Backing up previous NDAR report"
	Copy-Item $projectFolder\$outputFolder\*.ndar $projectFolder\previous.ndar
	
	$previous = ".\previous.ndar"
}

#The output path appears to be relative to the .ndproj file
& $nDepend $targetFile /Silent /OutDir .\$outputFolder /AnalysisResultToCompareWith .\previous.ndar