using System;
using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour 
{
    public event Action<int> OnNewWave;

    [SerializeField] private Wave[] _waves;
    [SerializeField] private Enemy _enemyPrefab;

    private MapGenerator _mapGenerator;
    private Health _playerHealth;

    private Wave _currentWave;
    private int _currentWaveIndex;

    private int _enemiesRemainingToSpawn;
    private int _enemiesRemainingAlive;

    private float _nextSpawnTime;

    private float _campingThreshold = 1.5f;
    private Vector3 _oldCampingPosition;
    private float _nextCampingCheckTime;
    private float _timeBetweenCampingChecks = 2f;
    private bool _isCamping;

    private bool _shouldSpawn = true;

    private void Start()
    {
        _mapGenerator = FindObjectOfType<MapGenerator>();
        _playerHealth = FindObjectOfType<Player>().GetComponent<Health>();

        _playerHealth.OnDeath += OnPlayerDeath;

        _nextCampingCheckTime = Time.time + _timeBetweenCampingChecks;
        _oldCampingPosition = _playerHealth.transform.position;
        NextWave();
    }

    private void OnPlayerDeath(Vector3 hitDirection)
    {
        _shouldSpawn = false;
    }

    private void Update()
    {
        if (!_shouldSpawn) return;

        if (Time.time > _nextCampingCheckTime)
        {
            _nextCampingCheckTime = Time.time + _timeBetweenCampingChecks;
            _isCamping = Vector3.Distance(_playerHealth.transform.position, _oldCampingPosition) < _campingThreshold;
            _oldCampingPosition = _playerHealth.transform.position;
        }

        if (_enemiesRemainingToSpawn > 0 && Time.time > _nextSpawnTime)
        {
            _enemiesRemainingToSpawn--;
            _nextSpawnTime = Time.time + _currentWave.timeBetweenSpawns;
            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator SpawnEnemy()
    {
        Transform openTile = _mapGenerator.GetRandomOpenTile();

        if (_isCamping)
        {
            openTile = _mapGenerator.GetTileFromPosition(_playerHealth.transform.position);
        }

        float tileFlashTime = 1f;
        float tileFlashSpeed = 4f;

        float spawnTimer = 0f;

        Material tileMaterial = openTile.GetComponent<Renderer>().material;
        Color tileOriginalColour = tileMaterial.color;
        Color tileFlashColour = Color.red;

        while (spawnTimer < tileFlashTime)
        {
            spawnTimer += Time.deltaTime;
            tileMaterial.color = Color.Lerp(tileOriginalColour, tileFlashColour, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1f));

            yield return null;
        }

        Enemy spawnedEnemy = Instantiate(_enemyPrefab, openTile.position + Vector3.up, Quaternion.identity);
        spawnedEnemy.OnDeath += SpawnedEnemy_OnDeath;
    }

    private void SpawnedEnemy_OnDeath(Vector3 hitDirection)
    {
        _enemiesRemainingAlive--;

        if (_enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    private void NextWave()
    {
        if (_currentWaveIndex > _waves.Length - 1) return;

        _currentWave = _waves[_currentWaveIndex];
        
        _enemiesRemainingToSpawn = _currentWave.enemyCount;
        _enemiesRemainingAlive = _currentWave.enemyCount;

        OnNewWave?.Invoke(_currentWaveIndex);
        _currentWaveIndex++;

        ResetPlayerPosition();
    }

    private void ResetPlayerPosition()
    {
        _playerHealth.transform.position = _mapGenerator.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3f;
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}

