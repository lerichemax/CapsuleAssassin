using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public EnemyMovement[] _enemies;
    private ExitDoor _door;
    private AudioSource _sound;
    private Target _target;
    private bool _onAlert = false;
    private bool _onMaximumAlert = false;
    private bool _countDownDone = false;
    public bool IsOnMaximumAlert
    {
        get { return _onMaximumAlert; }
    }

    public bool IsOnAlert
    {
        get { return _onAlert || _onMaximumAlert; }
    }

    public float _maxAlertTime = 10f;
    private float _alertCountDown;

    public float AlertTimePercentage
    {
        get { return _alertCountDown / _maxAlertTime; }
    }

    private void Awake()
    {
        _alertCountDown = _maxAlertTime;
        _sound = GetComponent<AudioSource>();
        _door = FindObjectOfType<ExitDoor>();
        _target = FindObjectOfType<Target>();
    }

    void Update()
    {
        bool targetInSight = false;
        foreach(EnemyMovement enemy in _enemies)
        {
            if (enemy.gameObject.activeInHierarchy && enemy.HasTarget)
            {
                targetInSight = true;
                break;
            }
        }

        if ((_onAlert || (_onMaximumAlert && !_countDownDone)) && !targetInSight)
        {
            _alertCountDown -= Time.deltaTime;
            if (_alertCountDown <= 0f)
            {
                if (_onAlert)
                {
                    foreach (EnemyMovement enemy in _enemies)
                    {
                        if (enemy.gameObject.activeInHierarchy)
                        {
                            enemy.SetAlert(false);
                        }
                    }
                    if (!_target.enabled)
                    {
                        _door.OpenDoor();
                    }
                    
                    _onAlert = false;
                }
                else if (_onMaximumAlert)
                {
                    _countDownDone = true;
                    _door.OpenDoor();
                }
                _alertCountDown = _maxAlertTime;
            }
        }
    }

    public void GetAlerted(bool isMaximumAlert = false)
    {
        if ((_onAlert && !isMaximumAlert) || (!_onAlert && isMaximumAlert))
        {
            _alertCountDown = _maxAlertTime;
            return;
        }
        else if (_onMaximumAlert)
        {
            return;
        }
        _sound.Play();
        _onMaximumAlert = isMaximumAlert;
        _onAlert = !isMaximumAlert;

        if (_door)
        {
            _door.CloseDoor();
        }
        
        foreach (EnemyMovement enemy in _enemies)
        {
            enemy.SetAlert(true);
        }
    }
}
