using StarterAssets;
using UnityEngine;

[RequireComponent(typeof(FirstPersonController))]
[RequireComponent(typeof(SurvivalHandler))]
public class FallDamage : MonoBehaviour
{
    private FirstPersonController _firstPersonController;
    private SurvivalHandler _survivalHandler;

    private int _waterLayer = 4;
    private bool _isLandedWater;
    private float _dropTimer;
    private float _minDropTimer = 1f;
    private float _maxDropTimer = 3f;
    private float _minDamage = 10;
    private float _maxDamage = 100;

    private void Awake()
    {
        _firstPersonController = GetComponent<FirstPersonController>();
        _survivalHandler = GetComponent<SurvivalHandler>();
    }

    private void Update()
    {
        if (_firstPersonController.IsEnable)
        {
            if (!_firstPersonController.IsGrounded)
            {
                _dropTimer += Time.deltaTime;
            }
            else
            {
                if (_dropTimer > 0 && !_isLandedWater)
                {
                    if (_minDropTimer < _dropTimer && _dropTimer < _maxDropTimer)
                    {
                        float normalizedTime = Mathf.InverseLerp(_minDropTimer, _maxDropTimer, _dropTimer);
                        float currentDamage = Mathf.Lerp(_minDamage, _maxDamage, normalizedTime);
                        _survivalHandler.PlayerHealth.Lower(currentDamage);
                    }
                    else if(_dropTimer > _maxDropTimer)
                        _survivalHandler.PlayerHealth.Lower(_maxDamage);
                }

                _dropTimer = 0;
                _isLandedWater = false;
            }
        }
        else
        {
            if(_dropTimer > 0)
                _dropTimer = 0; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == _waterLayer)
            _isLandedWater = true;
        else
            _isLandedWater = false;
    }
}

