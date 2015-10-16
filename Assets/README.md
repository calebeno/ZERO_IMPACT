#TECH SIG TALK:  Basic Unity 2D Development

##Intro to Unity Editor

####Create a new project.  
- Set it to 2D.
- Save Scene as "main_game"

####Create Folders in Assets
- Scripts
- _Scenes
- Sprites
- Prefabs
- SFX
- Materials

####Preparing Assets and Scene
Customizing the Camera
- Set Skybox to black

Import Sprites
- Auto Import the player for multiple
- Set all to bottom-left origin point
- Set wall to 128 ppu
- Player to 64 ppu
- Enemy to 64 ppu

####Building The Wall

Create Prefab for Wall
- Make an empty game object
- Apply SpriteRenderer
- Apply wall sprite 
- Apply BoxCollider
- Add New Script "Wall"
- Make Prefab

Add a few walls to scene
- Cumbersom to add walls manually
- Write a script to do it for us
- Remove all walls

Script Execution Order and Unity Methods
- Scripts Can be forced to execute in a pre-set order
- Edit < Project Settings < Script Execution Order
- `Awake()` Called the instant the GameObject is created
- `Start()` Called after all `Awake()`, has access to all objects in scene
- `Update()` Called Every time the game loop runs.
- `FixedUpdated()` Called once per frame of the game.

Edit the Wall Script
- Wall is added by script, so build custom `Initialization()`
- Requires an x and y coordinate.
```c#
using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{

	// Use this for initialization
	public void Initialize(float x, float y)
	{
		this.transform.position = new Vector3(x, y, 0f);
	}
}
```

Building a Wall:  The Object Factory
- Need an Object Factory to create the wall object and run its `Initialization()`
```c#
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectFactory : MonoBehaviour
{
	//// DESCRIPTION:  The Object Factory handles the creation and instantiation of new objects

	// Holds the instance of the ObjectFactory
	protected static ObjectFactory instance; // Needed

	// Prefab variables, must be manually inserted from Unity UI
	public Wall wall;

	void Awake()
	{
		instance = this;
	}

	// Build new wall
	public static Wall CreateWall(float x, float y)
	{
		var obj = (Wall)Instantiate(instance.wall,
			new Vector3(0f, 0f, 0f), Quaternion.identity);
		obj.Initialize(x, y);
		return obj;
	}
}
```
- Create GameObject "ObjectFactory"
- Add Script
- Add Wall prefab to script
- Create ObjectFactory Prefab


```c#

```