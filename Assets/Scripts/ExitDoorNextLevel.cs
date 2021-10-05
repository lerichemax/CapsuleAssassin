using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
public class ExitDoorNextLevel : ExitDoor
{

    public int _nextSceneIndex;

    protected override void DoorAction()
    {
        SceneManager.LoadScene(_nextSceneIndex);
    }
}
