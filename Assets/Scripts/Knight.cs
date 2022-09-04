using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidbody2D;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundCheck;
    private bool onGround = true;


    private void Awake()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        onGround = CheckGround();
        animator.SetBool("Jump", onGround);
        if (Input.GetButtonDown("Fire1")) animator.SetTrigger("Attack");
        if (Input.GetButtonDown("Jump") && onGround) rigidbody2D.AddForce(Vector2.up * jumpForce);
        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));
        Vector2 velocity = rigidbody2D.velocity;
        velocity.x = Input.GetAxis("Horizontal") * speed;
        rigidbody2D.velocity = velocity;
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

}
