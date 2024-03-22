using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierAI : MonoBehaviour
{
    [SerializeField] private float _patrolRadius = 5f;
    [SerializeField] private float _chaseDistance = 5f;
    [SerializeField] private float _alertNearbyRadius = 5f;
    [SerializeField] private float _suspicionStateTime = 3f;
    [SerializeField] private float _waypointDistanceTollerance = 0.2f;
    [SerializeField] private float _waypointDwellTime = 3f;
    [SerializeField] private float aggroCooldownTime = 5f;
    [SerializeField] private float _rotationSpeed = 30f;
    [SerializeField] private int _numOfPatrolPoints = 4;
    [SerializeField] private List<Vector3> _patrolPath;

    [SerializeField] private LayerMask _enemyLayerMask;
    [SerializeField] private LayerMask _obstacleLayerMask;

    private NavMeshAgent _agent;
    private EnemyWeaponController _weaponController;
    private GameObject _player;
    private Health _health;
    private Vector3 _guardPosition;
    private Vector3 _lastKnownPlayerPosition;

    private float _timeSinceLastSawPlayer = Mathf.Infinity;
    private float _currentWaypointDwellTime = Mathf.Infinity;
    private float _timeSinceLastAggrevated = Mathf.Infinity;
    private int _currentWaypointIndex;
    private bool _hasAlertedNearbyUnits = false;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _weaponController = GetComponent<EnemyWeaponController>();
        _health = GetComponent<Health>();
        _player = GameObject.FindWithTag("Player");
    }

    private void Start()
    {
        _guardPosition = transform.position;
        _patrolPath = new List<Vector3>();

        for (int i = 0; i < _numOfPatrolPoints; i++)
        {
            _patrolPath.Add(RandomNavmeshLocation(transform.position, _patrolRadius));
        }

        _health.OnHit += Health_OnHit;
    }

    private void Health_OnHit()
    {
        _lastKnownPlayerPosition = _player.transform.position;
        AttackState();
    }

    private void Update()
    {
        if (IsAggrevated())
        {
            AttackState();
        }
        else if (_timeSinceLastSawPlayer < _suspicionStateTime)
        {
            SuspicionState();
        }
        else
        {
            PatrolState();
        }

        UpdateTimers();
    }

    public void Aggrevate()
    {
        _timeSinceLastAggrevated = 0f;
    }

    private void AlertNearbyUnits()
    {
        if (_hasAlertedNearbyUnits) return;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, _alertNearbyRadius, Vector3.up, 0f, _enemyLayerMask);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.TryGetComponent(out SoldierAI soldierAI))
            {
                if (soldierAI == this) continue;
                soldierAI.Aggrevate();
            }
        }
    }

    private void UpdateTimers()
    {
        _timeSinceLastSawPlayer += Time.deltaTime;
        _currentWaypointDwellTime += Time.deltaTime;
        _timeSinceLastAggrevated += Time.deltaTime;
    }

    private void AttackState()
    {
        _timeSinceLastSawPlayer = 0f;

        AlertNearbyUnits();
        _hasAlertedNearbyUnits = true;

        if (!HasLineOfSight()) return;

        RotateTowardsPlayer();
        _lastKnownPlayerPosition = _player.transform.position;

        _weaponController.Shoot();
    }

    private void SuspicionState()
    {
        _hasAlertedNearbyUnits = false;
        _agent.SetDestination(_lastKnownPlayerPosition);
    }

    private void PatrolState()
    {
        Vector3 nextPosition = _guardPosition;

        if (_patrolPath != null)
        {
            if (AtWaypoint())
            {
                _currentWaypointDwellTime = 0f;
                CycleWaypoint();
            }

            nextPosition = GetCurrentWaypoint();
        }

        if (_currentWaypointDwellTime > _waypointDwellTime)
        {
            _agent.SetDestination(nextPosition);
        }
    }

    // Check if there's a clear line of sight to the player
    private bool HasLineOfSight()
    {
        RaycastHit hit;
        Vector3 direction = _player.transform.position - transform.position;

        // Perform a raycast to check for obstacles
        if (Physics.Raycast(transform.position, direction, out hit, _chaseDistance, _obstacleLayerMask))
        {
            // If the raycast hits an obstacle, return false
            return false;
        }

        // If no obstacles are detected, return true (line of sight is clear)
        return true;
    }

    private void RotateTowardsPlayer()
    {
        Vector3 dirToPlayer = (_player.transform.position - transform.position).normalized;
        transform.forward = Vector3.Slerp(transform.forward, dirToPlayer, _rotationSpeed * Time.deltaTime);
    }

    private Vector3 GetCurrentWaypoint()
    {
        return _patrolPath[_currentWaypointIndex];
    }

    private void CycleWaypoint()
    {
        if (_currentWaypointIndex == _patrolPath.Count - 1)
        {
            _currentWaypointIndex = 0;
        }
        else
        {
            _currentWaypointIndex++;
        }
    }

    private bool AtWaypoint()
    {
        return Vector3.Distance(transform.position, GetCurrentWaypoint()) < _waypointDistanceTollerance;
    }

    private bool IsAggrevated()
    {
        if (_player == null) return false;
        
        float distanceToPlayerSq = (transform.position - _player.transform.position).sqrMagnitude;
        return (distanceToPlayerSq < Mathf.Pow(_chaseDistance, 2) && HasLineOfSight()) || _timeSinceLastAggrevated < aggroCooldownTime;
    }

    private Vector3 RandomNavmeshLocation(Vector3 origin, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;

        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas);

        return hit.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, _chaseDistance);
    }
}
