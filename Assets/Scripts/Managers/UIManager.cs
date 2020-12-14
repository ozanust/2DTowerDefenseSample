using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoManager<UIManager>
{
	[SerializeField]
	private Button createTowerButton;
	[SerializeField]
	private Text scoreText;

	public Action onCreateTowerClicked;
	public Action<string> onScore;

	private void Awake()
	{
		createTowerButton.onClick.AddListener(OnCreateTowerClick);
		onScore += UpdateScoreText;
	}

	private void OnCreateTowerClick()
	{
		onCreateTowerClicked?.Invoke();
	}

	private void UpdateScoreText(string newScore)
	{
		scoreText.text = string.Format("{0} {1}", "Score:", newScore);
	}
}
