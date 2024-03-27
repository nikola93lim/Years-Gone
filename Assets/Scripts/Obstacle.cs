using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private ObjectHitParticleSettings _objectHitParticleSettings;
    [SerializeField] private SoundObjectFXSettings _soundObjectFXSettings;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Projectile>() != null)
        {
            CreateParticleFX(other);
            CreateSoundFX();
        }
    }

    private void CreateSoundFX()
    {
        SoundObjectFX soundFX = FlyweightFactory.Spawn(_soundObjectFXSettings) as SoundObjectFX;
        soundFX.transform.position = transform.position;
        soundFX.PlaySound();
    }

    private void CreateParticleFX(Collider other)
    {
        ObjectHitParticle objectHitParticle = FlyweightFactory.Spawn(_objectHitParticleSettings) as ObjectHitParticle;
        objectHitParticle.transform.SetPositionAndRotation(other.ClosestPointOnBounds(transform.position), Quaternion.identity);
    }
}
