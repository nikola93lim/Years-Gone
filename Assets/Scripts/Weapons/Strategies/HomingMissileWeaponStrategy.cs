using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "HomingMissileStrategy", menuName = "Weapon Strategy/Homing Missile")]
public class HomingMissileWeaponStrategy : WeaponStrategy
{
    [SerializeField] private float _trackingSpeed = 1.0f;
    [SerializeField] private ParticleSystem _fireBackParticleSystem;

    public override Projectile Fire(Transform projectileOrigin, Transform shellOrigin, Transform target, float muzzleVelocity)
    {
        Projectile projectile = base.Fire(projectileOrigin, shellOrigin, target, muzzleVelocity);

        projectile.Callback += () =>
        {
            if (target == null) return;
            Vector3 directionToTarget = (target.position - projectile.transform.position).normalized;

            Quaternion rotation = Quaternion.LookRotation(directionToTarget);
            projectile.transform.rotation = Quaternion.Slerp(projectile.transform.rotation, rotation, _trackingSpeed * Time.deltaTime);
        };

        return projectile;
    }
}
