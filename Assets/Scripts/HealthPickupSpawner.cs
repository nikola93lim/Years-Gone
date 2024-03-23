using UnityEngine;

public class HealthPickupSpawner : PickupSpawner
{
    [SerializeField] private int _healAmount;

    public override void Pickup(Collider other)
    {
        other.GetComponent<Health>().Heal(_healAmount);
    }
}
