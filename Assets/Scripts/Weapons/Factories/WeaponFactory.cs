using UnityEngine;

[CreateAssetMenu(fileName = "WeaponFactory", menuName = "Weapon Factory/Weapon")]
public class WeaponFactory : ScriptableObject
{
    [SerializeField] private BaseWeapon _weaponPrefab;
    [SerializeField] private WeaponStrategy _weaponStrategy;

    public BaseWeapon CreateWeapon(Transform spawnPoint)
    {
        BaseWeapon newWeapon = Instantiate(_weaponPrefab, spawnPoint.position, spawnPoint.rotation);
        newWeapon.transform.parent = spawnPoint;
        newWeapon.SetWeaponStrategy(_weaponStrategy);
        return newWeapon;
    }
}
