using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private uint _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private GameObject _slicerTrap;
    [SerializeField] private GameObject _fireTrap;
    [SerializeField] private Transform _environment;

    public static GridManager gridManager;

    [Header("Random path config")]
    [SerializeField] private Vector2 _pathPosition;
    [SerializeField] private uint _maxDistanceForward = 50;

    public uint MaxDistanceForward
    {
        get { return _maxDistanceForward; }
        private set { _maxDistanceForward = value; }
    }

    private List<Vector2> _spawns = new List<Vector2>();


    private void Awake()
    {
        gridManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gridManager);

        GenerateStartGrid();
        _pathPosition = new Vector2(_width / 2, _height);
        GeneratePath();
        GenerateEndGrid();
    }

    void GenerateStartGrid()
    {
        for (uint x = 0; x < _width; x++)
        {
            for (uint y = 0; y < _height; y++)
            {
                Tile tileSpawned = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                tileSpawned.name = $"Tile [{x}, {y}]";

                bool isOffset = (x + y) % 2 == 0;
                tileSpawned.Init(isOffset, true, _width, _maxDistanceForward + _height);
                tileSpawned.transform.parent = _environment;
            }
        }
    }

    void GenerateEndGrid()
    {
        for (uint x = 0; x < _width; x++)
        {
            for (uint y = 0; y < _height; y++)
            {
                Tile tileSpawned = Instantiate(_tilePrefab, new Vector3(x, _pathPosition.y + y), Quaternion.identity);
                tileSpawned.name = $"Tile [{x}, {y}]";

                bool isOffset = (x + y) % 2 == 0;
                tileSpawned.Init(isOffset, true, _width, _maxDistanceForward + _height);
                tileSpawned.transform.parent = _environment;
            }
        }
    }

    void GeneratePath()
    {
        //Move fixed three times upwards
        for (int i = 0; i < 3; i++)
        {
            var x = _pathPosition.x;
            var y = _pathPosition.y;
            Tile tileSpawned = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
            tileSpawned.name = $"Tile [{x}, {y}]";

            bool isOffset = (x + y) % 2 == 0;
            tileSpawned.Init(isOffset, false);
            tileSpawned.transform.parent = _environment;

            _pathPosition += Vector2.up;
        }

        //Extra tile needed
        Tile extraTile = Instantiate(_tilePrefab, _pathPosition, Quaternion.identity);
        extraTile.name = $"Tile [{_pathPosition.x}, {_pathPosition.y}]";
        bool isOffsetExtra = (_pathPosition.x + _pathPosition.y) % 2 == 0;
        extraTile.Init(isOffsetExtra, false);
        extraTile.transform.parent = _environment;

        while (_pathPosition.y < _maxDistanceForward)
        {
            float rand = UnityEngine.Random.Range(0.0f, 1.0f);

            if (rand < 0.05f)
            {
                GenerateRandomTrap();
            }
            else
            {
                GenerateRandomTile();
            }

            Utilities.ExitFromInfiniteLoopAfterSeconds(20.0f, out bool shouldExit);

            if (shouldExit) break;
        }
    }

    void GenerateRandomTile()
    {
        Direction[] randValues = (Direction[])System.Enum.GetValues(typeof(Direction));
        Direction rand = randValues[UnityEngine.Random.Range(0, randValues.Length)];

        switch (rand)
        {
            case Direction.North:
                _pathPosition += Vector2.up;
                break;
            case Direction.East:
                if (_pathPosition.x == _width - 2) return;
                _pathPosition += Vector2.right;
                break;
            case Direction.West:
                if (_pathPosition.x == 1) return;
                _pathPosition += Vector2.left;
                break;
        }

        var x = _pathPosition.x;
        var y = _pathPosition.y;
        Collider2D[] results = new Collider2D[4];

        if (Physics2D.OverlapBoxNonAlloc(_pathPosition, Vector2.one * 0.7f, 0, results) == 0)
        {
            Tile tileSpawned = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
            tileSpawned.name = $"Tile [{x}, {y}]";

            bool isOffset = (x + y) % 2 == 0;
            tileSpawned.Init(isOffset, false);
            tileSpawned.transform.parent = _environment;
        }
        else
        {
            _spawns.Add(_pathPosition);
        }
    }

    private void GenerateRandomTrap()
    {
        //Move up a unit
        _pathPosition += Vector2.up;

        //Generate a trap
        var trapToInstantiate = UnityEngine.Random.Range(0f, 1f) > 0.5f ? _slicerTrap : _fireTrap;
        float instantiatePoint = trapToInstantiate == _slicerTrap ? 4 : _pathPosition.x;

        var trap = Instantiate(trapToInstantiate, new Vector3(instantiatePoint, _pathPosition.y), Quaternion.identity);
        trap.transform.parent = _environment;

        //Move up another unit
        _pathPosition += Vector2.up;

        //Instantiate a tile
        var x = _pathPosition.x;
        var y = _pathPosition.y;

        Tile tileSpawned = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
        tileSpawned.name = $"Tile [{x}, {y}]";

        bool isOffset = (x + y) % 2 == 0;
        tileSpawned.Init(isOffset, false);
        tileSpawned.transform.parent = _environment;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach(var pos in _spawns)
        {
            Gizmos.DrawWireCube(pos, Vector2.one * 0.7f);
        }
    }
}

public enum Direction
{
    North, East, West
}