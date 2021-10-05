using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    enum MovementStatus
    {
        Idle,
        Walking,
        Rotating,
        InPursuit
    }

    private const float MAX_IDLE_TIME = 1f;

    private NavMeshAgent _agent;

    private Transform _targetPlayer;
    public bool HasTarget
    {
        get { return _targetPlayer != null; }
    }
    private MovementStatus _status;
    private Vector3 _targetPos;
    private float _idleTimeCounter;
    private bool _isOnAlert;

    public bool IsInPursuit
    {
        get { return _status == MovementStatus.InPursuit; }
    }
    
    public float _radius;

    public float _rotationSpeed = 25;
    public float _minRotationTime = 0.5f;
    public float _maxRotationTime= 1f;

    private float _rotationTime;
    private float _rotationTimerCount;
    private bool _isRotatingRight;

    public float _alertMoveSpeed;
    private float _savedMoveSpeed;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _status = MovementStatus.Idle;
        if (_agent)
        {
            _savedMoveSpeed = _agent.speed;
        }
        
    }

    void Update()
    {
        switch (_status)
        {
            case MovementStatus.Idle:
                _idleTimeCounter += Time.deltaTime;
                if (_idleTimeCounter >= MAX_IDLE_TIME)
                {
                    int nextAction = Random.Range(0, 2);
                    if (nextAction == 0)
                    {
                        FindMovementTarget();
                    }
                    else
                    {
                        Rotate();
                    }

                    _idleTimeCounter = 0;
                }
                break;
            case MovementStatus.Walking:
                if ((transform.position - _targetPos).magnitude <= 2f)
                {
                    if (_isOnAlert)
                    {
                        FindMovementTarget();
                    }
                    else
                    {
                        _status = MovementStatus.Idle;
                    }
                    
                }
                break;
            case MovementStatus.Rotating:
                _rotationTimerCount += Time.deltaTime;
                if (_rotationTimerCount > _rotationTime)
                {
                    FindMovementTarget();
                    _rotationTime = 0;
                    _agent.isStopped = false;
                    break;
                }
                Rotate();
                break;
            case MovementStatus.InPursuit:
                _agent.SetDestination(_targetPlayer.position);
                Vector3 dir = _targetPlayer.transform.position - transform.position;
                if (dir.magnitude < _agent.stoppingDistance)
                {
                    Quaternion rot = Quaternion.LookRotation(dir.normalized);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * _rotationSpeed);
                }
                break;
        }
    }

    private void FindMovementTarget()
    {
        Vector3 dir = Random.insideUnitSphere * _radius;
        dir.y = 0;

        Vector3 pos = transform.position + dir;

        NavMeshHit hit;
        NavMesh.SamplePosition(pos, out hit, _radius, NavMesh.AllAreas);

        _targetPos = hit.position;
        _agent.SetDestination(hit.position);
        _status = MovementStatus.Walking;
    }

    public void SpotTarget(Transform target)
    {
        _targetPlayer = target;
        if (_status == MovementStatus.Idle)
        {
            _idleTimeCounter = 0;
        }
        _status = MovementStatus.InPursuit;
        _agent.stoppingDistance = 15;
    }

    public void StopPursuit()
    {
        _targetPlayer = null;
        _status = MovementStatus.Idle;
        _agent.stoppingDistance = 0;
    }

    void Rotate()
    {
        if (_rotationTime == 0)
        {
            _rotationTime = Random.Range(_minRotationTime, _maxRotationTime);
            int right = Random.Range(0, 2);
            _isRotatingRight = right == 0 ? false : true;
            _status = MovementStatus.Rotating;
            _agent.isStopped = true;
        }

        if (_isRotatingRight)
        {
            transform.rotation = Quaternion.Euler(0, _rotationSpeed * Time.fixedDeltaTime, 0) * transform.rotation;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, -_rotationSpeed * Time.fixedDeltaTime, 0) * transform.rotation;
        }
    }

    public void SetAlert(bool isOnAlert)
    {
        if (!_isOnAlert && isOnAlert)
        {
            _agent.speed = _alertMoveSpeed;
            _radius *= 2;
            _agent.acceleration = _alertMoveSpeed;
        }
        else if(!isOnAlert)
        {
            _agent.speed = _savedMoveSpeed;
            _radius /= 2;
            _agent.acceleration = 8;
        }
        _isOnAlert = isOnAlert;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            SpotTarget(collision.transform);
            transform.Find("Gun").GetComponent<Gunner>().SpotTarget(collision.gameObject);
            CharacterAbilities abilities = collision.gameObject.GetComponent<CharacterAbilities>();
            if (abilities.IsInvisible)
            {
                abilities.InvisibilityTrigger();
            }
        }
    }
}
