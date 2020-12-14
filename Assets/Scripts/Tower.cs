using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tower : MonoBehaviour
{
	[SerializeField]
	private Bullet bulletPrototype;
	[SerializeField]
	private List<Bullet> bulletPool;
	[SerializeField]
	private TMP_Text powerText;

	private float power;
	private float timer = 0;

	private GameObject lockedEnemy = null;
	private Queue<GameObject> enemiesToLock = new Queue<GameObject>();

	public void SetTowerProperties(int dps)
	{
		power = dps / Constants.BULLETS_PER_SECOND;
		powerText.text = string.Format("{0} {1}", "DPS:", dps);
		PopulatePoolBy(Constants.BULLETS_PER_SECOND * 2, power);
	}

	private void FixedUpdate()
	{
		if (lockedEnemy != null)
		{
			timer += Time.deltaTime;
			if (timer >= 1f / Constants.BULLETS_PER_SECOND)
			{
				timer = 0;
				Fire();
			}
		}
		else if (timer > 0)
		{
			timer = 0;
		}
	}

	public void Fire()
	{
		Bullet bullet = GetBulletFromPool();
		bullet.gameObject.SetActive(true);
		bullet.SetDirection(lockedEnemy.gameObject.transform.position);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == Constants.ENEMY_TAG && lockedEnemy == null)
		{
			lockedEnemy = collision.gameObject;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == Constants.ENEMY_TAG && collision.gameObject.Equals(lockedEnemy))
		{
			lockedEnemy = null;
		}
	}

	private Bullet GetBulletFromPool()
	{
		int i = 0;
		while (bulletPool[i].gameObject.activeInHierarchy)
		{
			i++;
		}

		return bulletPool[i];
	}

	private void PopulatePoolBy(int bulletPerSecond, float bulletPower)
	{
		for (int i = 0; i < bulletPerSecond; i++)
		{
			Bullet bullet = Instantiate(bulletPrototype, this.gameObject.transform.position, Quaternion.identity);
			bullet.onHit += OnBulletHit;
			bulletPool.Add(bullet);
			bullet.SetPower(bulletPower);
		}
	}

	private void OnBulletHit(Bullet bullet)
	{
		bullet.gameObject.SetActive(false);
		bullet.gameObject.transform.localPosition = this.transform.position;
	}
}
