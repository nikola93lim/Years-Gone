using UnityEngine;

public class RPG : BaseWeapon
{
    [SerializeField] private LayerMask _enemyLayerMask;

    public override void Shoot()
    {
        if (Time.time > _nextShotTime)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _enemyLayerMask))
            {
                foreach (Transform muzzle in _muzzles)
                {
                    _weaponStrategy.Fire(muzzle, _shellEjector, hitInfo.transform, _muzzleVelocity);
                    _muzzleFlash.Activate();
                    _nextShotTime = Time.time + _timeBetweenShots;
                }
            }
        }
    }
}
