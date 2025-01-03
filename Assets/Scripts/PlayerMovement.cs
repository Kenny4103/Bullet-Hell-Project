using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    private Vector2 _movement;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Only allow movement if gameplay is active
        if (GameManager.Instance != null && GameManager.Instance.isGameplayActive)
        {
            _movement.Set(InputManager.Movement.x, InputManager.Movement.y);
        }
        else
        {
            // Stop movement if gameplay is not active
            _movement = Vector2.zero;
        }

        _rb.velocity = _movement * _moveSpeed;
    }
}

