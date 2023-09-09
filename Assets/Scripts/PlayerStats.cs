using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameParams;
using static TagHolder;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats SharedInstance;
    int health;
    ulong score;
    Animator animator;

    void Awake()
    {
        SharedInstance = this;

        animator = GetComponent<Animator>();
    }

    public ulong GetScore()
    {
        return score;
    }

    public void SetScore(ulong sc)
    {
        score = sc;
    }
    
    void Start()
    {
        health = maxHealth;
        score = 0;
        UI.SharedInstance.SetScoreCount(score);
        UI.SharedInstance.SetHealthBar(health);
    }

    void Update()
    {
        if (health <= 0)
            animator.SetBool(IS_DEAD, true);
    }

    public void IncrementScore()
    {
        score++;
        UI.SharedInstance.SetScoreCount(score);
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
        
    }
}
