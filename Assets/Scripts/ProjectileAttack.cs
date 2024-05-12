using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Vector3 playerLocation;

    private SpriteRenderer _spriteRenderer;
    Rigidbody2D rb;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        StartCoroutine(timer());
    }

    private void Update()
    {
        flipUpdate();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void getPlayerLocation()
    {
        if (_gameManager != null)
        {
            playerLocation = _gameManager._player.transform.position;
        }
    }

    void Movement()
    {
        getPlayerLocation();
        
        transform.position = Vector2.MoveTowards(transform.position, playerLocation, 10f * Time.deltaTime);
    }
    
    void flipUpdate()
    {
        if (rb.velocity.x > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (rb.velocity.x < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
}
