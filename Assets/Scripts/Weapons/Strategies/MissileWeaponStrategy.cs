using UnityEngine;

[CreateAssetMenu(fileName = "MissileStrategy", menuName = "Weapon Strategy/Missile")]
public class MissileWeaponStrategy : WeaponStrategy
{
    [SerializeField] private ParticleSystem _fireBackParticleSystem;

    public override Projectile Fire(Transform projectileOrigin, Transform shellOrigin, Transform target, float muzzleVelocity)
    {
        Projectile projectile = base.Fire(projectileOrigin, shellOrigin, target, muzzleVelocity);
        // implement particles

        return projectile;
    }
}
