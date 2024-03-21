using UnityEngine;

[CreateAssetMenu(fileName = "BulletStrategy", menuName = "Weapon Strategy/Bullet")]
public class BulletWeaponStrategy : WeaponStrategy
{
    [SerializeField] protected Projectile _bulletPrefab;
    [SerializeField] protected Shell _shellPrefab;

    public override void Fire(Transform projectileOrigin, Transform shellOrigin, float muzzleVelocity)
    {
        Projectile bullet = Instantiate(_bulletPrefab, projectileOrigin.position, projectileOrigin.rotation);
        bullet.SetSpeed(muzzleVelocity);

        Instantiate(_shellPrefab, shellOrigin.position, shellOrigin.rotation);
    }
}
