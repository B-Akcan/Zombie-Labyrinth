using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameParams;
using static TagHolder;

public class PlayerStats : MonoBehaviour
{
    int health;
    ulong score;
    Animator animator;

    void Awake()
    {
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

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int hl)
    {
        health = hl;
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
}
