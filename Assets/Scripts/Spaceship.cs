using System;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Collider2D _collider;
    
    private static readonly int Fly = Animator.StringToHash(nameof(Fly));
    private static readonly int Cooldown = Animator.StringToHash(nameof(Cooldown));

    public Rigidbody2D Rigidbody => _rigidbody;
    
    private void Update()
    {
        _animator.SetBool(Fly, PlayerController.Instance.IsFlying);
    }

    public void TurnCooldownMode(bool onCooldown)
    {
        _animator.SetBool(Cooldown, onCooldown);
        _collider.enabled = !onCooldown;
    }
    
}