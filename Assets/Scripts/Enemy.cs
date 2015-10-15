using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

	// Use this for initialization
	public void Initialize(float x, float y, float v)
	{
		this.transform.position = new Vector2(x, y);
		this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, v);
	}
}
