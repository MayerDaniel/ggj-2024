using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public enum HookState { Retracted, Shooting, Grabbed, ReelingIn };

    [Header("Hook Parts")]
    [SerializeField] private Transform _origin;
    [SerializeField] private Transform _rope;
    [SerializeField] private Transform _hook;
    [SerializeField] private Rigidbody2D _hookRb;

    [Header("Gameplay")]
    [SerializeField] private float _hookShootSpeed;

    [Header("Game State")]
    private HookState _state = HookState.Retracted;

    private void Start()
    {
        StartCoroutine(ShootHook());
    }

    private IEnumerator ShootHook()
    {
        if (_state != HookState.Retracted)
            yield break;

        _state = HookState.Shooting;
        var angle = _origin.transform.localEulerAngles.z * Mathf.Deg2Rad;
        var dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

        _hookRb.velocity = dir * _hookShootSpeed;

        while (_state == HookState.Shooting)
        {
            var dist = Vector3.Distance(_origin.position, _hook.position);
            _rope.localScale = new Vector3(dist, _rope.localScale.y, 1);
            Vector2 ropePos = (_origin.position + _hook.position) / 2f;
            _rope.position = new Vector3(ropePos.x, ropePos.y, _rope.position.z);
            float ropeAngle = Mathf.Atan2(_hook.position.y - _origin.position.y, _hook.position.x - _origin.position.x) * Mathf.Rad2Deg;
            _rope.eulerAngles = Vector3.forward * ropeAngle;
            yield return null;
        }
    }

    private void GrabHook()
    {
        _state = HookState.Grabbed;
        _hookRb.velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GrabHook();
    }
}
