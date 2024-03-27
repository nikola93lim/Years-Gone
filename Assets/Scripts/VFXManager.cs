using System.Collections;
using TreeEditor;
using UnityEngine;

public class VFXManager : MonoBehaviour 
{
    [SerializeField] private ObjectHitParticleSettings _deathParticleSettings;
    [SerializeField] private ObjectHitParticleSettings _bloodSplatterParticleSettings;

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
        ObjectHitParticle deathParticle = FlyweightFactory.Spawn(_deathParticleSettings) as ObjectHitParticle;
        deathParticle.transform.SetPositionAndRotation(transform.position, Quaternion.FromToRotation(Vector3.forward, hitDirection));

        SoundManager.PlaySound(SoundManager.Sound.EnemyDeath, transform.position);
    }

    private void Health_OnHit()
    {
        ObjectHitParticle bloodSplatterParticle = FlyweightFactory.Spawn(_bloodSplatterParticleSettings) as ObjectHitParticle;
        bloodSplatterParticle.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(Random.insideUnitSphere));

        SoundManager.PlaySound(SoundManager.Sound.EnemyHit, transform.position);

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

