using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeGame : MonoBehaviour
{
    public GameObject _pauseMenu;

    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        if (_button)
        {
            _button.onClick.AddListener(Resume);
        }
    }

    void Resume()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
    }
}
