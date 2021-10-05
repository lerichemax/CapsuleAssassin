using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        if (_button)
        {
            _button.onClick.AddListener(Quit);
        }

    }

    void Quit()
    {
        Application.Quit();
    }
}
