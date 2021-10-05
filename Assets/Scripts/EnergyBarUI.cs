using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBarUI : MonoBehaviour
{
    private Energy _energy;

    private Image _image;

    private float _imgMinRValue;
    private float _imgMaxBValue;

    private const float FLASH_TIME = 0.25f;
    private bool _isOff = false;
    private float _flashCounter;

    void Awake()
    {
        _image = GetComponent<Image>();
        _energy = FindObjectOfType<Energy>();
        _imgMaxBValue = _image.color.b/255;
        _imgMinRValue = _image.color.r/255;
    }

    void Update()
    {
        if (_energy && _image)
        {
            Color imgCol = _image.color;

            if (_energy.IsCriticallyRecovering())
            {
                
                _flashCounter += Time.deltaTime;
                if (_isOff)
                {
                    if (_flashCounter >= FLASH_TIME)
                    {
                        _isOff = false;
                        imgCol.a = 1;
                        _flashCounter = 0;
                    }
                }
                else
                {
                    if (_flashCounter >= FLASH_TIME)
                    {
                        _isOff = true;
                        imgCol.a = 0;
                        _flashCounter = 0;
                    }
                }
                
            }
            else
            {
                _isOff = false;
                imgCol.a = 1;
            }

            Vector3 scale = _image.transform.localScale;
            scale.x = _energy.CurrentEnergyPercent;
            _image.transform.localScale = scale;

            float ratio = _energy.CurrentEnergyPercent;

            imgCol.r = 1 - ratio + _imgMinRValue;
            imgCol.b = ratio - _imgMaxBValue;
            _image.color = imgCol;
        }
    }
}
