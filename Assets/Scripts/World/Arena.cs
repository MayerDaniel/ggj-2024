using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [Header("Initial Setup")]
    [SerializeField] private int _startingObstacles;
    [SerializeField] private float _leftEdge;
    [SerializeField] private float _rightEdge;
    [SerializeField] private float _topEdge;
    [SerializeField] private float _bottomEdge;

    [Header("General Obstacle Variables")]
    [SerializeField] private float _minDistBetweenObstacles;
    [SerializeField] private int _maxPlacementAttempts = 100;
    private List<Obstacle> _obstacles;

    [Header("Fixed Block Obstacle")]
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private float _maxBlockSize;
    [SerializeField] private float _minBlockSize;

    [Header("Fixed Round Obstacle")]
    [SerializeField] private GameObject _roundPrefab;
    [SerializeField] private float _minRadius;
    [SerializeField] private float _maxRadius;

    private void Start()
    {
        _obstacles = new List<Obstacle>();

        for (int i = 0; i < _startingObstacles; i++)
            AddNewBlockObstacle();
    }

    public void AddNewBlockObstacle()
    {
        var size = new Vector2(
            Random.Range(_minBlockSize, _maxBlockSize), 
            Random.Range(_minBlockSize, _maxBlockSize));

        var newObstacle = new Obstacle(Vector2.zero, size);

        int placementAttempts = 0;
        while (true)
        {
            var testPoint = new Vector2(
                Random.Range(_leftEdge, _rightEdge),
                Random.Range(_bottomEdge, _topEdge));
            newObstacle.Data.Center = testPoint;

            if (IsPlacementValid(newObstacle))
                break;

            placementAttempts++;
            if (placementAttempts >= _maxPlacementAttempts)
                return;
        }

        var blockObj = Instantiate(_blockPrefab, newObstacle.Data.Center, Quaternion.identity);
        blockObj.transform.localScale = new Vector3(size.x, size.y, 1);
        _obstacles.Add(newObstacle);
    }

    private bool IsPlacementValid(Obstacle testObstacle)
    {
        foreach(var obstacle in _obstacles)
        {
            if (IsOverlapping(testObstacle, obstacle, _minDistBetweenObstacles))
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
}
