using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
	[SerializeField]
	private int index;

	public int Index => index;
	public Vector3 Position => this.gameObject.transform.position;
}
