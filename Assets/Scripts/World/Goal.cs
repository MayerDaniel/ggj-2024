using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [Header("External")]
    [SerializeField] private Arena _arena;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _arena.AddNewObstacle();
        _arena.PlaceGoal();
    }
}