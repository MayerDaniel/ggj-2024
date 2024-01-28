using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;

    [Header("Gameplay")]
    private Transform _target;
    [SerializeField] private float _thrustFrequency = 2f;
    [SerializeField] private float _thrustAmount = 200f;
    [SerializeField] private float _linearDrag = 0.65f;
    [SerializeField] private float _minVariance = 0.5f;
    [SerializeField] private float _maxVariance = 1.5f;

    private float RandomVariance => Random.Range(_minVariance, _maxVariance);

    private void Start()
    {
        _target = FindObjectOfType<TestPlayer>().transform;
        _thrustFrequency *= RandomVariance;
        _thrustAmount *= RandomVariance;
        _linearDrag *= RandomVariance;
        _rb.drag = _linearDrag;

        StartCoroutine(Chase());
    }

    private IEnumerator Chase()
    {
        while (true)
        {
            Thrust();
            yield return new WaitForSeconds(_thrustFrequency);
        }
    }

    private void Thrust()
    {
        var dir = (_target.position - transform.position).normalized;
        _rb.AddForce(_thrustAmount * dir);
    }
}