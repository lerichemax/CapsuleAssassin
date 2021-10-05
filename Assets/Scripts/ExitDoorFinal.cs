using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoorFinal : ExitDoor
{
    public GameObject _finalScreen;

    protected override void DoorAction()
    {
        Time.timeScale = 0f;
        _finalScreen.SetActive(true);
    }
}
