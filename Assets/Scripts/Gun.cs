using UnityEngine;

public class Gun : MonoBehaviour 
{
    public enum FireMode { Auto, Burst, Single}

    [SerializeField] private FireMode _fireMode;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Projectile _bullet;
    [SerializeField] private Transform _shellEjector;
    [SerializeField] private Shell _shellPrefab;

    [SerializeField] private float _msBetweenShots = 100f;
    [SerializeField] private float _muzzleVelocity = 35f;
    [SerializeField] private int _burstCount;

    private float _nextShotTime;

    private MuzzleFlash _muzzleFlash;

    private bool _triggerReleasedSinceLastShot;
    private int _shotsRemainingInBurst;

    private void Awake()
    {
        _muzzleFlash = GetComponent<MuzzleFlash>();
        _shotsRemainingInBurst = _burstCount;
    }

    private void Shoot()
    {
        if (Time.time > _nextShotTime)
        {
            if (_fireMode == FireMode.Burst)
            {
                if (_shotsRemainingInBurst == 0) return;
                _shotsRemainingInBurst--;
            }
            else if (_fireMode == FireMode.Single)
            {
                if (!_triggerReleasedSinceLastShot) return;
            }

            _nextShotTime = Time.time + _msBetweenShots / 1000f;
            Projectile bullet = Instantiate(_bullet, _muzzle.position, _muzzle.rotation);
            bullet.SetSpeed(_muzzleVelocity);

            Instantiate(_shellPrefab, _shellEjector.position, _shellEjector.rotation);
            _muzzleFlash.Activate();
        }
    }

    public void OnTriggerHold()
    {
        Shoot();
        _triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        _triggerReleasedSinceLastShot = true;
        _shotsRemainingInBurst = _burstCount;
    }
}
