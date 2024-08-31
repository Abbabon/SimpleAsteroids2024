using System;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    
    private static readonly int Fly = Animator.StringToHash("Fly");

    public Rigidbody2D Rigidbody => _rigidbody;
    
    private void Update()
    {
        _animator.SetBool(Fly, PlayerController.Instance.isFlying);
    }
}