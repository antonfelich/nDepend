# Current state

1. Build the project
2. Run analyze.ps1 (this generates the baseline report for comparison)
3. Modify a method so so that the end result has more than ten lines of code OR add a new method with more than 10 lines of code
4. Build again
5. Run analyze.ps1

expectged result: an error should be output


- [X] Create a rule that fails the build if you touch a method and it ends up being more than 10 lines of code or add a new method with more than ten lines
- [X] Write a build script for the target project (inside _analyse.ps1_ probably)
- [X] Convert it to use S3 to store the _previous.ndar_ file (keyed by solution name probably)
- [X] Before running analysis, grab the previous from S3
- [X] Need to modify the backup process so it grabs the latest report by date, not by name
- [ ] Possible issue in that we save previous.ndar regardless of the results, therefore commiting again = no fail because its the same (aka. only backup on success)
- [ ] Move to monolith
- [ ] Get in Phabricator (how do we get _nDepend.exe_ on the build agent?)
- [ ] Come up with a decent message that makes sense to the end user, list the methods that are violating the rule
- [ ] Fix up the rule so that its 10 lines (we have set to 3 for testing)