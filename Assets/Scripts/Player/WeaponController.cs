using System;
using System.Collections;
using UnityEngine;

public class WeaponController : BaseWeaponController
{
    private InputReader _inputReader;
    [SerializeField] private BombSettings _bombSettings;

    private bool _isThrowing;
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

        if (_inputReader.FireSecondary)
        {
            TryFireSecondary();
        }
    }

    private bool TryFireSecondary()
    {
        if (_isThrowing) return false;

        StartCoroutine(Throw());
        return true;
    }

    public void OnTriggerHold()
    {
        if (_currentPrimaryWeapon != null)
        {
            _currentPrimaryWeapon.OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (_currentPrimaryWeapon != null)
        {
            _currentPrimaryWeapon.OnTriggerRelease();
        }
    }

    private IEnumerator Throw()
    {
        _isThrowing = true;

        float throwForce = 0f;

        while (!_inputReader.FireSecondaryReleased)
        {
            throwForce += 5f;
            yield return null;
        }

        throwForce = Mathf.Clamp(throwForce, 100f, 500f);

        Bomb bomb = FlyweightFactory.Spawn(_bombSettings) as Bomb;
        bomb.transform.SetPositionAndRotation(GunSpawnTransform.position, Quaternion.identity);
        bomb.Throw(transform.forward, throwForce);
        yield return Utility.GetWaitForSeconds(2f);
        _isThrowing = false;
    }
}
