using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        PistolShot,
        RifleShot,
        ShotgunShot,
        RPGShot,
        ObjectHit,
        EnemyHit,
        PlayerHit,
        EnemyDeath,
        PlayerDeath,
        EnemyAttack,
    }

    public static void PlaySound(Sound sound, Vector3 position)
    {
        GameObject soundGO = new GameObject("Sound");
        soundGO.transform.position = position;
        AudioSource audioSource = soundGO.AddComponent<AudioSource>();
        audioSource.clip = GetAudioClip(sound);
        audioSource.maxDistance = 100f;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.dopplerLevel = 0f;

        audioSource.Play();
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach(var soundAudioClip in GameAssets.Instance.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }

        return null;
    }
}
