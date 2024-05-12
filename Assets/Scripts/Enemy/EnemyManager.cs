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
    public EnemyData data;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject hookExpVFX;
    [SerializeField] private GameObject projectileGameObject;
    private GameObject newProjectile;
    private SpriteRenderer _spriteRenderer;
    private Sprite _sprite;
    private Sprite _spriteVirus;
    private AnimatorController _animation;
    private AnimatorController _animationVirus;
    private Animator _animator;
    private BoxCollider2D _collider;
    private Rigidbody2D _rigidbody2D;

    [Header("UI")]
    [SerializeField] private Text _textName;
    
    [Header("Art")]


    [Header("Values")] 
    private float _health;
    private float _speed;
    private float _distanceAttack;
    private float timer = 0f;
    private float damageTimer = 0f;
    [HideInInspector] public float _damage;
    
    private bool attacking = false;
    private bool ınTrigger = false;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
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
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // _spriteRenderer.sprite = _sprite
        _animator.runtimeAnimatorController = _animation;
        
        _collider.size = new Vector2(_spriteRenderer.size.x, _spriteRenderer.size.y);
    }

    private void Update()
    {
        if (attackDistance() == true)
        {
            gameObject.tag = "EnemyAttack";
            animationControl(true);
        }
        else
        {
            gameObject.tag = "Enemy";
        }

        if (projectileGameObject != null && attackDistance() == true && timer <= 0f)
        {
            newProjectile = Instantiate(projectileGameObject, new Vector2(transform.position.x, transform.position.y + 2),
                Quaternion.identity);
            timer = 3f;
        }
        
        timer -= Time.deltaTime;

        attacking = attackDistance();

        controlAnimEnd();
        rotation();

        damageTimer -= Time.deltaTime;

        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0f;
        _rigidbody2D.Sleep();

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
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
        _damage = data.Damage;
        projectileGameObject = data.Projectile;
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
            Destroy(gameObject);
            replacePlayer();;
            Destroy(other.transform.parent.gameObject);
            Instantiate(hookExpVFX, transform.position, quaternion.identity);
        }

        if (other.gameObject.tag == "Player" && damageTimer <= 0)
        {
            damageTimer -= 2f;
            _health -= 1;
            Debug.Log("GET Hit " + _health);
        }
    }
}
