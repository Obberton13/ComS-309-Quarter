# ComS-309-Quarter

## Important git information

**Only one person edits scenes at a time.**

Conflicts in scene (.unity) files are rediculously hard to resolve, unless you have Unity Pro's diff tool.

### TO CLONE:

1. cd to the directory you want the project to be in (mine is D:/projects/\_school/Com S 309/QuarterGame)
2. git clone https://github.com/Obberton13/ComS-309-Quarter.git
	* This will set up the above URL as the 'origin' remote automatically
3. *In explorer* navigate to QuarterGame/Assets/Scenes
4. Double-click defaultScene.unity to open Unity and create all other necessary files.

### Uncommitted Experimentation/Testing
Anything that does not need to be in the final game (tests, scenes that you made just to try something out, etc.) should be put into a 'dev' folder directly in the Assets folder. This entire folder will be ignored by git.

### Ignoring Files
You should not have to worry about having to ignore anything in git, that should all be set up already.

### New Scripts/Assets
When committing new scripts, be sure to add both the .cs and the .cs.meta files. Same goes with other file extensions for things that aren't scripts.

### Models
3d models should not be committed unless they are finished or in use (to keep git fast).