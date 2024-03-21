using System;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public event Action<Vector3> OnDeath;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        _health.OnDeath += OnDeath;
    }
}
