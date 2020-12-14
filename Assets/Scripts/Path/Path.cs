using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
	[SerializeField]
	private PathPoint[] pathPoints;
	[SerializeField]
	private PathElement pathRendererElement;

	private List<PathElement> pathElements = new List<PathElement>();
	private List<TowerSlot> towerSlots = new List<TowerSlot>();

	private void Awake()
	{
		Init();
	}

	private void Init()
	{
		CreatePathRenderer();
		CreateTowerLocations();
	}

	public int GetNextPathPointIndex(int currentPathIndex)
	{
		if (currentPathIndex < pathPoints.Length - 1)
			return currentPathIndex + 1;

		return pathPoints.Length - 1;
	}

	public PathPoint GetNextPathPoint(int currentPathIndex)
	{
		PathPoint pathPoint = null;
		TryGetPathPoint(currentPathIndex + 1, out pathPoint);
		return pathPoint;
	}

	public PathPoint GetPathStart()
	{
		return pathPoints[0];
	}

	public TowerSlot[] GetTowerLocations()
	{
		return towerSlots.ToArray();
	}

	/// <summary>
	/// Having this function is important since getting next path should not depend on the attachment order of path points on editor.
	/// </summary>
	/// <param name="index">Current path index of the moving object.</param>
	/// <param name="pathPoint"></param>
	/// <returns>True if path point exists with desired index.</returns>
	private bool TryGetPathPoint(int index, out PathPoint pathPoint)
	{
		int i = 0;
		while (pathPoints[i].Index != index)
		{
			if (i < pathPoints.Length - 1)
			{
				i++;
			}
			else
			{
				pathPoint = null;
				return false;
			}
		}

		pathPoint = pathPoints[i];
		return true;
	}

	private void CreatePathRenderer()
	{
		for(int i = 0; i < pathPoints.Length - 1; i++)
		{
			Vector3 center = (pathPoints[i + 1].transform.position - pathPoints[i].transform.position) / 2f;
			float pathLength = Vector3.Distance(pathPoints[i + 1].transform.position, pathPoints[i].transform.position);
			PathElement pathElement = Instantiate(pathRendererElement, pathPoints[i].transform.position + center, Quaternion.identity, this.gameObject.transform);
			//pathElement.transform.localScale = new Vector3(Mathf.Abs(center.x * 2) + 1, Mathf.Abs(center.y * 2) + 1, Mathf.Abs(center.z * 2) + 1);
			pathElement.transform.up = pathPoints[i + 1].transform.position - pathPoints[i].transform.position;
			pathElement.transform.localScale = new Vector3(1, Mathf.Abs(pathLength) + 1, 1);
			pathElements.Add(pathElement);
		}
	}

	private void CreateTowerLocations()
	{
		for (int i = 0; i < pathElements.Count; i++)
		{
			TowerSlot[] locations = pathElements[i].CreateTowerLocations();

			for(int j = 0; j < locations.Length; j++)
			{
				towerSlots.Add(locations[j]);
			}
		}
	}
}
