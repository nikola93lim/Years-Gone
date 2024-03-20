using UnityEngine;

[CreateAssetMenu(fileName = "PistolFactory", menuName = "Weapon Factory/Pistol")]
public class PistolFactory : WeaponFactory
{
    public override IWeapon CreateWeapon(Transform spawnPoint)
    {
        Pistol pistol = Instantiate(_weaponPrefab, spawnPoint.position, spawnPoint.rotation) as Pistol;
        pistol.transform.parent = spawnPoint;
        pistol.SetWeaponStrategy(_weaponStrategy);
        return pistol;
    }
}
