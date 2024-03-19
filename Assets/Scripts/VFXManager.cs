using System.Collections;
using TreeEditor;
using UnityEngine;

public class VFXManager : MonoBehaviour 
{
    [SerializeField] private ParticleSystem _deathParticleSystem;
    [SerializeField] private ParticleSystem _bloodSplatterParticleSystem;

    private Material _objectMaterial;
    private Color _objectOriginalColour;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _objectMaterial = GetComponentInChildren<Renderer>().material;
        _objectOriginalColour = _objectMaterial.color;
    }

    private void Start()
    {
        _health.OnHit += Health_OnHit;
        _health.OnDeath += Health_OnDeath;
    }

    private void Health_OnDeath(Vector3 hitDirection)
    {
        ParticleSystem deathVFX = Instantiate(_deathParticleSystem, transform.position, Quaternion.FromToRotation(Vector3.forward, hitDirection));
        Destroy(deathVFX, deathVFX.main.startLifetime.constant);
    }

    private void Health_OnHit()
    {
        ParticleSystem bloodSplatterVFX = Instantiate(_bloodSplatterParticleSystem, transform.position, Quaternion.Euler(Random.insideUnitSphere));
        Destroy(bloodSplatterVFX, bloodSplatterVFX.main.startLifetime.constant);
        StartCoroutine(FlashWhenHit());
    }

    private IEnumerator FlashWhenHit()
    {
        float flashTime = 0.2f;
        float flashSpeed = 4f;

        float flashTimer = 0f;

        while (flashTimer < flashTime)
        {
            flashTimer += Time.deltaTime;
            _objectMaterial.color = Color.Lerp(_objectOriginalColour, Color.red, Mathf.PingPong(flashTimer * flashSpeed, 1f));

            yield return null;
        }

        _objectMaterial.color = _objectOriginalColour;
    }
}

