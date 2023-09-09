using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameParams;
using static TagHolder;

public class Enemy : MonoBehaviour
{
    int health;
    Vector3 playerPosition;
    float distance;
    WaitForSeconds attackDelay;
    bool canMove;

    void Start()
    {
        health = maxHealth;
        attackDelay = new WaitForSeconds(enemyAttackDuration);
        canMove = true;
    }

    void Update()
    {
        if (canMove && !PlayerStats.SharedInstance.playerIsDead)
            Move();
    }

    void Move()
    {
        playerPosition = PlayerStats.SharedInstance.gameObject.transform.position;
        gameObject.transform.LookAt(playerPosition);

        transform.Translate(Vector3.forward * enemySpeed * Time.deltaTime);

        distance = Vector3.Distance(playerPosition, transform.position);

        if (distance <= enemyRange)
            Attack();
    }

    void Attack()
    {
        canMove = false;
        PlayerStats.SharedInstance.TakeDamage(enemyDamage);
        StartCoroutine(WaitAttack());
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, 100);

        if (health == 0)
            Die();
    }

    void Die()
    {
        gameObject.SetActive(false);
        health = maxHealth;

        PlayerStats.SharedInstance.IncrementScore();
    }

    IEnumerator WaitAttack()
    {
        yield return attackDelay;

        canMove = true;
    }
}
