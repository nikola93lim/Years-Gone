using UnityEngine;

public class ObjectHitParticle : Flyweight
{
    private void OnParticleSystemStopped()
    {
        FlyweightFactory.ReturnToPool(this);
    }
}
