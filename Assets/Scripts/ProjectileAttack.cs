using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    [SerializeField] private bool player = false;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Vector3 _destination;

    private SpriteRenderer _spriteRenderer;
    Rigidbody2D rb;

    private bool getOne = false;

    private float distanceToDest;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (player == false)
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();   
            StartCoroutine(timer());
        }
    }

    private void Update()
    {
        flipUpdate();
        
        distance();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void getPlayerLocation()
    {
        if (_gameManager != null)
        {
            _destination = _gameManager._player.transform.position;
        }
    }

    void MousePosition()
    {
        if (getOne == false)
        {
            _destination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            getOne = true;   
        }
    }

    void distance()
    {
        distanceToDest = Vector2.Distance(transform.position, _destination);

        if (distanceToDest <= 0.5f)
        {
            Destroy(gameObject);
        }
    }

    void Movement()
    {
        if (player == false)
        {
            getPlayerLocation();
        }
        else
        {
            MousePosition();
        }
        
        transform.position = Vector2.MoveTowards(transform.position, _destination, 10f * Time.deltaTime);
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
