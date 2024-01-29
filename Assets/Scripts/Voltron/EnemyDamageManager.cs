using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _turnTheseRed;
    [SerializeField] private float _stunTime;
    [SerializeField] private GrapplingHook _grapplingHook;

    private bool _currentlyHit = false;
    private float _timeUntilBetter = 3;

    public void OnGetHit()
    {
        _timeUntilBetter = _stunTime;
        if (!_currentlyHit)
            StartCoroutine(Stun());
    }

    private IEnumerator Stun()
    {
        foreach (var renderer in _turnTheseRed)
            SetColor(renderer, Color.red);

        _grapplingHook.RetractHook();

        while (_timeUntilBetter > 0)
        {
            _timeUntilBetter -= Time.deltaTime;
            yield return null;
        }

        foreach (var renderer in _turnTheseRed)
            SetColor(renderer, Color.white);
    }    

    private void SetColor(SpriteRenderer renderer, Color color)
    {
        renderer.color = color;
    }
}
