using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [Header("Componenets")]
    [SerializeField] private EnemyData data;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject hookExpVFX;
    private SpriteRenderer _spriteRenderer;
    private Sprite _sprite;
    private Sprite _spriteVirus;
    private AnimatorController _animation;
    private AnimatorController _animationVirus;
    private Animator _animator;
    private BoxCollider2D _collider;

    [Header("UI")]
    [SerializeField] private Text _textName;
    
    [Header("Art")]


    [Header("Values")] 
    [SerializeField] private float _health;
    [SerializeField] private float _speed;
    [SerializeField] private float _distanceAttack;
    
    private bool attacking = false;
    private bool ınTrigger = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        if (data != null)
        {
            InitializeData();
        }
    }

    private void Start()
    {
        // _spriteRenderer.sprite = _sprite
        _animator.runtimeAnimatorController = _animation;
        
        _collider.size = new Vector2(_spriteRenderer.size.x, _spriteRenderer.size.y);
    }

    private void Update()
    {
        if (attackDistance() == true)
        {
            animationControl(true);
        }

        attacking = attackDistance();

        controlAnimEnd();
        rotation();
    }

    void FixedUpdate()
    {
        followPlayer();

        ınTrigger = false;
    }

    private void InitializeData()
    {
        // _textName.text = data.enemyName;
        _sprite = data.Art;
        _spriteVirus = data.ArtVirus;
        _animation = data.AnimationNormal;
        _animationVirus = data.AnimationVırus;
        _health = data.Health;
        _speed = data.Speed;
        transform.localScale = data.Scale;
        _distanceAttack = data.Distance;
    }

    private void followPlayer()
    {
        if(attacking == false && ınTrigger == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, _gameManager._player.transform.position, _speed * Time.deltaTime);
        }
    }

    private void replacePlayer()
    {
        if (_gameManager != null & _gameManager._player != null)
        {
            _gameManager._player._capture = true;
            _gameManager._player.spriteShow(_spriteVirus);
            _gameManager._player.AnimationController(_animationVirus);
            _gameManager._player.collectDataEnemy(_speed + 2, _health, data.Scale);
            _gameManager._player.transform.position = transform.position;
        }
    }

    private bool attackDistance()
    {
        // Debug.Log("Distance: " + Vector2.Distance(this.transform.position, _gameManager._player.transform.position));
        
        if (Vector2.Distance(this.transform.position, _gameManager._player.transform.position) < _distanceAttack)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void controlAnimEnd()
    {
        if (_animator != null && _animator.GetCurrentAnimatorStateInfo(0).IsTag("InAttack"))
        {
            animationControl(false);
        }
    }

    private void animationControl(bool _condition)
    {
        _animator.SetBool("Attack", _condition);
    }

    private void rotation()
    {
        if (_gameManager._player.transform.position.x < transform.position.x)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "NormalAttack")
        {
            ınTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Hook")
        {
            replacePlayer();;
            Destroy(other.transform.parent.gameObject);
            Instantiate(hookExpVFX, transform.position, quaternion.identity);   
            Destroy(gameObject);
        }
    }
}
