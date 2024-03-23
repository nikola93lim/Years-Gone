using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "HomingMissileStrategy", menuName = "Weapon Strategy/Homing Missile")]
public class HomingMissileWeaponStrategy : WeaponStrategy
{
    [SerializeField] private float _trackingSpeed = 1.0f;
    [SerializeField] private ProjectileSettings _homingMissileSettings;
    [SerializeField] private ParticleSystem _fireBackParticleSystem;

    public override void Fire(Transform projectileOrigin, Transform shellOrigin, Transform target, float muzzleVelocity)
    {
        Projectile missile = FlyweightFactory.Spawn(_homingMissileSettings) as Projectile;
        missile.transform.SetPositionAndRotation(projectileOrigin.position, projectileOrigin.rotation);
        missile.SetSpeed(muzzleVelocity);

        missile.Callback += () =>
        {
            if (target == null) return;
            Vector3 directionToTarget = (target.position - missile.transform.position).normalized;

            Quaternion rotation = Quaternion.LookRotation(directionToTarget);
            missile.transform.rotation = Quaternion.Slerp(missile.transform.rotation, rotation, _trackingSpeed * Time.deltaTime);
        };
    }
}
