using System.Collections;
using UnityEngine;

public class Bomb : Flyweight
{
    private Rigidbody rb;
    private TrailRenderer _trailRenderer;

    new BombSettings Settings => (BombSettings)base.Settings;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine(DeactivateAfterLifetimeExpires(Settings.detonationTime));
    }

    private IEnumerator DeactivateAfterLifetimeExpires(float lifetime)
    {
        yield return Utility.GetWaitForSeconds(lifetime);
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 4f, Settings.damageLayerMask);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Health healthComponent))
            {
                Vector3 direction = (healthComponent.transform.position - transform.position).normalized;
                healthComponent.TakeHit(Settings.damage, direction);
            }
        }

        ObjectHitParticle explosionParticle = FlyweightFactory.Spawn(Settings.explosionParticles) as ObjectHitParticle;
        explosionParticle.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

        FlyweightFactory.ReturnToPool(this);
    }

    public void Throw(Vector3 direction, float force)
    {
        Vector3 forwardForce = direction * force;

        // Upward force to create the arc
        Vector3 upwardForce = Vector3.up * Mathf.Sqrt(Settings.arcHeight * -2f * Physics.gravity.y);

        Vector3 combinedForce = forwardForce + upwardForce;

        rb.AddForce(combinedForce, ForceMode.Acceleration);
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
