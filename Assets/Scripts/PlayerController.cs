using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private Rigidbody2D _spaceshipRigidbody;
    [SerializeField] private float _thrustForce;
    [SerializeField] private float _rotationDegrees = 10;

    [Header("Bullets")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _shootingForce;
    
    [Header("Audio")] 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _lasterClip;
    
    private float _rotationDelta;

    private void Update()
    {
        WarpManager.Instance.KeepInBounds(_spaceshipRigidbody.transform);
        HandleThrust();
        HandleRotation();
        HandleBullets();
    }

    private void HandleBullets()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var bullet = Instantiate(_bulletPrefab);
            var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidbody.transform.position = _spaceshipRigidbody.transform.position; 
            bulletRigidbody.AddForce(_spaceshipRigidbody.transform.up * _shootingForce, ForceMode2D.Impulse);
            
            _audioSource.PlayOneShot(_lasterClip);
        }
    }

    private void HandleThrust()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            var spaceshipTransform = _spaceshipRigidbody.transform;
            var forceVector = spaceshipTransform.up * (_thrustForce * Time.deltaTime);
            _spaceshipRigidbody.AddForce(forceVector, ForceMode2D.Force);
        }
    }
    
    private void HandleRotation()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _rotationDelta = -1 * _rotationDegrees * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            _rotationDelta = _rotationDegrees * Time.deltaTime;
        }
        else
        {
            _rotationDelta = 0;
        }
    }

    private void FixedUpdate()
    {
        _spaceshipRigidbody.MoveRotation(_spaceshipRigidbody.rotation + _rotationDelta);
    }
}
