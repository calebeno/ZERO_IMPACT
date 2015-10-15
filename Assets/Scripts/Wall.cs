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
