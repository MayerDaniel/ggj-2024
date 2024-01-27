using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private bool _clockwise;
    [SerializeField] private float _rotSpeed; //degrees per second

    void Start()
    {
        StartCoroutine(Spin());
    }

    private IEnumerator Spin()
    {
        while (true)
        {
            Vector3 dRot = Vector3.forward * Time.deltaTime * _rotSpeed * (_clockwise ? 1 : -1);
            transform.localEulerAngles += dRot;
            yield return null;
        }
    }
}
