using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            _rb.velocity += Vector2.up * _speed;
        if (Input.GetKey(KeyCode.A))
            _rb.velocity += Vector2.left * _speed;
        if (Input.GetKey(KeyCode.S))
            _rb.velocity += Vector2.down * _speed;
        if (Input.GetKey(KeyCode.D))
            _rb.velocity += Vector2.right * _speed;
    }
}
