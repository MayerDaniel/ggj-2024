using System.Collections;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public enum HookState { Retracted, Shooting, Grabbed, ReelingIn };

    [Header("Robot Parts References")]
    [SerializeField] private Transform _wrist;
    [SerializeField] private Transform _elbow;

    [Header("Hook Parts")]
    [SerializeField] private Transform _origin;
    [SerializeField] private Transform _rope;
    [SerializeField] private Transform _hook;
    [SerializeField] private Rigidbody2D _pullThis;
    [SerializeField] private Rigidbody2D _hookRb;

    [Header("Gameplay")]
    [SerializeField] private float _hookShootSpeed;
    [SerializeField] private float _pullIntensity;
    [SerializeField] private float _accuracy;

    [Header("Game State")]
    private HookState _state = HookState.Retracted;

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            OnFirePressed();
        }
    }
#endif

    public void OnFirePressed()
    {
        if (_state == HookState.Retracted)
            ShootHook();
        else
            RetractHook();
    }

    public void ShootHook()
    {
        if (_state == HookState.Retracted)
            StartCoroutine(ShootHook_Co());
    }

    private IEnumerator ShootHook_Co()
    {
        _rope.gameObject.SetActive(true);
        _hook.gameObject.SetActive(true);
        _rope.SetParent(null);
        _hook.SetParent(null);
        _hook.position = _origin.position;

        _state = HookState.Shooting;
        var dir = (_wrist.position - _elbow.position).normalized;

        _hookRb.velocity = dir * _hookShootSpeed;

        while (_state == HookState.Shooting)
        {
            UpdateRope();
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
        float startDist = Vector3.Distance(_origin.position, _hook.position);

        while (Vector3.Distance(_origin.position, _hook.position) > _accuracy && _state == HookState.ReelingIn)
        {
            float prog = Vector3.Distance(_origin.position, _hook.position) / startDist;
            var dir = (_hook.position - _origin.position).normalized;
            var force = time * _pullIntensity * prog * dir;
            _pullThis.AddForce(force);
            UpdateRope();

            time += Time.deltaTime;
            yield return null;
        }

        RetractHook();
    }

    public void RetractHook()
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
