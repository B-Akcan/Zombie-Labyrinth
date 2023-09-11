using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameParams;
using static TagHolder;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats SharedInstance;
    int health;
    uint score;
    Animator animator;
    bool isDead;

    public bool PlayerIsDead()
    {
        return isDead;
    }

    public uint GetScore()
    {
        return score;
    }

    public void SetScore(uint sc)
    {
        score = sc;
    }

    void Awake()
    {
        SharedInstance = this;

        animator = GetComponent<Animator>();
    }
    
    void Start()
    {
        health = maxHealth;
        score = 0;
        UI.SharedInstance.SetScoreCount(score);
        UI.SharedInstance.SetHealthBar(health);

        isDead = false;
    }

    public void IncrementScore()
    {
        score++;
        UI.SharedInstance.SetScoreCount(score);
        EnemyPool.SharedInstance.AdjustEnemySpawnTimer();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, 100);
        UI.SharedInstance.SetHealthBar(health);

        if (health == 0)
            Die();
    }

    void Die()
    {
        animator.SetBool(IS_DEAD, true);
        isDead = true;
        Cursor.lockState = CursorLockMode.None;
        PlayerController.SharedInstance.StopAllSounds();
        UI.SharedInstance.ActivateEndGameUI();
    }
}
