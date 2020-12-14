using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathElement : MonoBehaviour
{
	[SerializeField]
	private GameObject startPoint;
	[SerializeField]
	private GameObject endPoint;
	[SerializeField]
	private TowerSlot towerSlotPrototype;

	private List<TowerSlot> towerLocations = new List<TowerSlot>();

	public Vector3 StartPosition => startPoint.transform.localPosition;
	public Vector3 EndPosition => endPoint.transform.localPosition;
	public TowerSlot[] TowerLocations => towerLocations.ToArray();

	public TowerSlot[] CreateTowerLocations()
	{
		float pathLenght = transform.localScale.y;
		int possibleTowerLocationCount = Mathf.FloorToInt(pathLenght) - 1;

		for (int i = 0; i < possibleTowerLocationCount - 1; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				TowerSlot towerSlot = Instantiate(towerSlotPrototype, transform);
				towerSlot.transform.localPosition = new Vector3(StartPosition.x + (2 * j - 1) / 1.5f, StartPosition.y - (i + 1) * 1f / possibleTowerLocationCount);
				towerLocations.Add(towerSlot);
			}
		}

		return towerLocations.ToArray();
	}
}
