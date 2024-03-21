using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chasing,
        Attacking,
    }

    private State _currentState;

    public event Action<Vector3> OnDeath;

    [SerializeField] private float _attackSpeed = 3f;

    private Health _health;
    private NavMeshAgent _agent;
    private Transform _target;
    private Health _targetHealth;
    private Material _material;

    private Color _originalColour;
    private Color _attackColour = Color.red;

    private float _attackDistanceTreshold = 1.5f;

    private float _nextAttackTime;
    private float _timeBetweenAttacks = 1f;

    private float _myCollisionRadius;
    private float _targetCollisionRadius;

    private int _damage = 1;

    private bool _hasTarget;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
        _myCollisionRadius = GetComponent<CapsuleCollider>().radius;
        _material = GetComponentInChildren<Renderer>().material;
        _originalColour = _material.color;
    }

    private void Start()
    {
        _currentState = State.Idle;
        _health.OnDeath += OnDeath;
        PlayerController pc = FindObjectOfType<PlayerController>();

        if (pc != null) _target = pc.transform;

        if (_target == null) return;

        _targetHealth = _target.GetComponent<Health>();
        _targetHealth.OnDeath += OnTargetDeath;
        _targetCollisionRadius = _target.GetComponent<CapsuleCollider>().radius;

        _hasTarget = true;

        _currentState = State.Chasing;

        StartCoroutine(UpdatePath());

    }

    private void OnTargetDeath(Vector3 hitDirection)
    {
        _hasTarget = false;
        _currentState = State.Idle;
    }

    private void Update()
    {
        if (!_hasTarget) return;

        if (Time.time < _nextAttackTime) return;
        _nextAttackTime = Time.time + _timeBetweenAttacks;

        float distanceToPlayerSquared = (_target.position - transform.position).sqrMagnitude;
        if (distanceToPlayerSquared > Mathf.Pow(_attackDistanceTreshold + _myCollisionRadius + _targetCollisionRadius, 2)) return;

        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        _currentState = State.Attacking;
        _agent.enabled = false;

        _material.color = _attackColour;

        Vector3 originalPosition = transform.position;
        Vector3 directionToTarget = (_target.position - transform.position).normalized;
        Vector3 attackPosition = _target.position - directionToTarget * _myCollisionRadius;

        float percentage = 0f;
        bool hasAppliedDamage = false;

        while (percentage <= 1f)
        {

            if (percentage >= 0.5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                _targetHealth.TakeHit(_damage, transform.forward);
            }

            percentage += Time.deltaTime * _attackSpeed;
            float interpolationValue = (-Mathf.Pow(percentage, 2) + percentage) * 4;
            
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolationValue);

            yield return null;
        }

        _currentState = State.Chasing;
        _agent.enabled = true;

        _material.color = _originalColour;
    }

    private IEnumerator UpdatePath()
    {
        float refreshRate = 0.25f;

        while (_hasTarget)
        {
            if (!_health.IsAlive() || _currentState != State.Chasing)
            {
                yield return new WaitForSeconds(refreshRate);
                continue;
            }

            Vector3 directionToTarget = (_target.position - transform.position).normalized;
            Vector3 targetPos = _target.position - directionToTarget * (_myCollisionRadius + _targetCollisionRadius + _attackDistanceTreshold / 2);

            _agent.SetDestination(targetPos);
            yield return new WaitForSeconds(refreshRate);
        }
    }

    private void OnDestroy()
    {
        _health.OnDeath -= OnDeath;
    }
}
