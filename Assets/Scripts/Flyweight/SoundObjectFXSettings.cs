using UnityEngine;

[CreateAssetMenu(fileName = "SoundObjectFXSettings", menuName = "Flyweight/Sound Object FX Settings")]
public class SoundObjectFXSettings : FlyweightSettings
{
    public AudioClip AudioClip;

    public override Flyweight Create()
    {
        var go = Instantiate(Prefab);
        go.SetActive(false);
        go.name = Prefab.name;

        var flyweight = go.GetOrAdd<SoundObjectFX>();
        flyweight.Settings = this;
        flyweight.SetAudioClip(AudioClip);

        return flyweight;
    }
}
