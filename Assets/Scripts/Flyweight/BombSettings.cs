using UnityEngine;

[CreateAssetMenu(fileName = "BombSettings", menuName = "Flyweight/Bomb Settings")]
public class BombSettings : FlyweightSettings
{
    public float throwForce = 10f; // How much force is applied to the grenade
    public float arcHeight = 5f;
    public float detonationTime = 2f;
    public int damage = 5;

    public ObjectHitParticleSettings explosionParticles;
    public LayerMask damageLayerMask;
    public override Flyweight Create()
    {
        var go = Instantiate(Prefab);
        go.SetActive(false);
        go.name = Prefab.name;

        var flyweight = go.GetOrAdd<Bomb>();
        flyweight.Settings = this;

        return flyweight;
    }
    public override void OnGet(Flyweight flyweight)
    {
        base.OnGet(flyweight);
        ((Bomb)flyweight).ReactivateTrailRenderer();
    }

    public override void OnRelease(Flyweight flyweight)
    {
        base.OnRelease(flyweight);
        ((Bomb)flyweight).DeactivateTrailRenderer();
    }
}
