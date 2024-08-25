using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : Singleton<PlayerController>
{
    [Header("Spaceship")]
    [SerializeField] private Spaceship _spaceshipPrefab;
    [SerializeField] private float _thrustForce;
    [SerializeField] private float _rotationDegrees = 10;
    [SerializeField] private Vector3 _initialPosition;

    [Header("Bullets")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private float _shootingForce;
    
    [Header("Audio")] 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _lasterClip;
    
    private Spaceship _spaceship;

    private void Awake()
    {
        _spaceship = Instantiate(_spaceshipPrefab);
        WarpManager.Instance.SubscribeTransform(_spaceship.transform);
    }
    
    public void InitializePlayer()
    {
        _spaceship.transform.position = _initialPosition;
    }
    
    public void OnPlayerHit()
    {
        // TODO: animate and limit invincibility and input
        InitializePlayer();
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
            // TODO: Implement cooldown
            // TODO: Implement object pooling
            SpawnBullet();

            _audioSource.PlayOneShot(_lasterClip);
        }
    }

    private void SpawnBullet()
    {
        var bullet = Instantiate(_bulletPrefab);
        var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.transform.position = _spaceship.transform.position; 
        bulletRigidbody.AddForce(_spaceship.transform.up * _shootingForce, ForceMode2D.Impulse);
        WarpManager.Instance.SubscribeTransform(bulletRigidbody.transform);
    }

    private void HandleThrust()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            var spaceshipTransform = _spaceship.transform;
            var forceVector = spaceshipTransform.up * (_thrustForce * Time.deltaTime);
            _spaceship.Rigidbody.AddForce(forceVector, ForceMode2D.Force);
        }
    }

    private void HandleRotation()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _spaceship.transform.Rotate(Vector3.forward, -1 * _rotationDegrees * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            _spaceship.transform.Rotate(Vector3.forward, _rotationDegrees * Time.deltaTime);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        WarpManager.Instance.UnsubscribeTransform(_spaceship.transform);
    }
}
