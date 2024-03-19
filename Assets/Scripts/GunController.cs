using System;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Gun _startingGun;
    [SerializeField] private Transform _gunSpawnTransform;
    private Gun _equippedGun;

    public Transform GunSpawnTransform {  get { return _gunSpawnTransform; } }

    private void Awake()
    {
        if (_startingGun != null )
        {
            EquipGun(_startingGun);
        }
    }

    public void EquipGun(Gun gun)
    {
        if (_equippedGun != null)
        {
            Destroy(_equippedGun.gameObject);
        }

        _equippedGun = Instantiate(gun, _gunSpawnTransform.position, _gunSpawnTransform.rotation);
        _equippedGun.transform.parent = _gunSpawnTransform;
    }

    public void OnTriggerHold()
    {
        if (_equippedGun != null)
        {
            _equippedGun.OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (_equippedGun != null)
        {
            _equippedGun.OnTriggerRelease();
        }
    }

}
