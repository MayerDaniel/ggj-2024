using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [Header("External")]
    [SerializeField] private Arena _arena;
    [SerializeField] private AudioSource _source;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string colliderName = collision.gameObject.name;
        if (colliderName.Contains("ook"))
            return;

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            Debug.Log(child.name);
            if (!child.name.Contains("Target"))
            {
                Debug.Log($"{child.name} unparented");
                child.SetParent(null);
            }
        }

        _source.Play();
        _arena.AddNewObstacle();
        _arena.PlaceGoal();
    }
}