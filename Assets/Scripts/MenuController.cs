using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{

	public int levelW;
	public int levelH;
	public Wall wall;
	public Enemy enemy;
	public int count;

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

		while (count > 0)
		{
			BuildEnemy();
			count--;
		}
	}

	void BuildEnemy()
	{
		float wallsize = wall.GetComponent<SpriteRenderer>().bounds.size.x;
		float enemysize = enemy.GetComponent<SpriteRenderer>().bounds.size.x;

		float x = Random.Range(-(levelW / 2) + wallsize, levelW - (levelW / 2) - enemysize);
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

}
