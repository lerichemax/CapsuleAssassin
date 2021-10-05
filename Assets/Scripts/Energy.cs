using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    enum EnergyStatus
    {
        Normal,
        Critical,
        Replenishing,
        CriticalReplenishing
    }

    const float MAX_ENERGY = 100f;

    public float _consuptionRate = 2f;
    public float _replenishmentRate = 2f;
    public float _depletedReplenishmentRate = 1f;

    private EnergyStatus _status;
    private CharacterAbilities _abilities;

    private bool _isDepleting;

    private float _currentEnergy;
    public float CurrentEnergyPercent
    {
        get { return _currentEnergy / MAX_ENERGY; }
    }


    void Awake()
    {
        _currentEnergy = MAX_ENERGY;
        _abilities = GetComponent<CharacterAbilities>();
    }

    void Update()
    {
        if (_isDepleting)
        {
            _currentEnergy -= _consuptionRate * Time.deltaTime;
            if (_currentEnergy <= 0.15f * MAX_ENERGY)
            {
                _status = EnergyStatus.Critical;
            }

            if (_currentEnergy <= 0f)
            {
                _status = EnergyStatus.CriticalReplenishing;
                _isDepleting = false;
                _abilities.StopAllAbilities();
            }
        }

        switch (_status)
        {
            case EnergyStatus.Replenishing:
                _currentEnergy += _replenishmentRate * Time.deltaTime;
                if (_currentEnergy >= MAX_ENERGY)
                {
                    _currentEnergy = MAX_ENERGY;
                    _status = EnergyStatus.Normal;
                }
                break;
            case EnergyStatus.CriticalReplenishing:
                _currentEnergy += _depletedReplenishmentRate * Time.deltaTime;
                if (_currentEnergy >= 0.5f * MAX_ENERGY)
                {
                    _status = EnergyStatus.Replenishing;
                }
                break;
            default:
                break;
        }
    }

    public bool HasEnergy()
    {
        return _status != EnergyStatus.Critical && _status != EnergyStatus.CriticalReplenishing;
    }

    public void ConsumeEnergy()
    {
        if (HasEnergy())
        {
            _isDepleting = true;
            _status = EnergyStatus.Normal;
        }
    }

    public void StopConsuming()
    {
        _isDepleting = false;

        switch (_status)
        {
            case EnergyStatus.Normal:
                _status = EnergyStatus.Replenishing;
                break;
            case EnergyStatus.Critical:
                _status = EnergyStatus.CriticalReplenishing;
                break;
        }
    }

    public bool IsCriticallyRecovering()
    {
        return _status == EnergyStatus.CriticalReplenishing;
    }
}
