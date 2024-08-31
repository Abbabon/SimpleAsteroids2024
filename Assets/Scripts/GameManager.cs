using System;
using System.Collections.Generic;
using UnityEngine;

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

        // TODO: pool asteroids?
        foreach (var currentAsteroid in _currentAsteroids)
        {
            DestroyAsteroid(currentAsteroid, false);
        }
        _currentAsteroids.Clear();
        
        foreach (var asteroidSpawnLocation in _asteroidSpawnLocations)
        {
            SpawnAsteroid(asteroidSpawnLocation);
        }
    }

    private void SpawnAsteroid(AsteroidSpawnLocation asteroidSpawnLocation)
    {
        // TODO: pool asteroids?
        var newAsteroid = Instantiate(_asteroidPrefab);
        newAsteroid.transform.position = asteroidSpawnLocation.transform.position;
        WarpManager.Instance.SubscribeTransform(newAsteroid.transform);
        newAsteroid.InvokeInitialForce();
        _currentAsteroids.Add(newAsteroid);
    }

    public void OnBulletAsteroidCollision(Bullet bullet, Asteroid asteroid)
    {
        DestroyBullet(bullet);
        DestroyAsteroid(asteroid);

        _currentScore += asteroid.Score;
        
        if (_currentScore > _hiScore)
        {
            _hiScore = _currentScore;
            PlayerPrefs.SetInt(HiScore, _hiScore);
            PlayerPrefs.Save();
            
            CanvasManager.Instance.UpdateHiScore(_hiScore);
        }
        
        CanvasManager.Instance.UpdateCurrentScore(_currentScore);
    }

    private void DestroyBullet(Bullet bullet)
    {
        bullet.StopTimeoutCoroutine();
        PlayerController.Instance.ReturnBullet(bullet);
        WarpManager.Instance.UnsubscribeTransform(bullet.transform);
    }

    private void DestroyAsteroid(Asteroid asteroid, bool removeFromList = true)
    {
        // TODO: pool asteroids
        Destroy(asteroid.gameObject);
        WarpManager.Instance.UnsubscribeTransform(asteroid.transform);
        if (removeFromList)
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