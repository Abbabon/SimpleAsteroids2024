using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : Singleton<GameManager>
{
    private const string HiScore = "HiScore";
    
    [Header("GameLoop")]
    [SerializeField] private int _initialLives = 3;
    private int _currentLives;
    private int _currentScore;
    private int _hiScore;

    [Header("Asteroids")]
    [SerializeField] private List<AsteroidSpawnLocation> _asteroidSpawnLocations;
    [SerializeField] private Asteroid _asteroidPrefab;
    private readonly List<Asteroid> _currentAsteroids = new();

    [SerializeField] private List<AsteroidData> _asteroidDataList; 
    private BestObjectPool<Asteroid> _asteroidPool;
    private int _newAstroidsOnDestroy = 2;

    private void Awake()
    {
        _asteroidPool = new BestObjectPool<Asteroid>(_asteroidPrefab);
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        _hiScore = PlayerPrefs.GetInt(HiScore, 0);
        _currentLives = _initialLives;
        _currentScore = 0;
        
        PlayerController.Instance.InitializePlayerLocation();
        
        CanvasManager.Instance.UpdateLives(_currentLives);
        CanvasManager.Instance.UpdateCurrentScore(_currentScore);
        CanvasManager.Instance.UpdateHiScore(_hiScore);
        
        foreach (var currentAsteroid in _currentAsteroids)
        {
            DestroyAsteroid(currentAsteroid);
        }
        _currentAsteroids.Clear();
        
        foreach (var asteroidSpawnLocation in _asteroidSpawnLocations)
        {
            SpawnAsteroid(asteroidSpawnLocation.transform.position, 1);
        }
    }

    private void SpawnAsteroid(Vector3 asteroidSpawnPosition, int level)
    {
        var newAsteroid = _asteroidPool.Get();
        
        var asteroidData = _asteroidDataList.FirstOrDefault(asteroidData => asteroidData.Level == level);
        
        newAsteroid.Setup(asteroidData);
        newAsteroid.transform.position = asteroidSpawnPosition;
        WarpManager.Instance.SubscribeTransform(newAsteroid.transform);
        _currentAsteroids.Add(newAsteroid);
        
        newAsteroid.InvokeInitialForce();
    }

    public void OnBulletAsteroidCollision(Bullet bullet, Asteroid asteroid)
    {
        DestroyBullet(bullet);
        
        _currentScore += asteroid.Score;
        
        if (_currentScore > _hiScore)
        {
            _hiScore = _currentScore;
            PlayerPrefs.SetInt(HiScore, _hiScore);
            PlayerPrefs.Save();
            
            CanvasManager.Instance.UpdateHiScore(_hiScore);
        }
        
        CanvasManager.Instance.UpdateCurrentScore(_currentScore);
        if (asteroid.Level < _asteroidDataList.Count)
        {
            for (var i = 0; i < _newAstroidsOnDestroy; i++)
            {
                SpawnAsteroid(asteroid.transform.position, asteroid.Level + 1);    
            }
        }
        
        DestroyAsteroid(asteroid);
    }

    private void DestroyBullet(Bullet bullet)
    {
        bullet.StopTimeoutCoroutine();
        PlayerController.Instance.ReturnBullet(bullet);
        WarpManager.Instance.UnsubscribeTransform(bullet.transform);
    }

    private void DestroyAsteroid(Asteroid asteroid)
    {
        _asteroidPool.Release(asteroid);
        WarpManager.Instance.UnsubscribeTransform(asteroid.transform);
        _currentAsteroids.Remove(asteroid);
    }

    public void OnAsteroidSpaceshipCollision(Spaceship spaceship, Asteroid asteroid)
    {
        CanvasManager.Instance.UpdateLives(_currentLives);
        spaceship.Rigidbody.HaltRigidbody();
        _currentLives--;
        
        if (_currentLives <= 0)
        {
            HandleGameOver();
        }
        else
        {
            PlayerController.Instance.OnPlayerHit();
            CanvasManager.Instance.UpdateLives(_currentLives);
        }
    }

    private void HandleGameOver()
    {
        // TODO: start game over screen sequence, then call this:
        StartGame();
    }
}