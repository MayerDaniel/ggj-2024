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
    [SerializeField] private Rigidbody2D _originRb;
    [SerializeField] private Rigidbody2D _hookRb;

    [Header("Gameplay")]
    [SerializeField] private float _hookShootSpeed;
    [SerializeField] private float _pullIntensity;
    [SerializeField] private float _accuracy;

    [Header("Game State")]
    [SerializeField] private HookState _state = HookState.Retracted;

    private void Start()
    {
        StartCoroutine(ShootHook());
    }

    private IEnumerator ShootHook()
    {
        if (_state != HookState.Retracted)
            yield break;

        _rope.gameObject.SetActive(true);
        _hook.gameObject.SetActive(true);
        _rope.SetParent(null);
        _hook.SetParent(null);
        _hook.position = _origin.position;

        _state = HookState.Shooting;
        var angle = _origin.transform.localEulerAngles.z * Mathf.Deg2Rad;
        var dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

        _hookRb.velocity = dir * _hookShootSpeed;

        while (true)
        {
            UpdateRope();

            if (_state == HookState.Grabbed)
                break;

            yield return null;
        }
    }

    public void GrabHook()
    {
        if (_state != HookState.Shooting)
            return;

        _state = HookState.Grabbed;
        _hookRb.velocity = Vector2.zero;
        StartCoroutine(ReelIn());
    }

    private IEnumerator ReelIn()
    {
        _state = HookState.ReelingIn;

        float time = 0;
        while (Vector3.Distance(_origin.position, _hook.position) > _accuracy)
        {
            var dir = (_hook.position - _origin.position).normalized;
            var force = time * _pullIntensity * dir;
            _originRb.AddForce(force);
            UpdateRope();

            time += Time.deltaTime;
            yield return null;
        }

        RetractHook();
    }

    private void RetractHook()
    {
        _state = HookState.Retracted;
        _rope.SetParent(_origin);
        _hook.SetParent(_origin);
        _rope.gameObject.SetActive(false);
        _hook.gameObject.SetActive(false);
    }

    private void UpdateRope()
    {
        var dist = Vector3.Distance(_origin.position, _hook.position);
        _rope.localScale = new Vector3(dist, _rope.localScale.y, 1);
        Vector2 ropePos = (_origin.position + _hook.position) / 2f;
        _rope.position = new Vector3(ropePos.x, ropePos.y, _rope.position.z);
        float ropeAngle = Mathf.Atan2(_hook.position.y - _origin.position.y, _hook.position.x - _origin.position.x) * Mathf.Rad2Deg;
        _rope.eulerAngles = Vector3.forward * ropeAngle;
    }
}
