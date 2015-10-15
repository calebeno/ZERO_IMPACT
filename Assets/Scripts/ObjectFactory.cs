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
	public Enemy enemy;

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

	// Build new wall
	public static Enemy CreateEnemy(float x, float y, float vX, float vY)
	{
		var obj = (Enemy)Instantiate(instance.enemy,
			new Vector3(0f, 0f, 0f), Quaternion.identity);
		obj.Initialize(x, y, vX, vY);
		return obj;
	}
}
