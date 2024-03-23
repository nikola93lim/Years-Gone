using UnityEngine;

public class AmmoPickupSpawner : PickupSpawner
{
    [SerializeField] private int _ammoAmount;

    public override void Pickup(Collider other)
    {
        throw new System.NotImplementedException();
    }
}
