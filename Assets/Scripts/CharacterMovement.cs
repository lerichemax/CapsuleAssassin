using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private CharacterController _characterController;

    public float _moveSpeed;

    void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void MoveZAxis(float value)
    {
        if (_characterController)
        {
            Vector3 fwd = Vector3.forward;
            fwd *= value * _moveSpeed * Time.deltaTime;

            _characterController.Move(fwd);
        }
    }

    public void MoveXAxis(float value)
    {
        if(_characterController)
        {
            Vector3 right = Vector3.right;
            right *= value * _moveSpeed * Time.deltaTime;

            _characterController.Move(right);
        }
    }

    public void StopMovement()
    {
        _characterController.Move(Vector3.zero);
    }
}
