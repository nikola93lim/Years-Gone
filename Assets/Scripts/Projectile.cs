using System;
using UnityEngine;

public class Projectile : MonoBehaviour 
{
    public event Action Callback;

    [SerializeField] private LayerMask _collisionMask;
    [SerializeField] private ParticleSystem _objectHitParticleSystem;
    [SerializeField] private float _moveSpeed = 30f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _lifetime = 3f;

    private void Start()
    {
        Destroy(gameObject, _lifetime);

        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, 0.1f, _collisionMask);
        if (initialCollisions.Length > 0 )
        {
            OnHitObject(initialCollisions[0]);
        }
    }

    private void Update()
    {
        Callback?.Invoke();

        float moveDistance = _moveSpeed * Time.deltaTime;
        //CheckForCollision(moveDistance);
        transform.Translate(moveDistance * Vector3.forward);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnHitObject(other);
    }

    private void CheckForCollision(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, moveDistance, _collisionMask, QueryTriggerInteraction.Collide)) 
        {
            OnHitObject(hitInfo.collider);
        }
    }

    private void OnHitObject(Collider collider)
    {
        if (collider.TryGetComponent(out Health health))
        {
            health.TakeHit(_damage, transform.forward);
        }
        else
        {
            ParticleSystem obstacleHitParticle = Instantiate(_objectHitParticleSystem, transform.position, Quaternion.identity);
            Destroy(obstacleHitParticle.gameObject, obstacleHitParticle.main.startLifetime.constant);
        }

        Destroy(gameObject);
    }

    public void SetSpeed(float speed)
    {
        _moveSpeed = speed;
    }
}
