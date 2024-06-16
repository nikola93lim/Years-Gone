using UnityEngine;

public class AmmoPickupSpawner : PickupSpawner
{
    [SerializeField] private int _ammoAmount;

    public override void Pickup(Collider other)
    {
        Debug.Log("Picked up!");
    }
}
