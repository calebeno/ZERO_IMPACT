using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

	public float speed;


	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void FixedUpdate()
	{
		PlayerMove();
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
			Debug.Log("X " + distance);
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
			Debug.Log("Y " + distance);
			if (distance <= speed)
			{
				return false;
			}
			return true;
		}
		return true;
	}
}
