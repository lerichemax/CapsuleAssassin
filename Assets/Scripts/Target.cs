using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : MonoBehaviour
{
    public GameObject _bloodSplatter;

    private ExitDoor _exitDoor;
    private CapsuleCollider _collider;
    private EnemiesManager _manager;
    void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _manager = FindObjectOfType<EnemiesManager>();
        _exitDoor = FindObjectOfType<ExitDoor>();
    }

    public void GetKilled()
    {
        transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, 0);
        if (_manager)
        {
            if (_exitDoor && !_manager.IsOnAlert)
            {
                _exitDoor.OpenDoor();
            }
        }
        else
        {
            if (_exitDoor)
            {
                _exitDoor.OpenDoor();
            }
        }
        

        if (_bloodSplatter)
        {
            int rot = Random.Range(0, 361);
            Instantiate(_bloodSplatter, new Vector3(transform.position.x, 0.1f, transform.position.z), Quaternion.Euler(0, rot, 0));
        }
        enabled = false;
        WanderAroundSpawn wander = GetComponent<WanderAroundSpawn>();
        wander.StopAllCoroutines();
        wander.enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
    }
    
}
