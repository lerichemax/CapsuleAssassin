using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartBtn : MonoBehaviour
{

    private Button _btn;

    void Awake()
    {
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(RestartScene);
    }

    void RestartScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
