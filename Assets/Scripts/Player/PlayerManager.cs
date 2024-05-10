using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    
    [Header("Values")]
    private float _horizontal;
    private float _vertical;
    [SerializeField] private float _speedPlayer = 5f;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        playerInput();
    }

    void playerInput()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        _rigidbody.velocity = new Vector3(_horizontal * _speedPlayer, _vertical * _speedPlayer);
    }
}
