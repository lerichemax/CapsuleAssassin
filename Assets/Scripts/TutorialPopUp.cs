using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopUp : MonoBehaviour
{

    void Awake()
    {
        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }
}
