using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class PlayerManager : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Animator hookAnimator;
    [SerializeField] private AnimatorController animatorController;
    public SpriteRenderer _renderer;
    [SerializeField] private GameManager _gameManager;
        
    [SerializeField] private GameObject attackCollider;
    [SerializeField] private GameObject hookAttack;
    [SerializeField] private GameObject turnBackVFX;
    [SerializeField] private GameObject bloodVFX;
    [SerializeField] private GameObject InfectedProjectile;
    private GameObject newHook;
    private Sprite oldSprite;
    
    [Header("Values")]
    private float _horizontal;
    private float _vertical;
    [SerializeField] private float _speedPlayer = 12f;
    [SerializeField] private float _healthPlayer = 100f;
    private float oldHealth;
    private float oldSpeed;

    Vector3 mousePosition;
    private Vector3 oldScale;
    private Rigidbody2D newHookRb;
    private float camDis;
    private float angle;
    private float AngleRad;
    private float countDown = 15f;
    private float countDownDamage = 0f;

    private float coolDownAttacks = 0f;
    
    public bool _capture = false;
    private bool projectileInfect = false;

    private CapsuleCollider2D _collider2D;

    [SerializeField] private Slider healthBar;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        oldSprite = _renderer.sprite;
        oldScale = transform.localScale;
        healthBar.maxValue = _healthPlayer;
    }

    void Update()
    {
        if (_healthPlayer <= 0 && _gameManager != null)
        {
                _gameManager.GameOver();
        }
        
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

            if (_healthPlayer <= 0 && _capture == true)
            {
                getBackToNormal();
            }
        }

        healthBar.value = _healthPlayer;
        _gameManager.timerText.text = "CoolDown: " + coolDownAttacks.ToString("F2");

        if (coolDownAttacks >= 0)
        {
            _gameManager.ImpactImage.SetActive(false);
            _gameManager.TimerObject.SetActive(true);
        }
        else
        {
            _gameManager.ImpactImage.SetActive(true);
            _gameManager.TimerObject.SetActive(false);
        }

        Debug.Log(coolDownAttacks);
        coolDownAttacks -= Time.deltaTime;
        countDownDamage -= Time.deltaTime;
        
        if (newHook != null)
        {
            controlHookAnimEnd();
            hookTransform();
        }

        if (_capture == false && newHook == null)
        {
            playerHook();
        }

        // Debug.Log(countDown);
        //Debug.Log(_healthPlayer);
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
        if (Input.GetMouseButtonDown(0) && coolDownAttacks <= 0)
        {
            coolDownAttacks = 1f;
            StartCoroutine(Impact());
            _collider2D.isTrigger = true;
            Instantiate(attackCollider, transform.position, Quaternion.identity);
        }
    }

    IEnumerator Impact()
    {
        yield return new WaitForSeconds(0.7f);
        _collider2D.isTrigger = false;
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
        if (Input.GetMouseButtonDown(0) && gameObject.tag == "Untagged" && coolDownAttacks <= 0)
        {
            coolDownAttacks = 1f;
            Vector2 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (_mousePosition.x > transform.position.x)
            {
                _renderer.flipX = false;
            }
            else
            {
                _renderer.flipX = true;
            }

            if (projectileInfect == true)
            {
                Instantiate(InfectedProjectile, new Vector2(transform.position.x, transform.position.y + 2),
                    Quaternion.identity);
            }
            
            gameObject.tag = "Player";
            _animator.SetBool("Attack", true);
        }
    }
    
    private void controlAnimEnd()
    {
        if (_animator != null && _animator.GetCurrentAnimatorStateInfo(0).IsTag("InAttack"))
        {
            gameObject.tag = "Untagged";
            _animator.SetBool("Attack", false);
        }
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

    public void collectDataEnemy(float _speed, float _health, Vector3 _scale, bool condition)
    {
        oldSpeed = _speedPlayer;
        _speedPlayer = 20;

        oldHealth = _healthPlayer;
        _healthPlayer = 30;

        transform.localScale = _scale;

        projectileInfect = condition;
        
        healthBar.maxValue = _healthPlayer;
    }

    private void timer()
    {
        countDown -= Time.deltaTime;

        if (countDown <= 0 && _capture == true)
        {
            countDown = 15f;
            getBackToNormal();;
        }
    }

    public void getBackToNormal()
    {
        Instantiate(turnBackVFX, transform.position, Quaternion.identity);
        _animator.runtimeAnimatorController = animatorController;
        _renderer.sprite = oldSprite;
        _healthPlayer = oldHealth;
        _speedPlayer = oldSpeed;
        transform.localScale = oldScale;
        gameObject.tag = "Untagged";
        countDown = 15f;
        _capture = false;
        projectileInfect = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.TryGetComponent<EnemyManager>(out EnemyManager _data))
        {
            if (_data != null && other.gameObject.CompareTag("EnemyAttack") && gameObject.CompareTag("Untagged"))
            {
                countDownDamage = 2f;
                _healthPlayer -= _data._damage;
                Instantiate(bloodVFX, transform.position, quaternion.identity);
            }
        }

        if (other.gameObject.CompareTag("Projectile") && countDownDamage <= 0)
        {
            countDownDamage = 2f;
            Destroy(other.gameObject);
            _healthPlayer -= 10f;
            Instantiate(bloodVFX, transform.position, quaternion.identity);
        }
    }
}
