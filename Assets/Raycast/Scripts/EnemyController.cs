using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyStats enemyStats;
	public HealthBar healthBar;

	private float currentHealth;
	private bool isDead = false;

	private void Start() {
		currentHealth = enemyStats.maxHealth;
	}

	public void GetHit(float damage) {
		float damageInflicted = damage * (1 * enemyStats.defence) * Time.deltaTime;

		if (currentHealth - damageInflicted <= 0 && !isDead) {
			isDead = true;
			currentHealth = 0;
			healthBar.SetCurrentLife(currentHealth, enemyStats.maxHealth);
			StartCoroutine(ResetHealth());
		}
		else {
			currentHealth -= damageInflicted;
			healthBar.SetCurrentLife(currentHealth, enemyStats.maxHealth);
		}
	}

	public IEnumerator ResetHealth() {
		yield return new WaitForSeconds(3);
		currentHealth = enemyStats.maxHealth;
		healthBar.SetCurrentLife(currentHealth, enemyStats.maxHealth);
		isDead = false;
	}
}
