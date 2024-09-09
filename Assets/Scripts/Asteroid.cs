using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    public Rigidbody2D Rigidbody => _rigidbody;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private float _initialThrustForce;
    private int _score;
    private int _level;
    
    public int Score => _score;
    public int Level => _level;
    
    public void InvokeInitialForce()
    {
        _rigidbody.AddForce(Random.insideUnitCircle.normalized * _initialThrustForce, ForceMode2D.Impulse);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            GameManager.Instance.OnBulletAsteroidCollision(other.GetComponent<Bullet>(), this); 
        }        
        
        if (other.CompareTag("Ship"))
        {
             GameManager.Instance.OnAsteroidSpaceshipCollision(other.GetComponent<Spaceship>(), this);
        }
    }

    public void Setup(AsteroidData asteroidData)
    {
        _level = asteroidData.Level;
        _score = asteroidData.Score;
        _initialThrustForce = asteroidData.InitialThrustForce;
        _spriteRenderer.sprite = asteroidData.Sprites[Random.Range(0, asteroidData.Sprites.Count)];
    }
}
