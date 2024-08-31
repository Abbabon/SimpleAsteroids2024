using System;
using System.Collections;
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
    [SerializeField] private float _fireCooldown = 0.2f;
    
    [Header("Bullets")]
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _shootingForce;
    [SerializeField] private float _bulletTimeout = 0.5f;
    
    [Header("Audio")] 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _lasterClip;
    
    private Spaceship _spaceship;
    private bool _isFlying;
    private bool _isOnShootingTimeout;
    public bool isFlying => _isFlying;

    private BestObjectPool<Bullet> _bulletPool;

    private void Awake()
    {
        _bulletPool = new BestObjectPool<Bullet>(_bulletPrefab);
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
            if (_isOnShootingTimeout)
                return;
            SpawnBullet();
            StartCoroutine(ShootTimeout());
            _audioSource.PlayOneShot(_lasterClip);
        }
    }
    
    private IEnumerator ShootTimeout()
    {
        _isOnShootingTimeout = true;
        yield return new WaitForSeconds(_fireCooldown);
        _isOnShootingTimeout = false;
    }

    private void SpawnBullet()
    {
        var bullet = _bulletPool.Get();
        
        var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.transform.position = _spaceship.transform.position; 
        bulletRigidbody.AddForce(_spaceship.transform.up * _shootingForce, ForceMode2D.Impulse);
        WarpManager.Instance.SubscribeTransform(bulletRigidbody.transform);
        
        StartCoroutine(BulletTimeout(bullet));
    }

    private IEnumerator BulletTimeout(Bullet bullet)
    {
        yield return new WaitForSeconds(_bulletTimeout);
        GameManager.Instance.DestroyBullet(bullet);
    }

    private void HandleThrust()
    {
        _isFlying = Input.GetKey(KeyCode.UpArrow);
        if (isFlying)
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

    public void ReturnBullet(Bullet bullet)
    {
        bullet.GetComponent<Rigidbody2D>()?.HaltRigidbody();   
        _bulletPool.Release(bullet);
    }
}
