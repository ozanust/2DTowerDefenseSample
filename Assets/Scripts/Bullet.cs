using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField]
	private float speed = 2;
	private float power;
	private float deathTimer = 0;

	private Vector3 targetPathPosition;

	public Action<Bullet> onHit;

	private void OnEnable()
	{
		StartCoroutine(CStartDeathTimer());
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private void FixedUpdate()
	{
		transform.Translate(targetPathPosition * Time.deltaTime * speed, Space.World);
	}

	IEnumerator CStartDeathTimer()
	{
		while(deathTimer < 0.9f)
		{
			deathTimer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		deathTimer = 0;
		onHit?.Invoke(this);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(string.Equals(collision.gameObject.tag, Constants.ENEMY_TAG))
		{
			collision.gameObject.GetComponent<Enemy>().Hit(power);
			onHit?.Invoke(this);
		}
	}

	public void SetPower(float power)
	{
		this.power = power;
	}

	public void SetDirection(Vector3 targetPosition)
	{
		targetPathPosition = targetPosition - transform.position;
	}
}
