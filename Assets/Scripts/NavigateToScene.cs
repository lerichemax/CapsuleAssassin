using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NavigateToScene : MonoBehaviour
{
    public int _targetSceneIndex;

    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        if (_button)
        {
            _button.onClick.AddListener(Navigate);
        }
    }


    void Navigate()
    {
        SceneManager.LoadScene(_targetSceneIndex);
        Time.timeScale = 1;
    }
}
