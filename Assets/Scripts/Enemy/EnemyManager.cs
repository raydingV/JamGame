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
    
    private bool attacking = false;

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
        if (attackDistance() == true && attacking == false)
        {
            attacking = true;
            animationControl();
        }
        
        controlAnimEnd();
        rotation();
    }

    void FixedUpdate()
    {
        followPlayer();
    }

    private void InitializeData()
    {
        // _textName.text = data.enemyName;
         _sprite = data.Art;
        _animation = data.Animation;
        _health = data.Health;
        _speed = data.Speed;
    }

    private void followPlayer()
    {
        if(attacking == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, _gameManager._player.transform.position, _speed * Time.deltaTime);
        }
    }

    private bool attackDistance()
    {
        // Debug.Log("Distance: " + Vector2.Distance(this.transform.position, _gameManager._player.transform.position));
        
        if (Vector2.Distance(this.transform.position, _gameManager._player.transform.position) < 3f)
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
        if (_animator != null && _animator.GetCurrentAnimatorStateInfo(0).IsTag("attackFinish"))
        {
            attacking = false;
            animationControl();
        }
    }

    private void animationControl()
    {
        _animator.SetBool("Attack", attacking);
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
}
