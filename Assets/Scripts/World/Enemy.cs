using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState { Attacking, Retreating, Dead };

    [Header("Components")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioSource _audioSource;

    [Header("Sprites")]
    [SerializeField] private Sprite _sadSprite;
    [SerializeField] private Sprite _deadSprite;

    [Header("Gameplay")]
    private Transform _target;
    [SerializeField] private float _thrustFrequency = 2f;
    [SerializeField] private float _thrustAmount = 200f;
    [SerializeField] private float _linearDrag = 0.65f;
    [SerializeField] private float _minVariance = 0.5f;
    [SerializeField] private float _maxVariance = 1.5f;
    private bool _isRetreating;
    private EnemyState _state = EnemyState.Attacking;

    private float RandomVariance => Random.Range(_minVariance, _maxVariance);

    public void Assign(Transform target)
    {
        _target = target;
        _thrustFrequency *= RandomVariance;
        _thrustAmount *= RandomVariance;
        _linearDrag *= RandomVariance;
        _rb.drag = _linearDrag;

        StartCoroutine(Chase());
    }

    private IEnumerator Chase()
    {
        while (_state != EnemyState.Dead)
        {
            Thrust();
            yield return new WaitForSeconds(_thrustFrequency);
        }
    }

    private void Thrust()
    {
        var dir = (_isRetreating ? transform.position - _target.position : 
            _target.position - transform.position).normalized;
        _rb.AddForce(_thrustAmount * dir);
        _audioSource.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (_state == EnemyState.Attacking &&
            collision.gameObject.transform.parent != null && 
            collision.gameObject.transform.parent.name.Contains("Robot"))
        {
            _target.GetComponent<EnemyDamageManager>().OnGetHit();
            _isRetreating = true;
        }
    }

    public void OnGetHit()
    {
        switch (_state)
        {
            case EnemyState.Attacking:
                _state = EnemyState.Retreating;
                _spriteRenderer.sprite = _sadSprite;
                _isRetreating = true;
                break;
            case EnemyState.Retreating:
                _state = EnemyState.Dead;
                _spriteRenderer.sprite = _deadSprite;
                break;
        }
    }
}