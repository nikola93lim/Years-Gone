using UnityEngine;

[CreateAssetMenu(fileName = "MissileStrategy", menuName = "Weapon Strategy/Missile")]
public class MissileWeaponStrategy : WeaponStrategy
{
    [SerializeField] private Projectile _missilePrefab;
    [SerializeField] private ParticleSystem _fireBackParticleSystem;

    public override void Fire(Transform projectileOrigin, Transform shellOrigin, Transform target, float muzzleVelocity)
    {
        Projectile missile = Instantiate(_missilePrefab, projectileOrigin.position, projectileOrigin.rotation);
        missile.SetSpeed(muzzleVelocity);
    }
}
