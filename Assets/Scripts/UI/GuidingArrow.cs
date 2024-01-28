using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GuidingArrow : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private Transform _target;
    [SerializeField] private RectTransform _rt;
    [SerializeField] private Image _image;
    [SerializeField] private Vector3 _screenSize = new (1920, 1080, 0);
    [SerializeField] private float _margin;

    private void Start()
    {
        StartCoroutine(MoveArrow());
    }

    private IEnumerator MoveArrow()
    {
        while (true)
        {
            var left = -_screenSize.x / 2f + _margin;
            var right = -left;
            var top = _screenSize.y / 2f - _margin;
            var bottom = -top;

            var targetPos = _cam.WorldToScreenPoint(_target.position) - 0.5f * _screenSize;
            bool onScreen = left <= targetPos.x && targetPos.x <= right 
                && bottom <= targetPos.y && targetPos.y <= top;
            _image.enabled = !onScreen;

            if (!onScreen)
                SetArrowPosition(left, right, top, bottom, targetPos);

            yield return null;
        }
    }

    private void SetArrowPosition(float left, float right, float top, float bottom, Vector2 targetPos)
    {
        float q1 = Mathf.Atan2(top, right);
        float q2 = Mathf.PI - q1;
        float q3 = Mathf.PI + q1;
        float q4 = Mathf.PI + q2;

        float angle = Mathf.Atan2(targetPos.y, targetPos.x);
        _rt.localEulerAngles = angle * Mathf.Rad2Deg * Vector3.forward;

        if (angle < 0)
            angle += (2f * Mathf.PI);

        float x, y;
        if (angle <= q1 || angle >= q4) //right edge
        {
            x = right;
            y = x * Mathf.Tan(angle);
        }
        else if (q2 <= angle && angle <= q3) //left edge
        {
            x = left;
            y = x * Mathf.Tan(angle);
        }
        else if (q1 < angle && angle < q2) //top edge
        {
            y = top;
            x = y / Mathf.Tan(angle);
        }
        else //bottom edge
        {
            y = bottom;
            x = y / Mathf.Tan(angle);
        }
        _rt.localPosition = new Vector3(x, y, 0);
    }
}
