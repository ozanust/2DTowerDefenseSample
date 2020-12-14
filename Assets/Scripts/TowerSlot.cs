using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSlot : MonoBehaviour
{
	private bool isOccupied = false;
	private Tower tower = null;

	public bool IsOccupied => isOccupied;
	public Tower PlacedTower => tower;

	public Vector3 Position => transform.position;
   
	public void SetTower(Tower tower)
	{
		this.tower = tower;
		this.isOccupied = true;
	}
}
