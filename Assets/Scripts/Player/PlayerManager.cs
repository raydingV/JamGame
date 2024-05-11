using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerManager : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rigidbody;
    private Animator _animatorController; 
        
    [SerializeField] private GameObject attackCollider;

    [Header("Values")]
    private float _horizontal;
    private float _vertical;
    [SerializeField] private float _speedPlayer = 5f;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animatorController = GetComponent<Animator>();
    }
    
    void Update()
    {
        attackInput();
        
        animationPlayer();
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

    void attackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(attackCollider, transform.position, Quaternion.identity);
        }
    }

    void animationPlayer()
    {
        _animatorController.SetFloat("Speed", Mathf.Abs(_horizontal + _vertical));
    }
}
