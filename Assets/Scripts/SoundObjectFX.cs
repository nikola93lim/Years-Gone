using System.Collections;
using UnityEngine;

public class SoundObjectFX : Flyweight
{
    private AudioSource _audioSource;
    private WaitForSeconds _lifetime;

    public void PlaySound()
    {
        _audioSource.Play();
        StartCoroutine(ReturnToPool());
    }

    public void SetAudioClip(AudioClip clip)
    {
        _audioSource = gameObject.GetOrAdd<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.maxDistance = 100f;
        _audioSource.spatialBlend = 1f;
        _audioSource.rolloffMode = AudioRolloffMode.Linear;
        _audioSource.dopplerLevel = 0f;
        _audioSource.clip = clip;
        _lifetime = new WaitForSeconds(_audioSource.clip.length);
    }    

    private IEnumerator ReturnToPool()
    {
        yield return _lifetime;
        FlyweightFactory.ReturnToPool(this);
    }
}
