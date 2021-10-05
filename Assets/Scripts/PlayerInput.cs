using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private CharacterMovement _movement;
    private CharacterAbilities _abilities;
    public GameObject _pauseMenu;

    private bool _isPaused;
    private bool _stopMovementInput;

    private void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
        _abilities = GetComponent<CharacterAbilities>();
        _stopMovementInput = false;
        _isPaused = false;
    }

    void Update()
    {
        if (_movement)
        {
            if (!_stopMovementInput)
            {
                _movement.MoveZAxis(Input.GetAxis("Vertical"));
                _movement.MoveXAxis(Input.GetAxis("Horizontal"));
            }
            else if(Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            {
                _stopMovementInput = false;
            }

            if(_abilities.IsInvisible && (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) && !_stopMovementInput)
            {
                _abilities.InvisibilityTrigger();
            }

        }

        if (_abilities)
        {
            if (Input.GetButtonUp("Sprint"))
            {
                _abilities.SprintTrigger();
                _stopMovementInput = false;
            }

            if (Input.GetButtonUp("Invisibility"))
            {
                _abilities.InvisibilityTrigger();
                _stopMovementInput = _abilities.IsInvisible;
            }
        }

        if (_pauseMenu && Input.GetButtonUp("Pause"))
        {
            if (_isPaused)
            {
                Time.timeScale = 1;
                _pauseMenu.SetActive(false);
            }            
            else
            {
                Time.timeScale = 0;
                _pauseMenu.SetActive(true);
            }
        }
    }
}
