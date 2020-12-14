using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	private GameManager gameManager;
	private UIManager uiManager;
	private List<Enemy> enemyPool = new List<Enemy>();
	private int killedEnemiesInLevel = 0;
	private int totalKilledEnemies = 0;

	[SerializeField]
	private int[] enemyCountIndex;
	[SerializeField]
	private float spawnPeriod = 1.5f;
	[SerializeField]
	private Enemy enemyPrototype;
	[SerializeField]
	private Tower towerPrototype;
	[SerializeField]
	private Path path;

	private float timer = 0;

	private void Awake()
	{
		gameManager = GameManager.Instance;
		uiManager = UIManager.Instance;
		PopulatePoolBy(enemyCountIndex[gameManager.Level]);
		uiManager.onCreateTowerClicked += CreateTower;
	}

	private void Start()
	{
		StartCoroutine(CSpawnEnemies());
	}

	public void CreateTower()
	{
		TowerSlot[] towerLocations = path.GetTowerLocations();

		int randomPos = Random.Range(0, towerLocations.Length);
		TowerSlot randomlySelectedLocation = towerLocations[randomPos];

		if (randomlySelectedLocation.IsOccupied)
		{
			for(int i = randomPos + 1; i < towerLocations.Length; i++)
			{
				if (!towerLocations[i].IsOccupied)
				{
					randomlySelectedLocation = towerLocations[i];
					Tower tower = Instantiate(towerPrototype, randomlySelectedLocation.Position, Quaternion.identity);
					tower.SetTowerProperties(Random.Range(Constants.MIN_TOWER_POWER, Constants.MAX_TOWER_POWER));

					randomlySelectedLocation.SetTower(tower);
					return;
				}
			}
		}
		else
		{
			Tower tower = Instantiate(towerPrototype, randomlySelectedLocation.Position, Quaternion.identity);
			tower.SetTowerProperties(Random.Range(Constants.MIN_TOWER_POWER, Constants.MAX_TOWER_POWER));

			randomlySelectedLocation.SetTower(tower);
			return;
		}

		Debug.Log("Cant put tower");
	}

	private void PopulatePoolBy(int newEnemyCount)
	{
		int newEnemyToAdd = newEnemyCount - enemyPool.Count;
		for(int i = 0; i < newEnemyToAdd; i++)
		{
			Enemy enemy = Instantiate(enemyPrototype, path.GetPathStart().Position, Quaternion.identity, this.transform);
			enemy.gameObject.name = i + "th enemy";
			enemyPool.Add(enemy);
			enemy.onDie += OnEnemyDie;
		}
	}

	private Enemy GetEnemyFromPool()
	{
		int i = 0;
		while(enemyPool[i].gameObject.activeInHierarchy)
		{
			i++;
		}

		return enemyPool[i];
	}

	IEnumerator CSpawnEnemies()
	{
		for(int i = 0; i < enemyCountIndex[gameManager.Level]; i++)
		{
			GetEnemyFromPool().gameObject.SetActive(true);
			yield return new WaitForSeconds(spawnPeriod);
		}
	}

	private void OnEnemyDie(Enemy diedEnemy)
	{
		diedEnemy.gameObject.SetActive(false);
		diedEnemy.gameObject.transform.position = path.GetPathStart().Position;
		killedEnemiesInLevel++;
		totalKilledEnemies++;
		uiManager.onScore(totalKilledEnemies.ToString());
		SetLevelStatus();
	}

	private void SetLevelStatus()
	{
		if(killedEnemiesInLevel == enemyCountIndex[gameManager.Level])
		{
			killedEnemiesInLevel = 0;
			gameManager.LevelPass();
			PopulatePoolBy(enemyCountIndex[gameManager.Level]);
			StartCoroutine(CSpawnEnemies());
		}
	}
}
