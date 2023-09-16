static class GameParams
{
    // PlayerController
    public enum InvertCamera {NORMAL=-1, INVERTED=1};
    public const float speed = 7f;
    public const float yRotationLimit = 60f;

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

    // EnemyPool
    public const int ep_amountToPool = 100;
    public const float initialEnemySpawnTimer = 5f;
    public const int scoreMultiplier = 5;
    public const double difficultyEasy = 0.9;
    public const double difficultyMedium = 0.7;
    public const double difficultyHard = 0.5;
    public const double initialSpawnTime = 5f;

    // Enemy
    public const float enemySpeed = 3f;
    public const float rotationSpeed = 10f;
    public const float enemyRange = 2f;
    public const float enemyAttackDuration = 2.479f;
    public const float enemyDieDuration = 1.15f;
    public const int enemyDamage = 20;
    public const float searchingVolume = 0.01f;
    public const float attackVolume = 1f;
    public const float deadVolume = 0.5f;
    public const float headshotVolume = 1f;

    // GameMenu
    public const float initialBrightness = 0.3f;

    // BulletImpactPool
    public const int bip_amountToPool = 50;
    public const float bulletImpactDelay = 5f;
}