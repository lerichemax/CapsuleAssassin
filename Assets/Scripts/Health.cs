using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Health : MonoBehaviour
{
    private const float MAX_HEALTH = 50f;
    private const float VIGNETTE_MAX_STRENGTH = .4f;

    public float _healthRegenarationRate = 4f;
    public GameObject _gameOverScreen;
    public Volume _volume;
    private Vignette _vignette;
    private ParticleSystem _particles;

    private float _health;





    void Awake()
    {
        _health = MAX_HEALTH;
        if (_gameOverScreen)
        {
            _gameOverScreen.SetActive(false);
        }

        if (_volume)
        {
            _volume.profile.TryGet<Vignette>(out _vignette);
            _vignette.intensity.value = 0;
        }

        _particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (_health < MAX_HEALTH)
        {
            _health += _healthRegenarationRate * Time.deltaTime;
            Mathf.Clamp(_health, 0, MAX_HEALTH);
        }
        
        if (_vignette)
        {
            _vignette.intensity.value = VIGNETTE_MAX_STRENGTH - (_health/MAX_HEALTH) * VIGNETTE_MAX_STRENGTH;
        }
        
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;

        if (_particles)
        {
            _particles.Play();
        }
        
        if (_health < 0)
        {
            Time.timeScale = 0;
            if (_gameOverScreen)
            {
                _gameOverScreen.SetActive(true);
            }
            
        }
    }
}
