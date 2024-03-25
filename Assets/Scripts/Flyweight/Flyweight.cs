using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Flyweight : MonoBehaviour
{
    public FlyweightSettings Settings; // intrinsic state
}


public abstract class FlyweightSettings : ScriptableObject
{
    public FlyweightType Type;
    public GameObject Prefab;

    public abstract Flyweight Create();

    public virtual void OnGet(Flyweight flyweight) => flyweight.gameObject.SetActive(true);
    public virtual void OnRelease(Flyweight flyweight) => flyweight.gameObject.SetActive(false);
    public virtual void OnDestroyPoolObject(Flyweight flyweight) => Destroy(flyweight.gameObject);
}

public enum FlyweightType
{
    Bullet,
    Missile,
    HomingMisiile,
    Shell,
    ObstacleHitParticle,
    BloodSplatterParticle,
    DeathParticle,
}

