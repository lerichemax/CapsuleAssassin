using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ExitDoor : MonoBehaviour
{

    public Material _doorMaterial;

    private BoxCollider _collider;
    //private DoorArrowPointer _doorArrowPointer;

    private void Awake()
    {
        //_doorArrowPointer = FindObjectOfType<DoorArrowPointer>();
        _collider = GetComponent<BoxCollider>();
        if (_collider)
        {
            _collider.enabled = false;
        } 
    }
    private void Start()
    {
        CloseDoor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DoorAction();
           
        }
    }

    public void OpenDoor()
    {
        if (_collider)
        {
            _collider.enabled = true;
        }

        if (_doorMaterial)
        {

            _doorMaterial.SetColor("_BaseColor", Color.green);
        }

        //_doorArrowPointer.ShowArrow = true;
    }

    public void CloseDoor()
    {
        if (_collider)
        {
            _collider.enabled = false;
        }

        if (_doorMaterial)
        {
            _doorMaterial.SetColor("_BaseColor", Color.black);
        }
        //_doorArrowPointer.ShowArrow = false;
    }

    protected abstract void DoorAction();
}
