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
$targetBucket = "ndepend-reports"
$absoluteReportPath = "$projectFolder\$outputFolder\"
$relativeReportPath = Resolve-Path -Relative $absoluteReportPath


$previous = ""
Clear-Host

Import-Module AWSPowershell
Set-AWSCredentials -AccessKey $AWSAccessKey -SecretKey $AWSSecretKey -SessionToken $AWSSessionToken

function BackupSuccessfulReportToS3()
{
	if (Test-Path $projectFolder\$outputFolder\*.ndar)
	{
		Write-Output "Backing up previous NDAR report to S3"
		$latestReport = (Get-ChildItem -Filter "$relativeReportPath\*.ndar" | Select-Object -First 1).Name
		$targetFilename = iex "git rev-parse head"

		Write-Output "Source file: $latestReport"
		Write-Output "Target bucket: $targetBucket"
		Write-Output "Target file: $targetFilename.ndar"

		Write-S3Object -BucketName $targetBucket -File "$absoluteReportPath\$latestReport" -Key "$targetFilename" -Region ap-southeast-2
		Copy-Item $projectFolder\$outputFolder\*.ndar $projectFolder\previous.ndar

		$previous = ".\previous.ndar"
	}
}

function RestoreLatestReportFromS3()
{
	Write-Output "Restoring latest NDAR report from S3"
	Write-Output "Source bucket: $targetBucket"
	$sourceFilename = (Get-S3Object -BucketName $targetBucket | Sort-Object LastModified -Descending | Select-Object -First 1).Key
	Write-Output "Source file: $sourceFilename"

	Read-S3Object -BucketName $targetBucket -File "$absoluteReportPath\$sourceFilename" -Key "$sourceFilename" -Region ap-southeast-2
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

BackupSuccessfulReportToS3

#The output path appears to be relative to the .ndproj file

# Capture the failure code from below
AnalyseSolution


# If success then copy current.ndar over previous.ndar and backup history