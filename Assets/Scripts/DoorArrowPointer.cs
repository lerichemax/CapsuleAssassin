using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorArrowPointer : MonoBehaviour
{
    public Transform _target;
    public Transform _player;

    private RectTransform _pointerRectTransform;
    private Transform _arrow;

    private bool _showArrow;
    public bool ShowArrow
    {
        set {
            _showArrow = value;
            _arrow.gameObject.SetActive(value); 
        }
    }

    private void Awake()
    { 
        _arrow = transform.Find("Arrow");
        _pointerRectTransform = _arrow.GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 toPosition = _target.position;
        Vector3 fromPosition = _player.position;
        Vector3 dir = (toPosition - fromPosition).normalized;
        
        float angle = Vector3.Angle(Vector3.forward, dir);
        angle += angle < 0 ? 360 : 0;
        _pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);

        float offset = 100f;
        Vector3 arrowPositionScreen = Camera.main.worldToCameraMatrix.MultiplyPoint(
            (_target.position - _player.position).normalized * offset);

        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(_target.position);
        bool isOffScreen = targetPositionScreenPoint.x <= 0 ||
            targetPositionScreenPoint.x >= Screen.width ||
            targetPositionScreenPoint.y <= 0 ||
            targetPositionScreenPoint.y >= Screen.height;

        if (isOffScreen && _showArrow)
        {
            _arrow.gameObject.SetActive(true);
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0f);

            _pointerRectTransform.position = arrowPositionScreen + screenCenter;
            _pointerRectTransform.localPosition =
                new Vector3(_pointerRectTransform.localPosition.x, _pointerRectTransform.localPosition.y, 0);
        }
        else
        {
            _arrow.gameObject.SetActive(false);
        }
    }
}