using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform Transform => _transform; //<----
    private Transform _transform;
    private Rigidbody2D _rigidbody2;

    private Vector2 _inputDirection;

    [SerializeField] private float _velocity = 10;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _rigidbody2 = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
    private void FixedUpdate()
    {
        _rigidbody2.velocity = _inputDirection * _velocity * Time.deltaTime;
    }
}