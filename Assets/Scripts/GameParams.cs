using UnityEditor;

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
    public const float reloadDuration = 3f;
}