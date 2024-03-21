using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
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
