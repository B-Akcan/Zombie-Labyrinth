using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static GameParams;
using static TagHolder;

public class Gun : MonoBehaviour
{
    GameObject assault;
    GameObject shotgun;
    GameObject pistol;
    GameObject selectedGun;
    GameObject bulletSpawnPt_Assault;
    GameObject bulletSpawnPt_Shotgun;
    GameObject bulletSpawnPt_Pistol;
    GameObject bulletSpawnPt;
    ParticleSystem muzzleFlash_Assault;
    ParticleSystem muzzleFlash_Shotgun;
    ParticleSystem muzzleFlash_Pistol;
    ParticleSystem muzzleFlash;
    AudioSource audioSource;
    [SerializeField] AudioClip assaultReloadSound;
    [SerializeField] AudioClip shotgunReloadSound;
    [SerializeField] AudioClip pistolReloadSound;
    AudioClip reloadSound;
    [SerializeField] AudioClip assaultFireSound;
    [SerializeField] AudioClip shotgunFireSound;
    [SerializeField] AudioClip pistolFireSound;
    AudioClip fireSound;
    int assaultRounds;
    int shotgunRounds;
    int pistolRounds;
    int currentRounds;
    int magazineSize;
    float fireRate;
    float elapsedTime;
    IEnumerator coroutine;
    WaitForSeconds reloadDelayAssault;
    WaitForSeconds reloadDelayShotgun;
    WaitForSeconds reloadDelayPistol;
    WaitForSeconds reloadDelay;
    bool reloading;
    int range;
    int damage;
    int headshotDamage;
    bool isHeadshot;
    Enemy enemy;
    GameObject hitObject;

    void Awake()
    {
        assault = transform.Find(ASSAULT).gameObject;
        shotgun = transform.Find(SHOTGUN).gameObject;
        pistol = transform.Find(PISTOL).gameObject;

        audioSource = GetComponent<AudioSource>();

        bulletSpawnPt_Assault = assault.transform.Find(BULLET_SPAWN_POINT).gameObject;
        bulletSpawnPt_Shotgun = shotgun.transform.Find(BULLET_SPAWN_POINT).gameObject;
        bulletSpawnPt_Pistol = pistol.transform.Find(BULLET_SPAWN_POINT).gameObject;

        muzzleFlash_Assault = bulletSpawnPt_Assault.transform.GetChild(0).GetComponent<ParticleSystem>();
        muzzleFlash_Shotgun = bulletSpawnPt_Shotgun.transform.GetChild(0).GetComponent<ParticleSystem>();
        muzzleFlash_Pistol = bulletSpawnPt_Pistol.transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    
    void Start()
    {
        AssignReloadDelays();
        LoadAllWeapons();
        SelectAssault();
    }

    void Update()
    {
        if (!reloading && !PlayerStats.SharedInstance.PlayerIsDead() && !PlayerController.SharedInstance.isGameStopped())
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
        if (currentRounds > 0 && ((selectedGun == assault) ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0)))
        {
            if (Time.time >= elapsedTime)
            {
                elapsedTime = Time.time + (1f / fireRate);

                DecrementRounds();

                muzzleFlash.Play();
                audioSource.PlayOneShot(fireSound, shootVolume);

                RaycastHit hit;
                if (Physics.Raycast(bulletSpawnPt.transform.position, bulletSpawnPt.transform.TransformDirection(Vector3.forward), out hit, range))
                {
                    hitObject = hit.transform.gameObject;

                    if (hitObject.tag.Equals(ENEMY))
                    {
                        enemy = EnemyPool.SharedInstance.GetEnemy(hitObject);

                        if (hit.point.y >= 1.5)
                        {
                            isHeadshot = true;
                            enemy.TakeDamage(headshotDamage, isHeadshot);
                        }
                        else
                        {
                            isHeadshot = false;
                            enemy.TakeDamage(damage, isHeadshot);
                        }

                        GameObject bloodSprayEffect = BloodSprayPool.SharedInstance.GetEffect();

                        if (bloodSprayEffect != null)
                            BloodSprayPool.SharedInstance.InstantiateEffect(bloodSprayEffect, hit);
                    }

                    else if (hitObject.tag.Equals(ENVIRONMENT) || hitObject.tag.Equals(BOTTOM))
                    {
                        GameObject bulletImpactEffect = BulletImpactPool.SharedInstance.GetEffect();

                        if (bulletImpactEffect != null)
                            BulletImpactPool.SharedInstance.InstantiateEffect(bulletImpactEffect, hit);
                    }
                }
            }
        }

        else
        {
            if (currentRounds == 0)
                UI.SharedInstance.ActivateReloadWarning();

            if (currentRounds < magazineSize && Input.GetKeyDown(KeyCode.R))
                Reload(selectedGun, reloadDelay);
        }
    }

    void AssignReloadDelays()
    {
        reloading = false;
        reloadDelayAssault = new WaitForSeconds(assaultReloadDuration);
        reloadDelayShotgun = new WaitForSeconds(shotgunReloadDuration);
        reloadDelayPistol = new WaitForSeconds(pistolReloadDuration);
    }

