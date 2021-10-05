using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertBar : MonoBehaviour
{
    private EnemiesManager _manager;

    private Image _img;

    void Awake()
    {
        _img = GetComponent<Image>();
        _manager = FindObjectOfType<EnemiesManager>();
    }

    void Update()
    {
        Vector3 scale = _img.transform.localScale;
        scale.x = _manager.AlertTimePercentage;
        _img.transform.localScale = scale;
    }
}
