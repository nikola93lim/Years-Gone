using UnityEngine;

public abstract class WeaponFactory : ScriptableObject
{
    [SerializeField] protected BaseWeapon _weaponPrefab;
    [SerializeField] protected WeaponStrategy _weaponStrategy;

    public abstract IWeapon CreateWeapon(Transform spawnPoint);
}
