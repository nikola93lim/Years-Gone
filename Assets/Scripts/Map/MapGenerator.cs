using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Transform _tilePrefab;
    [SerializeField] private Transform _obstaclePrefab;
    [SerializeField] private Transform _navMeshFloor;
    [SerializeField] private Transform _navMeshMaskPrefab;

    [SerializeField] private Vector2 _maxMapSize;

    [SerializeField]
    [Range(0f, 1f)]
    private float _tileOutlinePercentage;

    [SerializeField] private float _tileSize = 1f;

    private List<Coord> _allTilesCoords;
    private Queue<Coord> _shuffledTilesCoords;

    [SerializeField] private Map[] _maps;
    [SerializeField]  private int _mapIndex;
    private Map _currentMap;

    private Transform[,] _tileMap;
    private List<Coord> _allOpenTilesCoords;
    private Queue<Coord> _shuffledOpenTilesCoords;


    private void Start()
    {
        FindObjectOfType<Spawner>().OnNewWave += Spawner_OnNewWave;
    }

    private void Spawner_OnNewWave(int waveIndex)
    {
        _mapIndex = waveIndex;
        GenerateMap();
    }

    public void GenerateMap()
    {
        _currentMap = _maps[_mapIndex];
        _tileMap = new Transform[_currentMap._mapSize.x, _currentMap._mapSize.y];

        System.Random prng = new System.Random(_currentMap._seed);

        GetComponent<BoxCollider>().size = new Vector3(_currentMap._mapSize.x * _tileSize, 0.05f, _currentMap._mapSize.y * _tileSize);
        CreateAllTilesCoordinates();

        _allOpenTilesCoords = new List<Coord>(_allTilesCoords);
        _shuffledTilesCoords = new Queue<Coord>(Utility.ShuffleArray(_allTilesCoords.ToArray(), _currentMap._seed));

        Transform holder = CreateHolderParent();
        CreateTiles(holder);
        CreateObstacles(prng, holder);

        _shuffledOpenTilesCoords = new Queue<Coord>(Utility.ShuffleArray(_allOpenTilesCoords.ToArray(), _currentMap._seed));

        CreateNavMeshMasks(holder);

        _navMeshFloor.localScale = new Vector3(_maxMapSize.x, _maxMapSize.y) * _tileSize;
    }

    private void CreateAllTilesCoordinates()
    {
        _allTilesCoords = new List<Coord>();
        for (int x = 0; x < _currentMap._mapSize.x; x++)
        {
            for (int y = 0; y < _currentMap._mapSize.y; y++)
            {
                _allTilesCoords.Add(new Coord(x, y));
            }
        }
    }

    private void CreateObstacles(System.Random prng, Transform holder)
    {
        bool[,] obstacleMap = new bool[_currentMap._mapSize.x, _currentMap._mapSize.y];
        int currentObstacleCount = 0;

        int obstacleCount = (int)(_currentMap._mapSize.x * _currentMap._mapSize.y * _currentMap._obstaclePercentage);

        for (int i = 0; i < obstacleCount; i++)
        {
            Coord randomCoord = GetRandomCoord();
            obstacleMap[randomCoord.x, randomCoord.y] = true;
            currentObstacleCount++;

            if (randomCoord != _currentMap.MapCentre && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
            {
                float obstacleHeight = Mathf.Lerp(_currentMap._minObstacleHeight, _currentMap._maxObstacleHeight, (float)prng.NextDouble());
                Vector3 obstaclePosition = CoordToPosition(randomCoord.x, randomCoord.y);
                Transform newObstacle = Instantiate(_obstaclePrefab, obstaclePosition + Vector3.up * obstacleHeight / 2, Quaternion.identity);
                newObstacle.transform.localScale = new Vector3((1f - _tileOutlinePercentage) * _tileSize, obstacleHeight, (1f - _tileOutlinePercentage) * _tileSize);
                newObstacle.parent = holder;

                _allOpenTilesCoords.Remove(randomCoord);

                Renderer obstacleRenderer = newObstacle.GetComponent<Renderer>();
                Material obstacleMaterial = new Material(obstacleRenderer.sharedMaterial);

                float colourPercent = (float)randomCoord.y / _currentMap._mapSize.y;
                obstacleMaterial.color = Color.Lerp(_currentMap._foregroundColour, _currentMap._backgroundColour, colourPercent);

                obstacleRenderer.sharedMaterial = obstacleMaterial;
            }
            else
            {
                obstacleMap[randomCoord.x, randomCoord.y] = false;
                currentObstacleCount--;
            }
        }
    }

    private void CreateTiles(Transform holder)
    {
        for (int x = 0; x < _currentMap._mapSize.x; x++)
        {
            for (int y = 0; y < _currentMap._mapSize.y; y++)
            {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(_tilePrefab, tilePosition, Quaternion.Euler(Vector3.right * 90f));
                newTile.transform.localScale = (1f - _tileOutlinePercentage) * _tileSize * Vector3.one;
                newTile.parent = holder;
                _tileMap[x, y] = newTile;
            }
        }
    }

    private Transform CreateHolderParent()
    {
        string tileHolderName = "Generated Map";
        Transform tileHolder = transform.Find(tileHolderName);

        if (tileHolder != null)
        {
            Destroy(tileHolder.gameObject);
        }

        Transform holder = new GameObject(tileHolderName).transform;
        holder.parent = transform;
        return holder;
    }

    private void CreateNavMeshMasks(Transform holder)
    {
        Transform leftMask = Instantiate(_navMeshMaskPrefab, Vector3.left * (_currentMap._mapSize.x + _maxMapSize.x) / 4f * _tileSize, Quaternion.identity);
        leftMask.parent = holder;
        leftMask.localScale = new Vector3((_maxMapSize.x - _currentMap._mapSize.x) / 2f, 1f, _currentMap._mapSize.y) * _tileSize;

        Transform rightMask = Instantiate(_navMeshMaskPrefab, Vector3.right * (_currentMap._mapSize.x + _maxMapSize.x) / 4f * _tileSize, Quaternion.identity);
        rightMask.parent = holder;
        rightMask.localScale = new Vector3((_maxMapSize.x - _currentMap._mapSize.x) / 2f, 1f, _currentMap._mapSize.y) * _tileSize;

        Transform topMask = Instantiate(_navMeshMaskPrefab, Vector3.forward * (_currentMap._mapSize.y + _maxMapSize.y) / 4f * _tileSize, Quaternion.identity);
        topMask.parent = holder;
        topMask.localScale = new Vector3(_maxMapSize.x, 1f, (_maxMapSize.y - _currentMap._mapSize.y) / 2f) * _tileSize;

        Transform bottomMask = Instantiate(_navMeshMaskPrefab, Vector3.back * (_currentMap._mapSize.y + _maxMapSize.y) / 4f * _tileSize, Quaternion.identity);
        bottomMask.parent = holder;
        bottomMask.localScale = new Vector3(_maxMapSize.x, 1f, (_maxMapSize.y - _currentMap._mapSize.y) / 2f) * _tileSize;
    }

    private bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> coordsQueue = new Queue<Coord>();
        coordsQueue.Enqueue(_currentMap.MapCentre);

        mapFlags[_currentMap.MapCentre.x, _currentMap.MapCentre.y] = true;

        int accesibleTileCount = 1;

        while (coordsQueue.Count > 0)
        {
            Coord tile = coordsQueue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;

                    if (x == 0 ||  y == 0)
                    {
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) &&  neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {
                            if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                coordsQueue.Enqueue(new Coord(neighbourX, neighbourY));
                                accesibleTileCount++;
                            }
                        }
                        
                    }
                }
            }
        }

        int targetAccesibleTileCount = (int)(_currentMap._mapSize.x * _currentMap._mapSize.y - currentObstacleCount);

        return accesibleTileCount == targetAccesibleTileCount;
    }

    private Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-_currentMap._mapSize.x / 2f + 0.5f + x, 0f, -_currentMap._mapSize.y / 2f + 0.5f + y) * _tileSize;
    }

    private Coord GetRandomCoord()
    {
        Coord randomCoord = _shuffledTilesCoords.Dequeue();
        _shuffledTilesCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public Transform GetRandomOpenTile()
    {
        Coord randomCoord = _shuffledOpenTilesCoords.Dequeue();
        _shuffledOpenTilesCoords.Enqueue(randomCoord);
        return _tileMap[randomCoord.x, randomCoord.y];
    }

    public Transform GetTileFromPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / _tileSize + (_currentMap._mapSize.x - 1) / 2f);
        int y = Mathf.RoundToInt(position.z / _tileSize + (_currentMap._mapSize.y - 1) / 2f);

        x = Mathf.Clamp(x, 0, _tileMap.GetLength(0) - 1);
        y = Mathf.Clamp(y, 0, _tileMap.GetLength(1) - 1);

        return _tileMap[x, y];
    }

    [Serializable]
    public struct Coord
    {
        public int x, y;

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Coord a, Coord b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Coord a, Coord b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    [Serializable]
    public class Map
    {
        public Coord _mapSize;
        public int _seed;
        [Range(0f, 1f)]
        public float _obstaclePercentage;

        public float _minObstacleHeight;
        public float _maxObstacleHeight;
        public Color _foregroundColour;
        public Color _backgroundColour;

        public Coord MapCentre
        {
            get { return new Coord(_mapSize.x / 2, _mapSize.y / 2); }
        }
    }
}
