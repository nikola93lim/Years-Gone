using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected SoundManager.Sound _fireSound;
    [SerializeField] protected Transform[] _muzzles;
    [SerializeField] protected Transform _shellEjector;
    [SerializeField] protected WeaponStrategy _weaponStrategy;
    [SerializeField] protected MuzzleFlash _muzzleFlash;
    [SerializeField] protected float _timeBetweenShots;
    [SerializeField] protected float _muzzleVelocity;

    protected bool _triggerReleasedSinceLastShot;
    protected float _nextShotTime;

    protected virtual void Start()
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

    public float GetTimeBetweenShots()
    {
        return _timeBetweenShots;
    }
}