    void SelectAssault()
    {
        selectedGun = assault;
        bulletSpawnPt = bulletSpawnPt_Assault;
        muzzleFlash = muzzleFlash_Assault;
        range = (int) Range.ASSAULT;
        fireSound = assaultFireSound;
        reloadSound = assaultReloadSound;
        fireRate = assaultFireRate;
        reloadDelay = reloadDelayAssault;
        currentRounds = assaultRounds;
        damage = (int) Damage.ASSAULT;
        magazineSize = (int) MagazineSize.ASSAULT;
        headshotDamage = (int) HeadshotDamage.ASSAULT;

        UI.SharedInstance.SetAmmoCount(assaultRounds);
        UI.SharedInstance.SetActiveGun(assault);

        if (currentRounds > 0)
            UI.SharedInstance.DeactivateReloadWarning();

        assault.SetActive(true);
        shotgun.SetActive(false);
        pistol.SetActive(false);

        elapsedTime = 0f;
    }

    void SelectShotgun()
    {
        selectedGun = shotgun;
        bulletSpawnPt = bulletSpawnPt_Shotgun;
        muzzleFlash = muzzleFlash_Shotgun;
        range = (int) Range.SHOTGUN;
        fireSound = shotgunFireSound;
        reloadSound = shotgunReloadSound;
        fireRate = shotgunFireRate;
        reloadDelay = reloadDelayShotgun;
        currentRounds = shotgunRounds;
        damage = (int) Damage.SHOTGUN;
        magazineSize = (int) MagazineSize.SHOTGUN;
        headshotDamage = (int) HeadshotDamage.SHOTGUN;

        UI.SharedInstance.SetAmmoCount(shotgunRounds);
        UI.SharedInstance.SetActiveGun(shotgun);

        if (currentRounds > 0)
            UI.SharedInstance.DeactivateReloadWarning();

        assault.SetActive(false);
        shotgun.SetActive(true);
        pistol.SetActive(false);

        elapsedTime = 0f;
    }

    void SelectPistol()
    {
        selectedGun = pistol;
        bulletSpawnPt = bulletSpawnPt_Pistol;
        muzzleFlash = muzzleFlash_Pistol;
        range = (int) Range.PISTOL;
        fireSound = pistolFireSound;
        reloadSound = pistolReloadSound;
        fireRate = pistolFireRate;
        reloadDelay = reloadDelayPistol;
        currentRounds = pistolRounds;
        damage = (int) Damage.PISTOL;
        magazineSize = (int) MagazineSize.PISTOL;
        headshotDamage = (int) HeadshotDamage.PISTOL;

        UI.SharedInstance.SetAmmoCount(pistolRounds);
        UI.SharedInstance.SetActiveGun(pistol);

        if (currentRounds > 0)
            UI.SharedInstance.DeactivateReloadWarning();

        assault.SetActive(false);
        shotgun.SetActive(false);
        pistol.SetActive(true);

        elapsedTime = 0f;
    }

    void DecrementRounds()
    {
        currentRounds--;

        if (selectedGun == assault)
        {
            assaultRounds--;
            UI.SharedInstance.SetAmmoCount(assaultRounds);
        }

        else if (selectedGun == shotgun)
        {
            shotgunRounds--;
            UI.SharedInstance.SetAmmoCount(shotgunRounds);
        }

        else if (selectedGun == pistol)
        {
            pistolRounds--;
            UI.SharedInstance.SetAmmoCount(pistolRounds);
        }
        
    }

    void Reload(GameObject gun, WaitForSeconds reloadDelay)
    {
        reloading = true;

        audioSource.PlayOneShot(reloadSound, reloadVolume);

        UI.SharedInstance.DeactivateReloadWarning();

        coroutine = WaitReload(gun, reloadDelay);
        StartCoroutine(coroutine);
    }

    void LoadAllWeapons()
    {
        assaultRounds = (int) MagazineSize.ASSAULT;
        shotgunRounds = (int) MagazineSize.SHOTGUN;
        pistolRounds = (int) MagazineSize.PISTOL;
    }

    IEnumerator WaitReload(GameObject gun, WaitForSeconds reloadDelay)
    {
        yield return reloadDelay;

        audioSource.Stop();

        if (gun == assault)
        {
            assaultRounds = (int) MagazineSize.ASSAULT;
            UI.SharedInstance.SetAmmoCount(assaultRounds);
            currentRounds = assaultRounds;
        }
        else if (gun == shotgun)
        {
            shotgunRounds = (int) MagazineSize.SHOTGUN;
            UI.SharedInstance.SetAmmoCount(shotgunRounds);
            currentRounds = shotgunRounds;
        }
        else if (gun == pistol)
        {
            pistolRounds = (int) MagazineSize.PISTOL;
            UI.SharedInstance.SetAmmoCount(pistolRounds);
            currentRounds = pistolRounds;
        }

        reloading = false;
    }
}
