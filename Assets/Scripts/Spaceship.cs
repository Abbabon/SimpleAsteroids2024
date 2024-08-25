using System;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    
    public Rigidbody2D Rigidbody => _rigidbody;
    
    private void Update()
    {
        // TODO: ANIMATION?
    }
}