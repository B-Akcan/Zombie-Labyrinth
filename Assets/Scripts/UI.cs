using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameParams;
using static TagHolder;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UI : MonoBehaviour
{
    public static UI SharedInstance;
    Image healthBar;
    TMP_Text scoreCount;
    TMP_Text ammoCount;
    TMP_Text activeGun;
    GameObject reloadWarning;
    GameObject crosshair_aslt;
    GameObject crosshair_shtg;
    GameObject crosshair_pstl;

    void Awake()
    {
        SharedInstance = this;

        healthBar = transform.Find(HEALTH_BAR).gameObject.GetComponent<Image>();
        scoreCount = transform.Find(SCORE_COUNT).gameObject.GetComponent<TMP_Text>();
        ammoCount = transform.Find(AMMO_COUNT).gameObject.GetComponent<TMP_Text>();
        activeGun = transform.Find(ACTIVE_GUN).gameObject.GetComponent<TMP_Text>();
        reloadWarning = transform.Find(RELOAD_WARNING).gameObject;
        crosshair_aslt = transform.Find(CROSSHAIR_ASSAULT).gameObject;
        crosshair_shtg = transform.Find(CROSSHAIR_SHOTGUN).gameObject;
        crosshair_pstl = transform.Find(CROSSHAIR_PISTOL).gameObject;
    }

    public void SetHealthBar(int health)
    {
        healthBar.fillAmount = health / 100f;
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
}
