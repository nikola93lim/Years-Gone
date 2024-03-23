using UnityEngine;

[CreateAssetMenu(fileName = "BulletStrategy", menuName = "Weapon Strategy/Bullet")]
public class BulletWeaponStrategy : WeaponStrategy
{
    [SerializeField] protected ProjectileSettings _bulletSettings;
    [SerializeField] protected Shell _shellPrefab;

    public override void Fire(Transform projectileOrigin, Transform shellOrigin, Transform target,float muzzleVelocity)
    {
        Projectile bullet = FlyweightFactory.Spawn(_bulletSettings) as Projectile;
        bullet.transform.SetPositionAndRotation(projectileOrigin.position, projectileOrigin.rotation);
        bullet.SetSpeed(muzzleVelocity);

        Instantiate(_shellPrefab, shellOrigin.position, shellOrigin.rotation);
    }
}
