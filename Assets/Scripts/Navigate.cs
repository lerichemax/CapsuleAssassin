using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navigate : MonoBehaviour
{
    public GameObject _target;
    public GameObject _currentMenu;

    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        if (_button)
        {
            _button.onClick.AddListener(NavigateToTarget);
        }

    }

    void NavigateToTarget()
    {
        _target.SetActive(true);
        _currentMenu.SetActive(false);
    }
}
