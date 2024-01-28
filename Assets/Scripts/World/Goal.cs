using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [Header("External")]
    [SerializeField] private Arena _arena;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string colliderName = collision.gameObject.name;
        if (colliderName.Contains("ook"))
            return;

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (!child.name.Contains("Target"))
                child.SetParent(null);
        }

        _arena.AddNewObstacle();
        _arena.PlaceGoal();
    }
}