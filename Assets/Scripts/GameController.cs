using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour
{

	public int levelW;
	public int levelH;
	public Wall wall;
	public Enemy enemy;
	public Player player;
	public Text scoreWriter;
	public Image gameOver;
	public Button start;
	public Button quit;
	public Canvas canvas;
	public GameObject disabled;

	public int count;
	public int timerDefault;

	private int score;
	private int timer;
	private bool _gameInPlay = true;

	// Use this for initialization
	void Start()
	{
		ObjectFactory.CreatePlayer(0f, 0f);
		gameOver.transform.SetParent(disabled.transform);
		start.transform.SetParent(disabled.transform);
		quit.transform.SetParent(disabled.transform);

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

		while (count > 0)
		{
			BuildEnemy();
			count--;
		}

		timer = timerDefault;
	}

	// Update is called once per frame
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
	}

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

	public bool gameInPlay
	{
		get { return _gameInPlay; }
		set { _gameInPlay = value; }
	}
}
