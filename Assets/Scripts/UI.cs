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
    }

    void Start()
    {
        scorePositionInGame = new Vector3(-789, 421, 0);
        scorePositionEndGame = new Vector3(35, 20, 0);
        scorePosition.anchoredPosition = scorePositionInGame;

        ActivateInitialUI();
        AdjustScreenMode();
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

        scorePosition.anchoredPosition = scorePositionEndGame;
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
}
