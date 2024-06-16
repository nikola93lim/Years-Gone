using UnityEngine;

[CreateAssetMenu(fileName = "BulletStrategy", menuName = "Weapon Strategy/Bullet")]
public class BulletWeaponStrategy : WeaponStrategy
{
    [SerializeField] protected ShellSettings _shellSettings;

    public override Projectile Fire(Transform projectileOrigin, Transform shellOrigin, Transform target, float muzzleVelocity)
    {
        Projectile projectile = base.Fire(projectileOrigin, shellOrigin, target, muzzleVelocity);

        Shell shell = FlyweightFactory.Spawn(_shellSettings) as Shell;
        shell.transform.SetPositionAndRotation(shellOrigin.position, shellOrigin.rotation);

        return projectile;
    }
}
