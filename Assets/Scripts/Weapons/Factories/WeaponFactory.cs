using UnityEngine;

[CreateAssetMenu(fileName = "WeaponFactory", menuName = "Weapon Factory/Weapon")]
public class WeaponFactory : ScriptableObject
{
    [SerializeField] private Weapon _weaponPrefab;
    [SerializeField] private WeaponStrategy _weaponStrategy;

    public Weapon CreateWeapon(Transform spawnPoint)
    {
        Weapon newWeapon = Instantiate(_weaponPrefab, spawnPoint.position, spawnPoint.rotation);
        newWeapon.transform.parent = spawnPoint;
        newWeapon.SetWeaponStrategy(_weaponStrategy);
        return newWeapon;
    }
}
