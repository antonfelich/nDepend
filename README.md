# Current state

1. Build the project
2. Run analyze.ps1 (this generates the baseline report for comparison)
3. Modify a method so so that the end result has more than ten lines of code OR add a new method with more than 10 lines of code
4. Build again
5. Run analyze.ps1

expectged result: an error should be output


- [X] Create a rule that fails the build if you touch a method and it ends up being more than 10 lines of code or add a new method with more than ten lines
- [ ] Write a build script for the target project (inside _analyse.ps1_ probably)
- [ ] Convert it to use S3 to store the _previous.ndar_ file (keyed by solution name probably)
- [ ] Move to monolith
- [ ] Get in Phabricator (how do we get _nDepend.exe_ on the build agent?)