using UnityEngine;

[CreateAssetMenu(fileName = "MissileStrategy", menuName = "Weapon Strategy/Missile")]
public class MissileWeaponStrategy : WeaponStrategy
{
    [SerializeField] private ProjectileSettings _missileSettings;
    [SerializeField] private ParticleSystem _fireBackParticleSystem;

    public override void Fire(Transform projectileOrigin, Transform shellOrigin, Transform target, float muzzleVelocity)
    {
        Projectile missile = FlyweightFactory.Spawn(_missileSettings) as Projectile;
        missile.transform.SetPositionAndRotation(projectileOrigin.position, projectileOrigin.rotation);
        missile.SetSpeed(muzzleVelocity);
    }
}
