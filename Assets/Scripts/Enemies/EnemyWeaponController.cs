using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [SerializeField] private Transform _weaponSpawnTransform;

    [SerializeField] WeaponFactory _weaponFactory;
    private Weapon _weapon;

    private float _nextShotTime;
    private float _timeBetweenShots;

    private void Start()
    {
        EquipWeapon(_weaponFactory);
    }

    private void Update()
    {
        if (Time.time > _nextShotTime)
        {
            _weapon.Shoot();
            _nextShotTime = Time.time + _timeBetweenShots;
        }
    }

    public void EquipWeapon(WeaponFactory weaponFactory)
    {
        _weapon = weaponFactory.CreateWeapon(_weaponSpawnTransform);
        _timeBetweenShots = _weapon.GetTimeBetweenShots();
    }
}
