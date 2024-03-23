using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FlyweightFactory : MonoBehaviour
{
    [SerializeField] private bool _collectionCheck = true;
    [SerializeField] private int _defaultCapacity = 10;
    [SerializeField] private int _maxPoolSize = 100;

    private static FlyweightFactory Instance;
    private readonly Dictionary<FlyweightType, IObjectPool<Flyweight>> _pools = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Flyweight Spawn(FlyweightSettings settings) => Instance.GetPoolFor(settings)?.Get();
    public static void ReturnToPool(Flyweight flyweight) => Instance.GetPoolFor(flyweight.Settings)?.Release(flyweight);

    private IObjectPool<Flyweight> GetPoolFor(FlyweightSettings settings)
    {
        if (_pools.TryGetValue(settings.Type, out IObjectPool<Flyweight> pool))  return pool;

        pool = new ObjectPool<Flyweight>(
            settings.Create,
            settings.OnGet,
            settings.OnRelease,
            settings.OnDestroyPoolObject,
            _collectionCheck,
            _defaultCapacity,
            _maxPoolSize
            );

        _pools.Add(settings.Type, pool);
        return pool;
    }
}

