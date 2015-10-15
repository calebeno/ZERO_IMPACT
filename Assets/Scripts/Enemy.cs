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
