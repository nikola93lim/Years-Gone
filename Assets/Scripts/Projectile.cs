using System;
using System.Collections;
using UnityEngine;

public class Projectile : Flyweight 
{
    public event Action Callback;

    private TrailRenderer _trailRenderer;
    new ProjectileSettings Settings => (ProjectileSettings) base.Settings;

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateAfterLifetimeExpires(Settings.Lifetime));

        /*Collider[] initialCollisions = Physics.OverlapSphere(transform.position, 0.1f, Settings.CollisionMask);
        if (initialCollisions.Length > 0 )
        {
            OnHitObject(initialCollisions[0]);
        }*/
    }

    private void Update()
    {
        transform.Translate(Settings.MoveSpeed * Time.deltaTime * Vector3.forward);
        Callback?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHitObject(other);
    }

    private IEnumerator DeactivateAfterLifetimeExpires(float lifetime)
    {
        yield return Utility.GetWaitForSeconds(lifetime);
        FlyweightFactory.ReturnToPool(this);
    }

    private void OnHitObject(Collider collider)
    {
        if (collider.TryGetComponent(out Health health))
        {
            if (collider.TryGetComponent(out Rigidbody rb))
            {
                rb.AddForce(transform.forward, ForceMode.VelocityChange);
            }

            health.TakeHit(Settings.Damage, transform.forward);
        }

        FlyweightFactory.ReturnToPool(this);
    }

    public void SetSpeed(float speed)
    {
        Settings.MoveSpeed = speed;
    }

    public void DeactivateTrailRenderer()
    {
        _trailRenderer.Clear();
        _trailRenderer.enabled = false;
    }

    public void ReactivateTrailRenderer()
    {
        _trailRenderer.enabled = true;
    }
}
