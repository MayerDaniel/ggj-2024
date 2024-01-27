using UnityEngine;

public class Obstacle
{
    public enum Type { Rectangle, Circle };

    public Type ObstacleType;
    public ObstacleData Data;

    public Obstacle(Vector2 center, Vector2 size) //rectangle
    {
        ObstacleType = Type.Rectangle;
        Data = new RectangleObstacleData(size.x, size.y);
    }

    public Obstacle(Vector2 center, float radius) //circle
    {
        ObstacleType = Type.Circle;
        Data = new CircleObstacleData(radius);
    }
}

public abstract class ObstacleData
{
    public Vector2 Center;
}

public class RectangleObstacleData : ObstacleData
{
    public float Width;
    public float Height;

    public float Left => Center.x - Width / 2f;
    public float Right => Center.x + Width / 2f;
    public float Top => Center.y + Height / 2f;
    public float Bottom => Center.y - Height / 2f;

    public RectangleObstacleData(float width, float height)
    {
        Width = width;
        Height = height;
    }
}

public class CircleObstacleData : ObstacleData
{
    public float Radius;

    public CircleObstacleData(float radius)
    {
        Radius = radius;
    }
}
