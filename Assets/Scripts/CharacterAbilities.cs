using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbilities : MonoBehaviour
{
    enum Ability
    {
        None,
        Invisible,
        Sprinting
    }

    public Material _characterMaterial;
    public MeshRenderer _characterMeshRenderer;

    public float _sprintMovementSpeed = 100f;

    private CharacterMovement _characterMovement;
    private Energy _energy;

    private float _savedMovementSpeed;

    private Ability _ability;
    public bool IsInvisible
    {
        get { return _ability == Ability.Invisible; }
    }

    void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        if (_characterMovement)
        {
            _savedMovementSpeed = _characterMovement._moveSpeed;
        }

        _energy = GetComponent<Energy>();
        _ability = Ability.None;
    }

    public void InvisibilityTrigger()
    {
        switch (_ability)
        {
            case Ability.None:
                SetInvisibility(true);
                _characterMovement.StopMovement();
                _ability = Ability.Invisible;
                break;
            case Ability.Invisible:
                SetInvisibility(false);
                _ability = Ability.None;
                break;
            case Ability.Sprinting:
                SetSprint(false);
                _characterMovement.StopMovement();
                SetInvisibility(true);
                _ability = Ability.Invisible;
                break;
            default:
                break;
        }
    }

    public void SprintTrigger()
    {
        switch (_ability)
        {
            case Ability.None:
                SetSprint(true);
                _ability = Ability.Sprinting;
                break;
            case Ability.Invisible:
                SetInvisibility(false);
                SetSprint(true);
                _ability = Ability.Sprinting;
                break;
            case Ability.Sprinting:
                SetSprint(false);
                _ability = Ability.None;
                break;
            default:
                break;
        }
    }

    void SetInvisibility(bool isInvisible)
    {
        if (isInvisible)
        {
            if (_characterMaterial && _energy && _energy.HasEnergy())
            {
                _energy.ConsumeEnergy();
                _characterMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                _characterMeshRenderer.material.SetFloat("Vector1_409a7bed54e84372a2509280a073fbbe", 0.25f);
            }
        }
        else
        {
            if (_characterMaterial && _energy)
            {
                _characterMeshRenderer.material.SetFloat("Vector1_409a7bed54e84372a2509280a073fbbe", 1f);
                _characterMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                _energy.StopConsuming();
            }
        }
    }

    void SetSprint(bool isSprinting)
    {
        if (isSprinting)
        {
            if (_characterMovement && _energy && _energy.HasEnergy())
            {
                _energy.ConsumeEnergy();
                _savedMovementSpeed = _characterMovement._moveSpeed;
                _characterMovement._moveSpeed = _sprintMovementSpeed;
            }
        }
        else
        {
            if (_characterMovement && _energy)
            {
                _energy.StopConsuming();
                _characterMovement._moveSpeed = _savedMovementSpeed;
            }
        }
    }

    public void StopAllAbilities()
    {
        SetInvisibility(false);
        SetSprint(false);

        _ability = Ability.None;
    }
}
