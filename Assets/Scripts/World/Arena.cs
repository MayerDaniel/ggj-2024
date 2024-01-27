using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [Header("External Objects")]
    [SerializeField] private Transform _voltron;
    [SerializeField] private float _voltronRadius;
    [SerializeField] private Transform _goal;
    private Obstacle _voltronObstacle;

    [Header("Initial Setup")]
    [SerializeField] private int _startingObstacles = 10;
    [SerializeField] private float _leftEdge = -9f;
    [SerializeField] private float _rightEdge = 9f;
    [SerializeField] private float _topEdge = -5f;
    [SerializeField] private float _bottomEdge = 5f;

    [Header("General Obstacle Variables")]
    [SerializeField] private float _minDistBetweenObstacles = 0.5f;
    [SerializeField] private int _maxPlacementAttempts = 100;
    [SerializeField] [Range(0, 1)] private float _specialObstacleChance = 0f;
    private List<Obstacle> _obstacles;

    [Header("Fixed Block Obstacle")]
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private GameObject[] _specialBlocks;
    [SerializeField] private float _minBlockSize = 0.5f;
    [SerializeField] private float _maxBlockSize = 3f;

    [Header("Fixed Round Obstacle")]
    [SerializeField] private GameObject _roundPrefab;
    [SerializeField] private GameObject[] _specialRounds;
    [SerializeField] private float _minDiameter = 0.5f;
    [SerializeField] private float _maxDiameter = 3f;

    private void Start()
    {
        _voltronObstacle = new Obstacle(_voltron.position, _voltronRadius);
        _obstacles = new List<Obstacle>();

        for (int i = 0; i < _startingObstacles; i++)
            AddNormalObstacle();

        if (_goal != null)
            PlaceGoal();
    }

    public void AddNewObstacle()
    {
        float rand = Random.value;

        if (rand < _specialObstacleChance)
            AddSpecialObstacle();
        else
            AddNormalObstacle();
    }

    private void AddSpecialObstacle()
    {
        if (_specialBlocks.Length == 0 && _specialRounds.Length == 0)
        {
            AddNormalObstacle();
            return;
        }

        int i = Random.Range(0, _specialRounds.Length + _specialBlocks.Length);
        bool isRound = i > _specialBlocks.Length;
        GameObject prefab = isRound ?
            _specialRounds[i - _specialBlocks.Length] : 
            _specialBlocks[i];

        if (isRound)
            AddNewRoundObstacle(prefab);
        else
            AddNewBlockObstacle(prefab);
    }

    private void AddNormalObstacle()
    {
        float rand = Random.value;
        if (rand > 0.5f)
            AddNewRoundObstacle(_roundPrefab);
        else
            AddNewBlockObstacle(_blockPrefab);
    }

    private void AddNewRoundObstacle(GameObject prefab)
    {
        var diameter = Random.Range(_minDiameter, _maxDiameter);
        var newObstacle = new Obstacle(Vector2.zero, diameter / 2f);
        PlaceNewObstacle(newObstacle, prefab, Vector2.one * diameter);
    }

    private void AddNewBlockObstacle(GameObject prefab)
    {
        var size = new Vector2(
            Random.Range(_minBlockSize, _maxBlockSize), 
            Random.Range(_minBlockSize, _maxBlockSize));
        var newObstacle = new Obstacle(Vector2.zero, size);
        PlaceNewObstacle(newObstacle, prefab, size);
    }

    private void PlaceNewObstacle(Obstacle newObstacle, GameObject prefab, Vector2 size)
    {
        if (!GetObstaclePos(newObstacle, _minDistBetweenObstacles).HasValue)
            return;

        var blockObj = Instantiate(prefab, newObstacle.Data.Center, Quaternion.identity);
        blockObj.transform.localScale = new Vector3(size.x, size.y, 1);
        _obstacles.Add(newObstacle);
    }

    private Vector2? GetObstaclePos(Obstacle obstacle, float minDist)
    {
        int placementAttempts = 0;
        while (true)
        {
            var testPoint = new Vector2(
                Random.Range(_leftEdge, _rightEdge),
                Random.Range(_bottomEdge, _topEdge));
            obstacle.Data.Center = testPoint;

            if (IsPlacementValid(obstacle, minDist))
                break;

            placementAttempts++;
            if (placementAttempts >= _maxPlacementAttempts)
                return null;
        }

        return obstacle.Data.Center;
    }

    private bool IsPlacementValid(Obstacle testObstacle, float minDist)
    {
        _voltronObstacle.Data.Center = _voltron.position;
        if (IsOverlapping(testObstacle, _voltronObstacle, 0))
            return false;

        foreach(var obstacle in _obstacles)
        {
            if (IsOverlapping(testObstacle, obstacle, minDist))
                return false;
        }

        return true;
    }

    private bool IsOverlapping(Obstacle o1, Obstacle o2, float minDist)
    {
        //both round
        if (o1.ObstacleType == Obstacle.Type.Circle && o2.ObstacleType == Obstacle.Type.Circle)
        {
            float dist = Vector2.Distance(o1.Data.Center, o2.Data.Center);
            float r1 = (o1.Data as CircleObstacleData).Radius;
            float r2 = (o2.Data as CircleObstacleData).Radius;
            return dist - r1 - r2 < minDist;
        }
        //both rectangular
        else if (o1.ObstacleType == Obstacle.Type.Rectangle && o2.ObstacleType == Obstacle.Type.Rectangle)
        {
            float xDist;
            float yDist;
            var data1 = o1.Data as RectangleObstacleData;
            var data2 = o2.Data as RectangleObstacleData;

            if (data1.Right < data2.Left)
                xDist = data2.Left - data1.Right;
            else if (data2.Right < data1.Left)
                xDist = data1.Left - data2.Right;
            else
                xDist = 0;

            if (data1.Top < data2.Bottom)
                yDist = data2.Bottom - data1.Top;
            else if (data2.Top < data1.Bottom)
                yDist = data1.Bottom - data2.Top;
            else
                yDist = 0;

            var dist = Mathf.Sqrt(xDist * xDist + yDist * yDist);
            return dist < minDist;
        }
        //circle and rectangle
        else
        {
            var circle = o1.ObstacleType == Obstacle.Type.Circle ? o1.Data as CircleObstacleData : o2.Data as CircleObstacleData;
            var rect = o1.ObstacleType == Obstacle.Type.Rectangle ? o1.Data as RectangleObstacleData : o2.Data as RectangleObstacleData;

            Vector2 rectClosestPoint;

            if (circle.Center.x < rect.Left)
            {
                if (circle.Center.y > rect.Top)
                    rectClosestPoint = new Vector2(rect.Left, rect.Top);
                else if (circle.Center.y < rect.Bottom)
                    rectClosestPoint = new Vector2(rect.Left, rect.Bottom);
                else
                    rectClosestPoint = new Vector2(rect.Left, circle.Center.y);
            }
            else if (circle.Center.x > rect.Right)
            {
                if (circle.Center.y > rect.Top)
                    rectClosestPoint = new Vector2(rect.Right, rect.Top);
                else if (circle.Center.y < rect.Bottom)
                    rectClosestPoint = new Vector2(rect.Right, rect.Bottom);
                else
                    rectClosestPoint = new Vector2(rect.Right, circle.Center.y);
            }
            else
            {
                if (circle.Center.y > rect.Top)
                    rectClosestPoint = new Vector2(circle.Center.x, rect.Top);
                else if (circle.Center.y < rect.Bottom)
                    rectClosestPoint = new Vector2(circle.Center.x, rect.Bottom);
                else //center of circle within bounds of rectangle
                    return true;
            }

            var dist = Vector2.Distance(rectClosestPoint, circle.Center);
            return dist - circle.Radius < minDist;
        }
    }

    public void PlaceGoal()
    {
        Obstacle goalObstacle = new Obstacle(Vector2.zero, _goal.localScale.x);

        while (true)
        {
            if (GetObstaclePos(goalObstacle, 0).HasValue)
            {
                _goal.position = goalObstacle.Data.Center;
                return;
            }
        }
    }
}
