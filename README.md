# Current state

1. Build the project
2. Run `analyze.ps1` or `analyze.ps1 -Baseline $true` if you don't yet have a baseline
3. Modify a method so so that the end result has more than ten lines of code OR add a new method with more than 10 lines of code
4. Build again
5. Run `analyze.ps1` or `analyze.ps1 -Diagnostics $false` to hide the nDepend output

Expected result: an error should be output base on the quality gate 'You Touched it Last'


- [X] Create a rule that fails the build if you touch a method and it ends up being more than 10 lines of code or add a new method with more than ten lines
- [X] Write a build script for the target project (inside _analyse.ps1_ probably)
- [X] Convert it to use S3 to store the _previous.ndar_ file (keyed by solution name probably)
- [X] Before running analysis, grab the previous from S3
- [X] Need to modify the backup process so it grabs the latest report by date, not by name
- [X] Have the ndepend bucket use separate folders per repository
- [X] Possible issue in that we save previous.ndar regardless of the results, therefore commiting again = no fail because its the same (aka. only backup on success)
- Get in Build Agent
    - [X] Talk to nDepend and exchange a dev license for a stand alone exe
    - [X] Zip up the exe and add it to the build-agent-bootstrap repo (along with the changes in the Build.ps1)
    - [X] COMMIT the changes so when you run Build.ps1 it pulls the changes
    - [X] Make the new image
    - [X] Deploy as per [This](https://docs.google.com/document/d/1WOjo1IMgukiE58jiM4WNPvDZ9DS300Qa7f-ZZMHSPrk/edit?ts=5977d745)
- Move to monolith
    - [X] Get the build.ps1 to execute the Analyse.ps1 file
    - [X] Add the -TestCodeQuality flag = $true to the build plan (This will still do nothing as we have a dummy Analyse.ps1 file)
    - [ ] Replace the sample Analyse.ps1 file with the real thing
    - [ ] Change Analyse.ps1 so it gets the AWS keys from passphrase
    - [ ] Update the tooling paths in Analyse.ps1
    - [ ] Add a rule and take a baseline
    - [ ] Talking to TLG + Floor (TCSC) - Decide on the rules, when should it run? (Diff or RC or Licences for everybody)
    - [ ]
- [ ] Come up with a decent message that makes sense to the end user, list the methods that are violating the rule
- [ ] Fix up the rule so that its 10 lines (we have set to 3 for testing)