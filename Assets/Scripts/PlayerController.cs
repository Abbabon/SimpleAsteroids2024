using System;
using Unity.VisualScripting;
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

    private void Awake()
    {
        WarpManager.Instance.SubscribeTransform(_spaceshipRigidbody.transform);
    }

    private void Update()
    {
        HandleThrust();
        HandleRotation();
        HandleBullets();
    }

    private void HandleBullets()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBullet();

            _audioSource.PlayOneShot(_lasterClip);
        }
    }

    private void SpawnBullet()
    {
        var bullet = Instantiate(_bulletPrefab);
        var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.transform.position = _spaceshipRigidbody.transform.position; 
        bulletRigidbody.AddForce(_spaceshipRigidbody.transform.up * _shootingForce, ForceMode2D.Impulse);
        WarpManager.Instance.SubscribeTransform(bulletRigidbody.transform);
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
            _spaceshipRigidbody.transform.Rotate(Vector3.forward, -1 * _rotationDegrees * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            _spaceshipRigidbody.transform.Rotate(Vector3.forward, _rotationDegrees * Time.deltaTime);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        WarpManager.Instance.UnsubscribeTransform(_spaceshipRigidbody.transform);
    }
}
