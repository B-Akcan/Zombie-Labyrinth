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
    float fireRate;
    float elapsedTime;
    IEnumerator coroutine;
    WaitForSeconds reloadDelayAssault;
    WaitForSeconds reloadDelayShotgun;
    WaitForSeconds reloadDelayPistol;
    WaitForSeconds reloadDelay;
    bool reloading;
    Animator animator;
    int range;
    int damage;
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

        animator = transform.parent.gameObject.GetComponent<Animator>();
    }
    
    void Start()
    {
        AssignReloadDelays();
        LoadAllWeapons();
        SelectAssault();
    }

    void Update()
    {
        if (!reloading && !PlayerStats.SharedInstance.playerIsDead)
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
            animator.SetBool(IS_SHOOTING, true);

            if (Time.time >= elapsedTime)
            {
                elapsedTime = Time.time + (1f / fireRate);

                DecrementRounds();

                muzzleFlash.Play();
                audioSource.PlayOneShot(fireSound);

                RaycastHit hit;
                if (Physics.Raycast(bulletSpawnPt.transform.position, bulletSpawnPt.transform.TransformDirection(Vector3.forward), out hit, range))
                {
                    hitObject = hit.transform.gameObject;

                    if (hitObject.tag.Equals(ENEMY))
                    {
                        enemy = EnemyPool.SharedInstance.GetEnemy(hitObject);
                        enemy.TakeDamage(damage);
                    }
                    else if (hitObject.tag.Equals(ENVIRONMENT))
                    {
                       
                    }
                }
            }
        }

        else
        {
            animator.SetBool(IS_SHOOTING, false);

            if (currentRounds == 0)
                UI.SharedInstance.ActivateReloadWarning();

            if (Input.GetKeyDown(KeyCode.R))
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
        fireRate = (float) FireRate.ASSAULT;
        reloadDelay = reloadDelayAssault;
        currentRounds = assaultRounds;
        damage = (int) Damage.ASSAULT;

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
        fireRate = (float) FireRate.SHOTGUN;
        reloadDelay = reloadDelayShotgun;
        currentRounds = shotgunRounds;
        damage = (int) Damage.SHOTGUN;

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
        fireRate = (float) FireRate.PISTOL;
        reloadDelay = reloadDelayPistol;
        currentRounds = pistolRounds;
        damage = (int) Damage.PISTOL;

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

        audioSource.PlayOneShot(reloadSound);

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
