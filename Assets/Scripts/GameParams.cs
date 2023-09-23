static class GameParams
{
    // PlayerController
    public enum InvertCamera {NORMAL=-1, INVERTED=1};
    public const float speed = 7f;
    public const float yRotationLimit = 60f;
    public const float maxRecoil = 10f;
    public const float assaultRecoilAmount = 1.5f;
    public const float shotgunRecoilAmount = 10.5f;
    public const float pistolRecoilAmount = 5.1f;
    public const float revertAmount = 0.3f;
    public const float horizontalRecoilAmount = 0.3f;
    public const int speedBuffLifetime = 5; // used by UI script as well

    // PlayerStats
    public const int maxHealth = 100; // used by Enemy script as well

    // Gun
    public const float assaultFireRate = 10f;
    public const float shotgunFireRate = 0.9f;
    public const float pistolFireRate = 10f;
    public enum MagazineSize {ASSAULT = 30, SHOTGUN = 6, PISTOL = 7};
    public enum Range {ASSAULT = 25, SHOTGUN = 7, PISTOL = 15};
    public enum Damage {ASSAULT = 20, SHOTGUN = 100, PISTOL = 17};
    public enum HeadshotDamage {ASSAULT = 34, SHOTGUN = 100, PISTOL = 50};
    public const float assaultReloadDuration = 3f;
    public const float shotgunReloadDuration = 4.3f;
    public const float pistolReloadDuration = 1.7f;
    public const float shootVolume = 0.3f;
    public const float reloadVolume = 0.5f;
    public const int damageBuffLifetime = 5; // used by UI script as well
    public const int buffedDamage = 100;

    // EnemyPool
    public const int ep_amountToPool = 50;
    public const float initialEnemySpawnTimer = 5f;
    public const double spawnTimerEasy = 5;
    public const double spawnTimerMedium = 2.5;
    public const double spawnTimerHard = 1.25;
    public const double initialSpawnTime = 5;

    // Enemy
    public const float enemySpeed = 3f;
    public const float rotationSpeed = 10f;
    public const float enemyRange = 2f;
    public const float enemyAttackDuration = 2.479f;
    public const float enemyDieDuration = 1.15f;
    public const int damageEasy = 20;
    public const int damageMedium = 34;
    public const int damageHard = 50;
    public const float searchingVolume = 0.3f;
    public const float attackVolume = 1f;
    public const float deadVolume = 0.5f;
    public const float headshotVolume = 1f;

    // BulletImpactPool
    public const int bip_amountToPool = 50;
    public const float bulletImpactDelay = 5f;

    // BloodSprayPool
    public const int bsp_amountToPool = 50;
    public const float bloodSprayDelay = 0.5f;

    // Loot
    public const int healthAmount = 20;
    public const int speedMultiplier = 2;
}