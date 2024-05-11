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
    private Animator _animator;
    private Animator hookAnimator;
    [SerializeField] private AnimatorController animatorController;
    public SpriteRenderer _renderer;
        
    [SerializeField] private GameObject attackCollider;
    [SerializeField] private GameObject hookAttack;
    private GameObject newHook;
    private Sprite oldSprite;
    
    [Header("Values")]
    private float _horizontal;
    private float _vertical;
    [SerializeField] private float _speedPlayer = 5f;
    [SerializeField] private float _healthPlayer = 100f;

    Vector3 mousePosition;
    private Vector3 oldScale;
    private Rigidbody2D newHookRb;
    private float camDis;
    private float angle;
    private float AngleRad;
    private float countDown = 3f;

    public bool _capture = false;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        oldSprite = _renderer.sprite;
        oldScale = transform.localScale;
    }

    void Update()
    {
        if (_capture == false)
        {
            attackInput();
        
            animationPlayer();
        }
        else
        {
            ınfectedAttack();
            flipSprite();
            controlAnimEnd();
            timer();;
        }
        
        if (newHook != null)
        {
            controlHookAnimEnd();
            hookTransform();
        }

        if (_capture == false && newHook == null)
        {
            playerHook();
        }

        Debug.Log(countDown);
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
        _animator.SetFloat("Speed", Mathf.Abs(_horizontal + _vertical));
    }

    void playerHook()
    {
        if (Input.GetMouseButtonDown(1))
        {
            newHook = Instantiate(hookAttack, transform.position, Quaternion.identity);
            newHookRb = newHook.GetComponent<Rigidbody2D>();
            hookAnimator = newHook.GetComponent<Animator>();
            
            if (newHook != null)
            {
                camDis = Camera.main.transform.position.y - newHook.transform.position.y;
            
                mousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, camDis));
            }
        }
    }

    void hookTransform()
    {
        newHook.transform.position = transform.position;

        AngleRad = Mathf.Atan2 (mousePosition.y - newHook.transform.position.y, mousePosition.x - newHook.transform.position.x);
        angle = (180 / Mathf.PI) * AngleRad;

        newHookRb.rotation = angle;
    }

    void ınfectedAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetBool("Attack", true);
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

    private void controlHookAnimEnd()
    {
        if (hookAnimator != null && hookAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Destroy"))
        {
            Destroy(newHook.gameObject);
        }
    }

    private void flipSprite()
    {
        if (_horizontal > 0)
        {
            _renderer.flipX = false;
        }
        else if (_horizontal < 0)
        {
            _renderer.flipX = true;
        }
    }

    public void spriteShow(Sprite _sprite)
    {
        _renderer.sprite = _sprite;
    }

    public void AnimationController(AnimatorController _animatorController)
    {
        _animator.runtimeAnimatorController = _animatorController;
    }

    public void collectDataEnemy(float _speed, float _health, Vector3 _scale)
    {
        _speedPlayer = _speed;
        _healthPlayer = _health;

        transform.localScale = _scale;
    }

    private void timer()
    {
        countDown -= Time.deltaTime;

        if (countDown <= 0)
        {
            _capture = false;
            countDown = 3f;
            getBackToNormal();;
        }
    }

    public void getBackToNormal()
    {
        _animator.runtimeAnimatorController = animatorController;
        _renderer.sprite = oldSprite;
        transform.localScale = oldScale;
    }
}
