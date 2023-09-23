using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameParams;
using static TagHolder;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System;

public class UI : MonoBehaviour
{
    public static UI SharedInstance;
    Image healthBar;
    TMP_Text scoreCount;
    TMP_Text ammoCount;
    TMP_Text activeGun;
    GameObject crosshair_aslt;
    GameObject crosshair_shtg;
    GameObject crosshair_pstl;
    GameObject health;
    GameObject score;
    Vector3 scorePositionInGame;
    Vector3 scorePositionEndGame;
    RectTransform scorePosition;
    GameObject ammo;
    GameObject gun;
    GameObject crosshairs;
    GameObject reloadWarning;
    GameObject endGame;
    GameObject stopGame;
    GameObject damageBuff;
    TMP_Text damageBuffTimeLeft;
    float db_timeLeft;
    WaitForSeconds damageBuffDelay;
    GameObject speedBuff;
    TMP_Text speedBuffTimeLeft;
    float sb_timeLeft;
    WaitForSeconds speedBuffDelay;
    [SerializeField] IntSO screenModeSO;

    void Awake()
    {
        SharedInstance = this;

        health = transform.Find(HEALTH).gameObject;
        score = transform.Find(SCORE).gameObject;
        ammo = transform.Find(AMMO).gameObject;
        gun = transform.Find(GUN).gameObject;
        crosshairs = transform.Find(CROSSHAIRS).gameObject;
        reloadWarning = transform.Find(RELOAD_WARNING).gameObject;
        endGame = transform.Find(END_GAME).gameObject;
        stopGame = transform.Find(STOP_GAME).gameObject;

        healthBar = health.transform.Find(HEALTH_BAR).gameObject.GetComponent<Image>();
        scoreCount = score.transform.Find(SCORE_COUNT).gameObject.GetComponent<TMP_Text>();
        ammoCount = ammo.transform.Find(AMMO_COUNT).gameObject.GetComponent<TMP_Text>();
        activeGun = gun.transform.Find(ACTIVE_GUN).gameObject.GetComponent<TMP_Text>();
        
        crosshair_aslt = crosshairs.transform.Find(CROSSHAIR_ASSAULT).gameObject;
        crosshair_shtg = crosshairs.transform.Find(CROSSHAIR_SHOTGUN).gameObject;
        crosshair_pstl = crosshairs.transform.Find(CROSSHAIR_PISTOL).gameObject;

        scorePosition = score.GetComponent<RectTransform>();

        damageBuff = transform.Find(DAMAGE_BUFF).gameObject;
        damageBuffTimeLeft = damageBuff.transform.Find(DB_TIMELEFT).gameObject.GetComponent<TMP_Text>();
        speedBuff = transform.Find(SPEED_BUFF).gameObject;
        speedBuffTimeLeft = speedBuff.transform.Find(SB_TIMELEFT).gameObject.GetComponent<TMP_Text>();
    }

    void Start()
    {
        scorePositionInGame = new Vector3(-789, 421, 0);
        scorePositionEndGame = new Vector3(35, 20, 0);
        scorePosition.anchoredPosition = scorePositionInGame;

        damageBuffDelay = new WaitForSeconds(damageBuffLifetime);
        speedBuffDelay = new WaitForSeconds(speedBuffLifetime);

        db_timeLeft = damageBuffLifetime;
        sb_timeLeft = speedBuffLifetime;

        ActivateInitialUI();
        AdjustScreenMode();
    }

    void Update()
    {
        BuffCountdown();
    }

    public void SetHealthBar(int health)
    {
        healthBar.fillAmount = health / 100f;
    }

    public void SetScoreCount(uint score)
    {
        scoreCount.text = score.ToString();
    }

    public void SetAmmoCount(int ammo)
    {
        ammoCount.text = ammo.ToString();
    }

    public void SetActiveGun(GameObject gun)
    {
        activeGun.text = gun.name;

        switch (gun.name)
        {
            case "Assault Rifle":
            {
                crosshair_aslt.SetActive(true);
                crosshair_shtg.SetActive(false);
                crosshair_pstl.SetActive(false);
                break;
            }
            case "Shotgun":
            {
                crosshair_aslt.SetActive(false);
                crosshair_shtg.SetActive(true);
                crosshair_pstl.SetActive(false);
                break;
            }
            case "Pistol":
            {
                crosshair_aslt.SetActive(false);
                crosshair_shtg.SetActive(false);
                crosshair_pstl.SetActive(true);
                break;
            }
        }
    }

    public void ActivateReloadWarning()
    {
        reloadWarning.SetActive(true);
    }

    public void DeactivateReloadWarning()
    {
        reloadWarning.SetActive(false);
    }

    void ActivateInitialUI()
    {
        health.SetActive(true);
        score.SetActive(true);
        ammo.SetActive(true);
        gun.SetActive(true);
        crosshairs.SetActive(true);
        reloadWarning.SetActive(false);
        endGame.SetActive(false);
        stopGame.SetActive(false);
        damageBuff.SetActive(false);
        speedBuff.SetActive(false);
    }

    public void ActivateGameStoppedUI()
    {
        crosshairs.SetActive(false);
        stopGame.SetActive(true);
    }

    public void DeactivateGameStoppedUI()
    {
        crosshairs.SetActive(true);
        stopGame.SetActive(false);
    }

    public void ActivateEndGameUI()
    {
        health.SetActive(false);
        score.SetActive(true);
        ammo.SetActive(false);
        gun.SetActive(false);
        crosshairs.SetActive(false);
        reloadWarning.SetActive(false);
        endGame.SetActive(true);
        stopGame.SetActive(false);
        damageBuff.SetActive(false);
        speedBuff.SetActive(false);

        scorePosition.anchoredPosition = scorePositionEndGame;
    }

    public void DamageBuffUI()
    {
        damageBuff.SetActive(true);

        StartCoroutine(WaitDamageBuff());
    }

    public void SpeedBuffUI()
    {
        speedBuff.SetActive(true);

        StartCoroutine(WaitSpeedBuff());
    }

    void BuffCountdown()
    {
        if (damageBuff.activeInHierarchy)
        {  
            db_timeLeft -= Time.deltaTime;
            damageBuffTimeLeft.text = db_timeLeft.ToString("n2") + "s";
        }

        if (speedBuff.activeInHierarchy)
        {
            sb_timeLeft -= Time.deltaTime;
            speedBuffTimeLeft.text = sb_timeLeft.ToString("n2") + "s";
        }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    void AdjustScreenMode()
    {
        switch (screenModeSO.Value)
        {
            case 0: Screen.fullScreenMode = FullScreenMode.FullScreenWindow; break;
            case 1: Screen.fullScreenMode = FullScreenMode.Windowed; break;
        }
    }

    public void Resume()
    {
        PlayerController.SharedInstance.ResumeGame();
    }

    IEnumerator WaitDamageBuff()
    {
        yield return damageBuffDelay;

        damageBuff.SetActive(false);
        db_timeLeft = damageBuffLifetime;
    }

    IEnumerator WaitSpeedBuff()
    {
        yield return speedBuffDelay;

        speedBuff.SetActive(false);
        sb_timeLeft = speedBuffLifetime;
    }
}
