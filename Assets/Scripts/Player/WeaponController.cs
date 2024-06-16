using System;
using UnityEngine;

public class WeaponController : BaseWeaponController
{
    private InputReader _inputReader;

    public Transform GunSpawnTransform {  get { return _weaponSpawnTransform; } }

    private void Awake()
    {
        _inputReader = GetComponent<InputReader>();
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

    public void OnTriggerHold()
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.OnTriggerRelease();
        }
    }

}
