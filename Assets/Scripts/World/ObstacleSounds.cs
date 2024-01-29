using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSounds : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _audioSource.Play();
    }
}
