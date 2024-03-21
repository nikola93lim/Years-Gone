using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [SerializeField] protected Transform[] _muzzles;
    [SerializeField] protected Transform _shellEjector;
    [SerializeField] protected WeaponStrategy _weaponStrategy;
    [SerializeField] protected MuzzleFlash _muzzleFlash;
    [SerializeField] protected float _timeBetweenShots;
    [SerializeField] protected float _muzzleVelocity;

    protected bool _triggerReleasedSinceLastShot;
    protected int _shotsRemainingInBurst;
    protected int _burstCount;
    protected float _nextShotTime;

    private void Start()
    {
        _nextShotTime = 0f;
    }

    public abstract void Shoot();

    public void SetWeaponStrategy(WeaponStrategy strategy)
    {
        _weaponStrategy = strategy;
    }

    public void OnTriggerHold()
    {
        Shoot();
        _triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        _triggerReleasedSinceLastShot = true;
    }
}
