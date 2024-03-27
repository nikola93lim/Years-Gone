using System.Collections;
using TreeEditor;
using UnityEngine;

public class VFXManager : MonoBehaviour 
{
    [SerializeField] private ObjectHitParticleSettings _deathParticleSettings;
    [SerializeField] private ObjectHitParticleSettings _bloodSplatterParticleSettings;
    [SerializeField] private SoundObjectFXSettings _hitSoundSettings;
    [SerializeField] private SoundObjectFXSettings _deathSoundSettings;

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
        CreateParticleFX(_deathParticleSettings, hitDirection);
        CreateSoundFX(_deathSoundSettings);
    }

    private void Health_OnHit()
    {
        CreateParticleFX(_bloodSplatterParticleSettings, Vector3.zero);
        CreateSoundFX(_hitSoundSettings);

        StartCoroutine(FlashWhenHit());
    }

    private void CreateParticleFX(ObjectHitParticleSettings settings, Vector3 hitDirection)
    {
        ObjectHitParticle particleFX = FlyweightFactory.Spawn(settings) as ObjectHitParticle;
        particleFX.transform.SetPositionAndRotation(transform.position, Quaternion.FromToRotation(Vector3.forward, hitDirection));
    }

    private void CreateSoundFX(SoundObjectFXSettings settings)
    {
        SoundObjectFX soundFX = FlyweightFactory.Spawn(settings) as SoundObjectFX;
        soundFX.transform.position = transform.position;
        soundFX.PlaySound();
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

