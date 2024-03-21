using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [SerializeField] private Transform _weaponSpawnTransform;

    [SerializeField] WeaponFactory _weaponFactory;
    private BaseWeapon _weapon;

    private float _fireTimer;

    private void Start()
    {
        EquipWeapon(_weaponFactory);
    }

    private void Update()
    {
        if (Time.time > _fireTimer)
        {
            _weapon.Shoot();
            _fireTimer = Time.time + 3f;
        }
    }

    public void EquipWeapon(WeaponFactory weaponFactory)
    {
        _weapon = weaponFactory.CreateWeapon(_weaponSpawnTransform);
    }
}
