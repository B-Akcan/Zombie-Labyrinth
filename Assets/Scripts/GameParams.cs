static class GameParams
{
    // PlayerController
    public enum InvertCamera {NORMAL=-1, INVERTED=1};
    public const float speed = 5f;
    public const float yRotationLimit = 60f;

    // PlayerStats
    public const int maxHealth = 100;

    // Gun
    public enum FireRate {ASSAULT = 10, SHOTGUN = 1, PISTOL = 10};
    public enum MagazineSize {ASSAULT = 30, SHOTGUN = 6, PISTOL = 7};
    public enum Range {ASSAULT = 25, SHOTGUN = 5, PISTOL = 15};
    public enum Damage {ASSAULT = 25, SHOTGUN = 100, PISTOL = 17};
    public const float assaultReloadDuration = 2.978f;
    public const float shotgunReloadDuration = 4.152f;
    public const float pistolReloadDuration = 1f;
}