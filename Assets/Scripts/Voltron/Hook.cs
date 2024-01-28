using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private GrapplingHook _hookManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _hookManager.GrabHook();
    }
}
