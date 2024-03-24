using UnityEngine;

[CreateAssetMenu(fileName = "BulletStrategy", menuName = "Weapon Strategy/Bullet")]
public class BulletWeaponStrategy : WeaponStrategy
{
    [SerializeField] protected ProjectileSettings _bulletSettings;
    [SerializeField] protected ShellSettings _shellSettings;

    public override void Fire(Transform projectileOrigin, Transform shellOrigin, Transform target,float muzzleVelocity)
    {
        Projectile bullet = FlyweightFactory.Spawn(_bulletSettings) as Projectile;
        bullet.transform.SetPositionAndRotation(projectileOrigin.position, projectileOrigin.rotation);
        bullet.SetSpeed(muzzleVelocity);

        Shell shell = FlyweightFactory.Spawn(_shellSettings) as Shell;
        shell.transform.SetPositionAndRotation(shellOrigin.position, shellOrigin.rotation);
    }
}
