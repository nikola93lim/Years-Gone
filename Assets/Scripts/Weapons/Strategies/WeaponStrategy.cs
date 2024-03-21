using UnityEngine;

public abstract class WeaponStrategy : ScriptableObject
{
    public abstract void Fire(Transform projectileOrigin, Transform shellOrigin, Transform target, float muzzleVelocity);
}
