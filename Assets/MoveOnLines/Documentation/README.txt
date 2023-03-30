- Pathfinding on the line -

E-mail: reply@tkitfacn.com

1. ABOUT
This tool saves you time to handle move node and finding the path on lines.

2. HOW TO  

*To Move on the line:
- In MenuItem in Editor, select Tools/Move On Lines/Single Line to create new line.
or create a new game object in your scene, add the component "LineGanerate" in this game object.
- Create new child game objects in LineGanerate game object with some positions correspond on your line.
To adjust the line style, you can change in Line Type in LineGanerate game object.
- To move object on the line. In script for move object, connect to the LineGanerate in update function add lineganerate.move(transform, speed)
- View example in MoveOnLines/Demo/MoveNode.
*To use the Pathfinding on the line simply:
- In MenuItem in Editor, select Tools/Move On Lines/New Path to create PathFinding 
or create a new game object in your scene, add the component "PathFinding" in this game object.
- Select PathFinding game object, click button "Create The Line" into the Inspector tab to create a new line.
- You can edit the line by change you line type in "LineBehaviour" component, add new child game object and change this position in the line.
- Create lines near each other.
- To find the way to some position on the line. In your scripts add PathFinding.Find(current.positon, target.position) to return the list position from current start position to the end target position. 
PathFinding.Move(transform, target, speed) in update script to move transform to target 
Or you can drag and drop "Player.prefab" in Prefabs folder to your scene for test.
- View example in MoveOnLines/Demo/PathFinding.