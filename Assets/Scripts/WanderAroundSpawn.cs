using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderAroundSpawn : MonoBehaviour
{
    public float _radius;

    private NavMeshAgent _agent;

    private Vector3 _center;
    private Vector3 _destination = new Vector3();

    void Awake()
    {
        _center = transform.position;
        _agent = GetComponent<NavMeshAgent>();
        SetNextDestination();
    }

    void Update()
    {
        if ((_destination - transform.position).magnitude < 1f)
        {
            StartCoroutine("WaitAndWalk");
        }
    }

    void SetNextDestination()
    {
        Vector3 dir = Random.insideUnitSphere * _radius;
        dir += _center;
        NavMeshHit hit;
        NavMesh.SamplePosition(dir, out hit, _radius, 1);

        if (_agent)
        {
            _agent.SetDestination(hit.position);
        }

        _destination = hit.position;
    }

    IEnumerator WaitAndWalk()
    {
        yield return new WaitForSeconds(1.5f);
        SetNextDestination();
    }
}
