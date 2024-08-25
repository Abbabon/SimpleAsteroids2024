using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    public Rigidbody2D Rigidbody => _rigidbody;

    [SerializeField] private float _initialThrustForce;
    [SerializeField] private int _score;
    public int Score => _score;
    
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
}
