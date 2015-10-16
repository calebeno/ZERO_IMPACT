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
- Set Game size to 1280x720

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
- Add Code to Wall.cs
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
- Create GameObject "ObjectFactory"
- Add Code to ObjectFactory.cs
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
- Add Wall prefab to script
- Create ObjectFactory Prefab

Building The Whole Wall:  The Game Controller
- GameController manages the game state and builds the game objectes, such as the wall.
- Create GameObject "GameController"
- Add Code to GameController.cs
```c#
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{

	public int levelW;
	public int levelH;
	public Wall wall;

	public int timerDefault;

	private int score;
	private int timer;
	private bool _gameInPlay = true;

	// Use this for initialization
	void Start()
	{
		for (float i = 0; i < levelW; i = i + .5f)
		{
			for (float j = 0; j < levelH; j = j + .5f)
			{
				if (i == 0 || i == levelW - .5f)
				{
					ObjectFactory.CreateWall(i - (levelW / 2), j - (levelH / 2));
				}
				else if (j == 0 || j == levelH - .5f)
				{
					ObjectFactory.CreateWall(i - (levelW / 2), j - (levelH / 2));
				}
			}
		}
	}
}
```
- Set levelW to 18, levelH to 10

####Creating A Player
- Empty GameObject, SpriteRenderer, Player Script
- Polygone Collider (adjust)
- Add Code to Player.cs:
```c#
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	public float speed;

	private GameController gameController;

	// Use this for initialization
	public void Initialize(float x, float y)
	{
		gameController = GameObject.FindObjectOfType<GameController>();
		this.transform.position = new Vector2(x, y);
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (gameController.gameInPlay)
		{
			PlayerMove();
		}
	}
	
	void PlayerMove()
	{
		float currentX = this.transform.position.x;
		float currentY = this.transform.position.y;

		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			if (CheckWallY(Vector2.up, this.transform.position.y + this.GetComponent<SpriteRenderer>().bounds.size.y))
			{
				currentY += speed;
			}
		}
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			if (CheckWallX(Vector2.left, this.transform.position.x))
			{
				currentX -= speed;
			}
		}
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			if (CheckWallY(Vector2.down, this.transform.position.y))
			{
				currentY -= speed;
			}
		}
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			if (CheckWallX(Vector2.right, this.transform.position.x + this.GetComponent<SpriteRenderer>().bounds.size.x))
			{
				currentX += speed;
			}
		}

		Vector3 newPosition = new Vector3(currentX, currentY, 0f);
		this.transform.position = newPosition;
	}
	
	bool CheckWallX(Vector2 vector, float point)
	{
		int layerMask = 1 << LayerMask.NameToLayer("Wall");
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, vector, Mathf.Infinity, layerMask);
		if (hit.collider != null)
		{
			float distance = Mathf.Abs(hit.point.x - point);
			//Debug.Log("X " + distance);
			if (distance <= speed)
			{
				return false;
			}
			return true;
		}
		return true;
	}

	bool CheckWallY(Vector2 vector, float point)
	{
		int layerMask = 1 << LayerMask.NameToLayer("Wall");
		RaycastHit2D hit = Physics2D.Raycast(this.transform.position, vector, Mathf.Infinity, layerMask);
		if (hit.collider != null)
		{
			float distance = Mathf.Abs(hit.point.y - point);
			//Debug.Log("Y " + distance);
			if (distance <= speed)
			{
				return false;
			}
			return true;
		}
		return true;
	}
}
```
- Set speed to 0.04 in unity.
- Make Player a Prefab
- Explain raycasting


Add Player To ObjectFactory
- Add Code to ObjectFactory.cs
```c#
	// Build new player
	public static Player CreatePlayer(float x, float y)
	{
		var obj = (Player)Instantiate(instance.player,
			new Vector3(0f, 0f, 0f), Quaternion.identity);
		obj.Initialize(x, y);
		return obj;
	}
```

Add Player Generation to GameController
- Add Code to GameController.cs
```c#
...
void Start()
	{
		ObjectFactory.CreatePlayer(0f, 0f);
...
```


```c#

```