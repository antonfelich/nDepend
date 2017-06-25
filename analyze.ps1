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

param
(
	[Parameter(Mandatory=$False)][string]$BaselineFilePath
)

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

function BackupBaselineReportToS3()
{
	BackupReportToS3 $BaselineFilePath "baseline"
}

function BackupSuccessfulReportToS3()
{
	if (Test-Path $projectFolder\$outputFolder\*.ndar)
	{
		$latestReport = (Get-ChildItem -Filter "$relativeReportPath\*.ndar" | Select-Object -First 1).Name
		$targetFileKey = iex "git rev-parse head"

		BackupReportToS3 "$absoluteReportPath$latestReport" $targetFileKey
	}
}

function BackupReportToS3([Parameter(Mandatory=$True)][string]$sourceFilePath, [Parameter(Mandatory=$True)][string]$targetFileKey)
{
	Write-Host "Backing up NDAR report to S3 by filepath [$sourceFilePath] to [$targetFileKey]"

	Write-Host "Source file: $sourceFilePath"
	Write-Host "Target bucket: $targetBucket"
	Write-Host "Target file key: $targetFileKey"

	Write-S3Object -BucketName $targetBucket -File $sourceFilePath -Key "$targetFileKey" -Region ap-southeast-2

	Write-Host "File backed up to S3 successfully"
}


function RestoreLatestReportFromS3()
{
	try
	{
		$sourceFileKey = iex "git rev-parse head~1"
		return RestoreReportFromS3 $sourceFileKey
	}
	catch
	{
		if ($_.Exception.Message -eq "The specified key does not exist.")
		{
			$sourceFileKey = "baseline"
			return RestoreReportFromS3 $sourceFileKey
		}

		Write-Host $_.Exception.Message
		Exit
	}
}

function RestoreReportFromS3([Parameter(Mandatory=$True)][string]$sourceFileKey)
{
	Write-Host "Restoring NDAR report from S3 by key [$sourceFileKey]"
	Write-Host "Source bucket: $targetBucket"

	$sourceFilename = "$sourceFileKey.ndar"
	Write-Host "Source file: $sourceFilename"

	Read-S3Object -BucketName $targetBucket -File "$absoluteReportPath$sourceFilename" -Key "$sourceFileKey" -Region ap-southeast-2 | out-null
	return "$absoluteReportPath$sourceFilename"
}

function AnalyseSolution([Parameter(Mandatory=$True)][string]$previousFilename)
{
	& $nDepend $targetFile /Silent /OutDir .\$outputFolder /AnalysisResultToCompareWith $previousFilename
	return $LASTEXITCODE
}

if (![string]::IsNullOrEmpty($BaselineFilePath))
{
	BackupBaselineReportToS3
	exit
}

Write-Host "Building solution..."
.\build.ps1 | out-null

#Download the previous comparison file (or get baseline if there isn't one)
$previous = RestoreLatestReportFromS3

# Run the comparison
Write-Host "Analysing: - $targetFile"
Write-Host "ProjectFolder: - $projectFolder"
$result = AnalyseSolution $previous

# If success then copy current.ndar over previous.ndar and backup history
if ($result -eq 0)
{
	BackupSuccessfulReportToS3
}