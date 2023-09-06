using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameParams;
using static TagHolder;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI SharedInstance;
    Image healthBar;
    TMP_Text scoreCount;
    TMP_Text ammoCount;
    TMP_Text activeGun;
    GameObject reloadWarning;

    void Awake()
    {
        SharedInstance = this;

        healthBar = transform.Find(HEALTH_BAR).gameObject.GetComponent<Image>();
        scoreCount = transform.Find(SCORE_COUNT).gameObject.GetComponent<TMP_Text>();
        ammoCount = transform.Find(AMMO_COUNT).gameObject.GetComponent<TMP_Text>();
        activeGun = transform.Find(ACTIVE_GUN).gameObject.GetComponent<TMP_Text>();
        reloadWarning = transform.Find(RELOAD_WARNING).gameObject;
    }

    public void SetHealthBar(int health)
    {
        healthBar.fillAmount = health / 100;
    }

    public void SetScoreCount(ulong score)
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
    }

    public void ActivateReloadWarning()
    {
        reloadWarning.SetActive(true);
    }

    public void DeactivateReloadWarning()
    {
        reloadWarning.SetActive(false);
    }
}
