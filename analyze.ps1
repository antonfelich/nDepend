<#

// <QualityGate Name="You Touched it Last - Method Length" Unit="methods" />
failif count > 0 methods
from  m in Methods
where m.NbLinesOfCode > 3
where m.CodeWasChanged()
select new { m, m.NbLinesOfCode }

#>
# param
# (
#     [Parameter(Mandatory=$True)][string]$AWSAccessKey,
#     [Parameter(Mandatory=$True)][string]$AWSSecretKey,
#     [Parameter(Mandatory=$True)][string]$AWSSessionToken
# )

$AWSAccessKey = ''
$AWSSecretKey = ''
$AWSSessionToken = ''

$root = pwd
$nDepend = ".\NDepend.Console.exe"
$targetFile =  "$root\TestSolution\TestSolution.ndproj"
$projectFolder = Split-Path -Path $targetFile
$outputFolder = "nDepend.Reports"

$previous = ""
Clear-Host

function BackupSuccessfulReport()
{
	if (Test-Path $projectFolder\$outputFolder\*.ndar)
	{
		Write-Output "Backing up previous NDAR report"
		$absoluteSearchPath = "$projectFolder\$outputFolder\"
		$relativeSearchPath = Resolve-Path -Relative $absoluteSearchPath

		$latestReport = (Get-ChildItem -Filter "$relativeSearchPath\*.ndar" | Select-Object -First 1).Name

		$targetBucket = "ndepend-reports"

		$targetFilename = iex "git rev-parse head"

		Import-Module AWSPowershell
		Set-AWSCredentials -AccessKey $AWSAccessKey -SecretKey $AWSSecretKey -SessionToken $AWSSessionToken

		Write-Output "Source file: $latestReport"
		Write-Output "Target bucket: $targetBucket"
		Write-Output "Target bucket: $targetFilename"

		Write-S3Object -BucketName $targetBucket -File "$absoluteSearchPath\$latestReport" -Key "$targetFilename.ndar" -Region ap-southeast-2
		Copy-Item $projectFolder\$outputFolder\*.ndar $projectFolder\previous.ndar

		$previous = ".\previous.ndar"
	}
}

function AnalyseSolution()
{
	& $nDepend $targetFile /Silent /OutDir .\$outputFolder /AnalysisResultToCompareWith .\previous.ndar
	Write-Host $LASTEXITCODE
}

Write-Output "Building solution..."

.\build.ps1

Write-Output "Analysing: - $targetFile"
Write-Output "ProjectFolder: - $projectFolder"

BackupSuccessfulReport

#The output path appears to be relative to the .ndproj file

# Capture the failure code from below
AnalyseSolution


# If success then copy current.ndar over previous.ndar and backup history