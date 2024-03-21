using System;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform _weaponSpawnTransform;

    [SerializeField] WeaponFactory _weaponFactory;
    private BaseWeapon _weapon;

    private InputReader _inputReader;

    public Transform GunSpawnTransform {  get { return _weaponSpawnTransform; } }

    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
    }

    private void Start()
    {
        EquipWeapon(_weaponFactory);
    }

    private void Update()
    {
        if (_inputReader.Fire)
        {
            OnTriggerHold();
        }
        else
        {
            OnTriggerRelease();
        }
    }

    public void EquipWeapon(WeaponFactory weaponFactory)
    {
        _weapon = weaponFactory.CreateWeapon(_weaponSpawnTransform);
    }

    public void OnTriggerHold()
    {
        if (_weapon != null)
        {
            _weapon.OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (_weapon != null)
        {
            _weapon.OnTriggerRelease();
        }
    }

}
