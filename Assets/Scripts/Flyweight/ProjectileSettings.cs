using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSettings", menuName = "Flyweight/Projectile Settings")]
public class ProjectileSettings : FlyweightSettings
{
    public LayerMask CollisionMask;
    public ObjectHitParticleSettings ObjectHitParticleSettings;
    public float MoveSpeed = 30f;
    public int Damage = 1;
    public float Lifetime = 3f;

    public override Flyweight Create()
    {
        var go = Instantiate(Prefab);
        go.SetActive(false);
        go.name = Prefab.name;

        var flyweight = go.GetOrAdd<Projectile>();
        flyweight.Settings = this;

        return flyweight;
    }

    public override void OnGet(Flyweight flyweight)
    {
        base.OnGet(flyweight);
        ((Projectile)flyweight).ReactivateTrailRenderer();
    }

    public override void OnRelease(Flyweight flyweight)
    {
        base.OnRelease(flyweight);
        ((Projectile)flyweight).DeactivateTrailRenderer();
    }
}
