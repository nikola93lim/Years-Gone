using UnityEngine;

[CreateAssetMenu(fileName = "ObjectHitParticleSettings", menuName = "Flyweight/Object Hit Particle Settings")]
public class ObjectHitParticleSettings : FlyweightSettings
{
    public override Flyweight Create()
    {
        var go = Instantiate(Prefab);
        go.SetActive(false);
        go.name = Prefab.name;

        var flyweight = go.GetOrAdd<ObjectHitParticle>();
        flyweight.Settings = this;

        return flyweight;
    }
}
