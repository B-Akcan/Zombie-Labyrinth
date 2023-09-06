using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameParams;
using static TagHolder;

public class Gun : MonoBehaviour
{
    GameObject assault;
    GameObject shotgun;
    GameObject pistol;
    GameObject selectedGun;
    GameObject bulletSpawnPt;
    float elapsedTime;
    ParticleSystem muzzleFlash;
    AudioSource assaultSound;
    AudioSource shotgunSound;
    AudioSource pistolSound;
    int assaultRounds;
    int shotgunRounds;
    int pistolRounds;
    AudioSource reloadSound;
    IEnumerator coroutine;
    WaitForSeconds reloadDelay = new WaitForSeconds(reloadDuration);
    bool reloading = false;

    void Awake()
    {
        assault = transform.Find(ASSAULT).gameObject;
        shotgun = transform.Find(SHOTGUN).gameObject;
        pistol = transform.Find(PISTOL).gameObject;
        assaultSound = assault.GetComponent<AudioSource>();
        shotgunSound = shotgun.GetComponent<AudioSource>();
        pistolSound = pistol.GetComponent<AudioSource>();
        reloadSound = GetComponent<AudioSource>();
    }
    
    void Start()
    {
        LoadAllWeapons();
        SelectAssault();
    }

    void Update()
    {
        if (!reloading)
        {
            GunSelect();
            Fire();
        }  
    }

    void GunSelect()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectAssault();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectShotgun();
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectPistol();
        }
    }

    void Fire()
    {
        if (selectedGun == assault)
        {
            if (assaultRounds == 0)
                UI.SharedInstance.ActivateReloadWarning();

            if (assaultRounds > 0 && Input.GetKey(KeyCode.Mouse0) && Time.time >= elapsedTime)
            {
                elapsedTime = Time.time + (1f / (float) FireRate.ASSAULT);

                assaultRounds--;
                UI.SharedInstance.SetAmmoCount(assaultRounds);

                muzzleFlash.Play();
                assaultSound.Play();

                RaycastHit hit;
                if (Physics.Raycast(bulletSpawnPt.transform.position, Vector3.forward, out hit, Mathf.Infinity))
                {

                }
            }

            else if (Input.GetKeyDown(KeyCode.R) && assaultRounds < (int) MagazineSize.ASSAULT)
                Reload(assault); 
        }
        
        else if (selectedGun == shotgun)
        {
            if (shotgunRounds == 0)
                UI.SharedInstance.ActivateReloadWarning();

            if (shotgunRounds > 0 && Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= elapsedTime)
            {
                elapsedTime = Time.time + (1f / (float) FireRate.SHOTGUN);

                shotgunRounds--;
                UI.SharedInstance.SetAmmoCount(shotgunRounds);

                muzzleFlash.Play();
                shotgunSound.Play();

                RaycastHit hit;
                if (Physics.Raycast(bulletSpawnPt.transform.position, Vector3.forward, out hit, Mathf.Infinity))
                {

                }
            }

            else if (Input.GetKeyDown(KeyCode.R) && shotgunRounds < (int) MagazineSize.SHOTGUN)
                Reload(shotgun);
        }

        else
        {
            if (pistolRounds == 0)
                UI.SharedInstance.ActivateReloadWarning();

            if (pistolRounds > 0 && Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= elapsedTime)
            {
                elapsedTime = Time.time + (1f / (float) FireRate.PISTOL);

                pistolRounds--;
                UI.SharedInstance.SetAmmoCount(pistolRounds);

                muzzleFlash.Play();
                pistolSound.Play();

                RaycastHit hit;
                if (Physics.Raycast(bulletSpawnPt.transform.position, Vector3.forward, out hit, Mathf.Infinity))
                {

                }
            }
        
            else if (Input.GetKeyDown(KeyCode.R) && pistolRounds < (int) MagazineSize.PISTOL)
                Reload(pistol);
        }

    }

    void SelectAssault()
    {
        selectedGun = assault;
        bulletSpawnPt = selectedGun.transform.Find(BULLET_SPAWN_POINT).gameObject;
        muzzleFlash = bulletSpawnPt.transform.GetChild(0).GetComponent<ParticleSystem>();

        UI.SharedInstance.SetAmmoCount(assaultRounds);
        UI.SharedInstance.SetActiveGun(assault);

        assault.SetActive(true);
        shotgun.SetActive(false);
        pistol.SetActive(false);

        elapsedTime = 0f;
    }

    void SelectShotgun()
    {
        selectedGun = shotgun;
        bulletSpawnPt = selectedGun.transform.Find(BULLET_SPAWN_POINT).gameObject;
        muzzleFlash = bulletSpawnPt.transform.GetChild(0).GetComponent<ParticleSystem>();

        UI.SharedInstance.SetAmmoCount(shotgunRounds);
        UI.SharedInstance.SetActiveGun(shotgun);

        assault.SetActive(false);
        shotgun.SetActive(true);
        pistol.SetActive(false);

        elapsedTime = 0f;
    }

    void SelectPistol()
    {
        selectedGun = pistol;
        bulletSpawnPt = selectedGun.transform.Find(BULLET_SPAWN_POINT).gameObject;
        muzzleFlash = bulletSpawnPt.transform.GetChild(0).GetComponent<ParticleSystem>();

        UI.SharedInstance.SetAmmoCount(pistolRounds);
        UI.SharedInstance.SetActiveGun(pistol);

        assault.SetActive(false);
        shotgun.SetActive(false);
        pistol.SetActive(true);

        elapsedTime = 0f;
    }

    void Reload(GameObject gun)
    {
        reloading = true;

        reloadSound.Play();

        UI.SharedInstance.DeactivateReloadWarning();

        coroutine = WaitReload(gun);
        StartCoroutine(coroutine);
    }

    void LoadAllWeapons()
    {
        assaultRounds = (int) MagazineSize.ASSAULT;
        shotgunRounds = (int) MagazineSize.SHOTGUN;
        pistolRounds = (int) MagazineSize.PISTOL;
    }

    IEnumerator WaitReload(GameObject gun)
    {
        yield return reloadDelay;

        if (gun == assault)
        {
            assaultRounds = (int) MagazineSize.ASSAULT;
            UI.SharedInstance.SetAmmoCount(assaultRounds);
        }
        else if (gun == shotgun)
        {
            shotgunRounds = (int) MagazineSize.SHOTGUN;
            UI.SharedInstance.SetAmmoCount(shotgunRounds);
        }
        else if (gun == pistol)
        {
            pistolRounds = (int) MagazineSize.PISTOL;
            UI.SharedInstance.SetAmmoCount(pistolRounds);
        }

        reloading = false;
    }
}
