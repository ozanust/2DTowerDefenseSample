using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
	private GameManager gameManager;
	private Path path;

	private float health = 100;
	private int pathIndex = -1;

	[SerializeField]
	private float speed = 1;

	[SerializeField]
	private TMP_Text healthText;

	private Vector3 targetPathPosition;
	public float Health => health;

	private Action pathPointReached;
	private Action pathCompleted;
	public Action<Enemy> onDie;

	private void Awake()
	{
		gameManager = GameManager.Instance;
		path = gameManager.GamePath;

		pathPointReached += OnPathPointReached;
		pathCompleted += gameManager.GameOver;
	}

	private void OnEnable()
	{
		OnPathPointReached();
	}

	private void FixedUpdate()
	{
		transform.Translate((targetPathPosition - transform.position) * Time.deltaTime * speed / (Vector3.Distance(targetPathPosition, transform.position) > 0 ? Vector3.Distance(targetPathPosition, transform.position) : 1), Space.World);
		if (Vector3.Distance(targetPathPosition, transform.position) < Constants.EPSILON)
			pathPointReached?.Invoke();
	}

	private void OnPathPointReached()
	{
		PathPoint nextPathPoint = path.GetNextPathPoint(pathIndex);
		pathIndex++;

		if (nextPathPoint != null)
			targetPathPosition = nextPathPoint.Position;
		else
			pathCompleted?.Invoke();
	}

	public void Hit(float firePower)
	{
		health = health - firePower;
		healthText.text = Mathf.CeilToInt(health).ToString();
		if (health <= 0)
			Die();
	}

	private void Die()
	{
		//fade out and call onDie here
		pathIndex = -1;
		health = 100;
		healthText.text = Mathf.CeilToInt(health).ToString();
		onDie?.Invoke(this);
	}
}
