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
