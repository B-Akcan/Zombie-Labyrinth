using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static GameParams;
using static TagHolder;

public class Enemy : MonoBehaviour
{
    int health;
    Vector3 playerPosition;
    Vector3 relativePosition;
    Quaternion finalRotation;
    float distance;
    CharacterController controller;
    WaitForSeconds attackDelay;
    WaitForSeconds dieDelay;
    bool canMove;
    Animator animator;
    AudioSource audioSource;
    [SerializeField] AudioClip searching1;
    WaitForSeconds searching1_length;
    [SerializeField] AudioClip searching2;
    WaitForSeconds searching2_length;
    List<AudioClip> searchingClips;
    [SerializeField] AudioClip attack1;
    WaitForSeconds attack1_length;
    [SerializeField] AudioClip attack2;
    WaitForSeconds attack2_length;
    List<AudioClip> attackClips;
    [SerializeField] AudioClip dead;
    WaitForSeconds dead_length;
    [SerializeField] AudioClip headshot;
    WaitForSeconds headshot_length;
    AudioClip clip;
    IEnumerator coroutine;
    WaitForSeconds soundDelay;
    float volume;
    bool canSpeak;
    bool isAlive;


    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        health = maxHealth;
        attackDelay = new WaitForSeconds(enemyAttackDuration);
        dieDelay = new WaitForSeconds(enemyDieDuration);
        canMove = true;
        canSpeak = true;
        isAlive = true;

        searchingClips = new List<AudioClip> {searching1, searching2};
        searching1_length = new WaitForSeconds(searching1.length * 2);
        searching2_length = new WaitForSeconds(searching2.length * 2);
        attackClips = new List<AudioClip> {attack1, attack2};
        attack1_length = new WaitForSeconds(attack1.length * 2);
        attack2_length = new WaitForSeconds(attack2.length * 2);
        dead_length = new WaitForSeconds(dead.length);
        headshot_length = new WaitForSeconds(headshot.length);
    }

    void FixedUpdate()
    {
        if (!PlayerStats.SharedInstance.PlayerIsDead() && !PlayerController.SharedInstance.isGameStopped())
            Move();
    }

    void CalculateDistance()
    {
        playerPosition = PlayerStats.SharedInstance.gameObject.transform.position;
        distance = Vector3.Distance(playerPosition, transform.position);
    }

    void RotateToPlayer()
    {
        relativePosition = playerPosition - transform.position;
        finalRotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, rotationSpeed * Time.deltaTime);
    }

    AudioClip PickRandomClip(List<AudioClip> clips)
    {
        return clips[Random.Range(0, clips.Count)];
    }

    void PlayClip(AudioClip clip)
    {
        canSpeak = false;

        switch (clip.name)
        {
            case "Searching1": soundDelay = searching1_length; volume = searchingVolume; break;
            case "Searching2": soundDelay = searching2_length; volume = searchingVolume; break;
            case "Attack1": soundDelay = attack1_length; volume = attackVolume; break;
            case "Attack2": soundDelay = attack2_length; volume = attackVolume; break;
            case "Headshot": soundDelay = headshot_length; volume = headshotVolume; break;
            case "Dead": soundDelay = dead_length; volume = deadVolume; break;

            default: break;
        }

        audioSource.PlayOneShot(clip, volume);

        coroutine = WaitSound(soundDelay);
        StartCoroutine(coroutine);
    }

    void Move()
    {
        CalculateDistance();

        if (canMove)
        {
            if (canSpeak)
            {
                clip = PickRandomClip(searchingClips);
                PlayClip(clip);
            }

            RotateToPlayer();

            animator.SetBool(IS_RUNNING, true);
            controller.Move(transform.forward * enemySpeed * Time.deltaTime);

            if (distance <= enemyRange)
                Attack();
        }
    }

    void Attack()
    {
        audioSource.Stop();
        clip = PickRandomClip(attackClips);
        PlayClip(clip);

        animator.SetBool(IS_RUNNING, false);
        canMove = false;
        PlayerStats.SharedInstance.TakeDamage(enemyDamage);
        StartCoroutine(WaitAttack());
    }

    public void TakeDamage(int damage, bool isHeadshot)
    {
        if (isAlive)
        {
            health -= damage;
            health = Mathf.Clamp(health, 0, 100);

            if (isHeadshot)
                PlayClip(headshot);

            if (health == 0)
                Die();
        }
    }

    void Die()
    {
        isAlive = false;
        canMove = false;

        PlayClip(dead);
            
        PlayerStats.SharedInstance.IncrementScore();

        animator.SetBool(IS_DEAD, true);
        StartCoroutine(WaitDie());
    }

    IEnumerator WaitAttack()
    {
        yield return attackDelay;

        if (isAlive)
            canMove = true;
    }

    IEnumerator WaitDie()
    {
        yield return dieDelay;

        gameObject.SetActive(false);
        health = maxHealth;
        canMove = true;
        canSpeak = true;
        isAlive = true;
    }

    IEnumerator WaitSound(WaitForSeconds delay)
    {
        yield return delay;

        canSpeak = true;
    }
}
