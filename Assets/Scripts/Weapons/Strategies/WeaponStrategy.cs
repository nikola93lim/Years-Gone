using UnityEngine;

public abstract class WeaponStrategy : ScriptableObject
{
    public abstract void Shoot(Transform projectileOrigin, Transform shellOrigin, float muzzleVelocity);
}
