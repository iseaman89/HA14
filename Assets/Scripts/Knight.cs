using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Creature, IDestructable
{
    [SerializeField] private float _stairSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _hitDelay;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _attackPoint;

    private bool _onGround = true;
    private bool _onStair;

    private Stair _stair;

    public bool OnStair
    {
        get => _onStair;

        set
        {
            if (value)
            {
                rigidbody.gravityScale = 0;
            }

            else
            {
                rigidbody.gravityScale = 1;
            }

            _onStair = value;
        }
    }

    private void Start()
    {
        GameController.S_instance.Knight = this;
        GameController.S_instance.OnUpdateHeroParameters += HandleOnUpdateParameters;
    }

    private void Update()
    {
        _onGround = CheckGround();
        animator.SetBool("Jump", !_onGround);

        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
        Vector2 velocity = rigidbody.velocity;
        velocity.x = Input.GetAxis("Horizontal") * Speed;
        rigidbody.velocity = velocity;

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetBool("Attack", true);
            Invoke("Attack", _hitDelay);
        }
        if (Input.GetButtonDown("Jump") && _onGround)
        {
            animator.SetBool("Jump", _onGround);

            rigidbody.AddForce(Vector2.up * _jumpForce);
            GameController.S_instance.AudioManager.PlaySound("Jump");
        }

        if (transform.localScale.x < 0)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.localScale = Vector3.one;
            }
        }
        else
        {
            if (Input.GetAxis("Horizontal") < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        if (OnStair)
        {
            velocity = rigidbody.velocity;
            velocity.y = Input.GetAxis("Vertical") * _stairSpeed;
            rigidbody.velocity = velocity;
        }
    }

    private void OnDestroy()
    {
        GameController.S_instance.OnUpdateHeroParameters -= HandleOnUpdateParameters;
    }

    private bool CheckGround()
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, _groundCheck.position);
        for (int i = 0; i < hits.Length; i++)
        {
            if (!GameObject.Equals(hits[i].collider.gameObject, gameObject))
            {
                return true;
            }
        }
        return false;
    }

    private void Attack()
    {
        GameController.S_instance.AudioManager.PlaySound("Attack");

        DoHit(_attackPoint.position, _attackRange, Damage);
        
        animator.SetBool("Attack", false);
    }

    private void HandleOnUpdateParameters(HeroParameters parameters)
    {
        Health = parameters.MaxHealth;
        Damage = parameters.Damage;
        Speed = parameters.Speed;
    }
}
