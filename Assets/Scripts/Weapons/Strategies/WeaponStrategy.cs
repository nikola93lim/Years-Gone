using UnityEngine;

public abstract class WeaponStrategy : ScriptableObject
{
    public abstract bool TryFire(Transform projectileOrigin, Transform shellOrigin, float muzzleVelocity);
}
