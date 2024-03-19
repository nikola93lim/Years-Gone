using System;
using UnityEngine;
public class Health : MonoBehaviour
{
    public event Action<Vector3> OnDeath;
    public event Action OnHit;
    [SerializeField] private int _startingHealth;

    private float _currentHealth;
    private bool _isAlive = true;

    private void Awake()
    {
        _currentHealth = _startingHealth;
    }

    public void TakeHit(int damageAmount, Vector3 hitDirection)
    {
        _currentHealth -= damageAmount;

        if (_currentHealth <= 0 && _isAlive)
        {
            Die(hitDirection);
        }
        else
        {
            OnHit?.Invoke();
        }
    }

    public bool IsAlive()
    {
        return _isAlive;
    }

    private void Die(Vector3 hitDirection)
    {
        _isAlive = false;
        OnDeath?.Invoke(hitDirection);
        Destroy(gameObject);
    }
}

