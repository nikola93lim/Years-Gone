using UnityEngine;

public abstract class PickupSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject _pickupPrefab;
    [SerializeField] protected float _rotationSpeed = 50f;
    public abstract void Pickup(Collider other);

    public virtual void Update() 
    {
        transform.Rotate(_rotationSpeed * Time.deltaTime * Vector3.up);
    }    

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Pickup(other);
            Destroy(gameObject);
        }
    }
}
