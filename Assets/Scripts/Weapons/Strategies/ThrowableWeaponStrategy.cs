using UnityEngine;

[CreateAssetMenu(fileName = "ThrowableStrategy", menuName = "Weapon Strategy/Throwable")]
public class ThrowableWeaponStrategy : WeaponStrategy
{
    public override Projectile Fire(Transform projectileOrigin, Transform shellOrigin, Transform target, float muzzleVelocity)
    {
        Projectile projectile = base.Fire(projectileOrigin, shellOrigin, target, muzzleVelocity);

        return projectile;
    }
}
