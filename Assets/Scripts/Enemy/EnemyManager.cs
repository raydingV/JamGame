using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyData data;

    [Header("UI")]
    [SerializeField] private Text _textName;
    
    [Header("Art")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private AnimatorController _animation;
    [SerializeField] private Animator _animator;

    [Header("Values")] 
    [SerializeField] private float _health;
    [SerializeField] private float _speed;
    [SerializeField] private float _distanceAttack;
    
    private bool attacking = false;
    private bool ınTrigger = false;

    [SerializeField] private GameManager _gameManager;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
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
        _animation = data.Animation;
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
}
