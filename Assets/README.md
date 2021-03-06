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
- Put Wall on the 8th Layer

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
- Put Enemy on the 9th Layer
- Explain raycasting


Add Player To ObjectFactory
- Add Code to ObjectFactory.cs
```c#
...
public Player player;
...
	// Build new player
	public static Player CreatePlayer(float x, float y)
	{
		var obj = (Player)Instantiate(instance.player,
			new Vector3(0f, 0f, 0f), Quaternion.identity);
		obj.Initialize(x, y);
		return obj;
	}
...
```
- Add Player Prefab to ObjectFactory in Unity

Add Player Generation to GameController
- Add Code to GameController.cs
```c#
...
public Player player;
...
private bool _gameInPlay = true;
...
void Start()
	{
		ObjectFactory.CreatePlayer(0f, 0f);
...
public bool gameInPlay
	{
		get { return _gameInPlay; }
		set { _gameInPlay = value; }
	}
...
```

####Add an Enemy
- Empty GameObject, SpriteRenderer, Enemy Script, Box Collider 2D
- Create Bouncy Physics2DMaterial, Friction 0, bounciness 1
- Add Bouncy Material to Collider
- RigidBody2D, Mass 1, Drag 0, Gravity 0
- Add Code to Enemy.cs:
```c#
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

	public float constantSpeed;

	// Use this for initialization
	public void Initialize(float x, float y, float vX, float vY)
	{
		this.transform.position = new Vector2(x, y);
		this.GetComponent<Rigidbody2D>().velocity = new Vector2(vX, vY);
	}

	void Update()
	{
		this.GetComponent<Rigidbody2D>().velocity = constantSpeed * (this.GetComponent<Rigidbody2D>().velocity.normalized);
	}
}
```
- Make Enemy a Prefab
- Put Enemy on the 10th Layer

Add Enemy to Object Factory
- Add Code to ObjectFactory.cs
```c#
...
	public Enemy enemy;
...
	// Build new enemy
	public static Enemy CreateEnemy(float x, float y, float vX, float vY)
	{
		var obj = (Enemy)Instantiate(instance.enemy,
			new Vector3(0f, 0f, 0f), Quaternion.identity);
		obj.Initialize(x, y, vX, vY);
		return obj;
	}
...
```
- Add Enemy Prefab to ObjectFactory in Unity

Generate Enemies
- Create a count to generate initial enemies
- Add Code to GameController.cs:
```c#
...
public Enemy enemy;
...
public int count;
...
while (count > 0)
		{
			BuildEnemy();
			count--;
		}

		timer = timerDefault;
...
// FixedUpdate is called once per frame
	void FixedUpdate()
	{
		if (timer <= 0)
		{
			BuildEnemy();
			timer = timerDefault;
		}
		else
		{
			timer--;
		}
	}
...
void BuildEnemy()
	{
		float wallsize = wall.GetComponent<SpriteRenderer>().bounds.size.x;
		float enemysize = enemy.GetComponent<SpriteRenderer>().bounds.size.x;

		float x = Random.Range(-(levelW / 2) + wallsize, levelW - (levelW / 2) - enemysize); ;
		float y = Random.Range(-(levelH / 2) + wallsize, levelH - (levelH / 2) - enemysize);

		int vX = 0;
		int vY = 0;

		while (vX == 0)
		{
			vX = Random.Range(-1, 1);
		}
		while (vY == 0)
		{
			vY = Random.Range(-1, 1);
		}
		vX = vX * 5;
		vY = vY * 5;

		ObjectFactory.CreateEnemy(x, y, vX, vY);
	}
...
```
- Set count to 5 and timerDefault to 60, and set enemy to Enemy prefab in Unity

####Player Response to Hits
- Add Code to Player.cs
```c#
...
	public Sprite full;
	public Sprite dam1;
	public Sprite dam2;
	public Sprite end;
...
void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.name == "Enemy(Clone)")
		{
			if (this.GetComponent<SpriteRenderer>().sprite == full)
			{
				this.GetComponent<SpriteRenderer>().sprite = dam1;
			}
			else if (this.GetComponent<SpriteRenderer>().sprite == dam1)
			{
				this.GetComponent<SpriteRenderer>().sprite = dam2;
			}
			else if (this.GetComponent<SpriteRenderer>().sprite == dam2)
			{
				this.GetComponent<SpriteRenderer>().sprite = end;
				gameController.gameInPlay = false;
			}
		}
	}
...
```
- Add sprites to Player Prefab in Unity

####Adding Menus
- Create New Scene, "start_menu"

Create LevelController
- New GameObject, LevelController script
- Add Code to LevelController.cs
```c#
using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour
{

	public void LoadLevel(string name)
	{
		Application.LoadLevel(name);
	}

	public void ReloadLevel()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void LoadNextLevel()
	{
		Application.LoadLevel(Application.loadedLevel + 1);
	}

	public void QuitRequest()
	{
		Application.Quit();
	}
}
```
- Add Canvas, Image, and Buttons
- Add function calls to Buttons
- Build Order

####Adding Score and Endgame
- Add text element to Canvus
- Add Code to GameController.cs
```c#
...
public Text scoreWriter;
...
if (gameInPlay)
		{
			score++;
			scoreWriter.text = "Score: " + score.ToString();
		}
...
```

Adding Hidden UI Elements
- Place Game Over Image
- Copy Menu buttons
- Add GameObject "Disabled"
- Add Code to GameController.cs
```c#
...
public Image gameOver;
public Button start;
public Button quit;
public Canvas canvas;
public GameObject disabled;
...
ObjectFactory.CreatePlayer(0f, 0f);
gameOver.transform.SetParent(disabled.transform);
start.transform.SetParent(disabled.transform);
quit.transform.SetParent(disabled.transform);
...
if (gameInPlay)
{
	score++;
	scoreWriter.text = "Score: " + score.ToString();
}
else
{
	if (gameOver.transform.parent != canvas.transform)
	{
		gameOver.transform.SetParent(canvas.transform);
		start.transform.SetParent(canvas.transform);
		quit.transform.SetParent(canvas.transform);
	}
}
...
```

####Build And Run the Game
- Got to Build
- Set the Settings
- Build and Run

####Audio
- GameObject, MusicPlayerScript, Audio Source Component
- Audio Component To Retro, Play on Awake, Loop
- Add Code to MusicPlayer.cs
```c#
using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{

	static MusicPlayer instance = null;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
		}
	}
}
```

Sound Effects on Hit
- Add Code to Player.cs
```c#
public AudioClip hitSound;
...
if (col.gameObject.name == "Enemy(Clone)")
		{
			if (gameController.gameInPlay)
			{
				AudioSource.PlayClipAtPoint(hitSound, transform.position, 0.5f);
			}
```
- Set hitSound in Unity
