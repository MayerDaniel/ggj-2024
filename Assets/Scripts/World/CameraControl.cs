using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform _focus;

    private void Update()
    {
        transform.position = _focus.position + 10f * Vector3.back;
    }
}
