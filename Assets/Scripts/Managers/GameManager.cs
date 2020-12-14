using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoManager<GameManager>
{
	[SerializeField]
	private Path currentGamePath;

	private int level = 0;

	public Path GamePath => currentGamePath;
	public int Level => level;

	public void GameOver()
	{
		Debug.Log("Game over!");
	}

	public void LevelPass()
	{
		level++;
	}
}
