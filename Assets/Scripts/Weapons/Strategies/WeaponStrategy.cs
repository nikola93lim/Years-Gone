using UnityEngine;

public abstract class WeaponStrategy : ScriptableObject
{
    [SerializeField] protected ProjectileSettings _projectileSettings;
    [SerializeField] protected SoundObjectFXSettings _soundObjectFXSettings;
    public virtual Projectile Fire(Transform projectileOrigin, Transform shellOrigin, Transform target, float muzzleVelocity)
    {
        Projectile projectile = FlyweightFactory.Spawn(_projectileSettings) as Projectile;
        projectile.transform.SetPositionAndRotation(projectileOrigin.position, projectileOrigin.rotation);
        projectile.SetSpeed(muzzleVelocity);

        SoundObjectFX soundObject = FlyweightFactory.Spawn(_soundObjectFXSettings) as SoundObjectFX;
        soundObject.transform.position = projectileOrigin.position;
        soundObject.PlaySound();

        return projectile;
    }
}
