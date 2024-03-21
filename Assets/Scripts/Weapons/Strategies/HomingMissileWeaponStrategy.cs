using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "HomingMissileStrategy", menuName = "Weapon Strategy/Homing Missile")]
public class HomingMissileWeaponStrategy : WeaponStrategy
{
    [SerializeField] private float _trackingSpeed = 1.0f;
    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private Projectile _missilePrefab;
    [SerializeField] private ParticleSystem _fireBackParticleSystem;

    public override bool TryFire(Transform projectileOrigin, Transform shellOrigin, float muzzleVelocity)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _enemyLayerMask))
        {
            Transform target = hitInfo.transform;

            Projectile missile = Instantiate(_missilePrefab, projectileOrigin.position, projectileOrigin.rotation);
            missile.SetSpeed(muzzleVelocity);

            missile.Callback += () =>
            {

                if (target == null) return;
                Vector3 directionToTarget = (target.position - missile.transform.position).normalized;

                Quaternion rotation = Quaternion.LookRotation(directionToTarget);
                missile.transform.rotation = Quaternion.Slerp(missile.transform.rotation, rotation, _trackingSpeed * Time.deltaTime);
            };

            return true;
        }

        return false;
    }
}
