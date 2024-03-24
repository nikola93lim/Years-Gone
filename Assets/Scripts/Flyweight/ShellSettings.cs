using UnityEngine;

[CreateAssetMenu(fileName = "ShellSettings", menuName = "Flyweight/Shell Settings")]
public class ShellSettings : FlyweightSettings
{
    public float MinForce = 90;
    public float MaxForce = 120;
    public float Lifetime = 4f;
    public float FadeTime = 2f;

    public override Flyweight Create()
    {
        var go = Instantiate(Prefab);
        go.SetActive(false);
        go.name = Prefab.name;

        var flyweight = go.GetOrAdd<Shell>();
        flyweight.Settings = this;

        return flyweight;
    }

    public override void OnRelease(Flyweight flyweight)
    {
        base.OnRelease(flyweight);
        ((Shell)flyweight).ResetColour();
    }
}
