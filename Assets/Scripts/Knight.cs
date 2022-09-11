using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Creature, IDestructable
{
    [SerializeField] private float stairSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float attackRange;
    [SerializeField] private float hitDelay;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform attackPoint;
    private bool onGround = true;

    private bool onStair;

    public Stair stair;

    public bool OnStair
    {
        get => onStair; set

        {
            if (value)
            {
                rigidbody.gravityScale = 0;
            }

            else
            {
                rigidbody.gravityScale = 1;
            }

            onStair = value;
        }
    }

    private void Awake()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        health = GameController.S_instance.MaxHealth;
    }

    private void Update()
    {

        onGround = CheckGround();
        animator.SetBool("Jump", !onGround);
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
            //Attack();
            Invoke("Attack", hitDelay);
        }
        if (Input.GetButtonDown("Jump") && onGround)
        {
            rigidbody.AddForce(Vector2.up * jumpForce);
        }

        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
        Vector2 velocity = rigidbody.velocity;
        velocity.x = Input.GetAxis("Horizontal") * speed;
        rigidbody.velocity = velocity;
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
            velocity.y = Input.GetAxis("Vertical") * stairSpeed;
            rigidbody.velocity = velocity;
        }
    }

    private bool CheckGround()
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, groundCheck.position);
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);

        for (int i = 0; i < hits.Length; i++)
        {
            if (!GameObject.Equals(hits[i].gameObject, gameObject))
            {
                IDestructable destructable = hits[i].gameObject.GetComponent<IDestructable>();
                if (destructable != null)
                {
                    destructable.Hit(damage);
                    break;
                }
            }
        }
    }
}
