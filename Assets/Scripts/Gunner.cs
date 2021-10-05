using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : MonoBehaviour
{
    private const float SHOT_COOLDOWN = 0.25f;

    private GameObject _target;
    private GameObject _parent;
    private AudioSource _sound;

    public ParticleSystem _muzzleFlash;

    private float _range = 20f;

    public float _damage = 10f;

    private float _shotCooldownTimer = 0f;
    private bool _hasShot;

    public bool HasTarget
    {
        get { return _target != null; }
    }
    void Awake()
    {
        _parent = transform.root.gameObject;
        _sound = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        if (_target && _parent && !_hasShot)
        {
            if ((_parent.transform.position - _target.transform.position).magnitude <= _range)
            {
                Shoot();
            }
        }
        else if (_hasShot)
        {
            _shotCooldownTimer += Time.deltaTime;
            if (_shotCooldownTimer >= SHOT_COOLDOWN)
            {
                _hasShot = false;
                _shotCooldownTimer = 0;
            }
        }
    }

    public void SpotTarget(GameObject target)
    {
        _target = target;
    }

    public void LoseTarget()
    {
        _target = null;
    }

    void Shoot()
    {
        if (!_target)
        {
            return;
        }

        if (_sound)
        {
            _sound.Play();
        }
        
        if (_muzzleFlash)
        {
            _muzzleFlash.Play();
        }

        Health health = _target.GetComponent<Health>();
        if (health)
        {
            health.TakeDamage(_damage);
        }
        _hasShot = true;
    }
}
