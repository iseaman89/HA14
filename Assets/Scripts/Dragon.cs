using UnityEngine;

public class Dragon : Creature, IDestructable
{
    [SerializeField] private CircleCollider2D hitCollider;

    private void Update()
    {
        Vector2 velocity = rigidbody.velocity;
        velocity.x = Speed * transform.localScale.x * -1;
        rigidbody.velocity = velocity;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Knight knight = collision.gameObject.GetComponent<Knight>();
        if (knight != null)
        {
            animator.SetTrigger("Attack");
            Attack();
        }
        else
        {
            ChangeDirection();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Knight>() != null)
        {
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                Vector2 fromDragonToContactVector = collision.contacts[i].point
                    - (Vector2)transform.position;

                if (Vector2.Angle(fromDragonToContactVector, Vector2.up) < 45)
                {
                    Die();
                }
            }
        }
    }

    private void ChangeDirection()
    {
        if (transform.localScale.x < 0)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Attack()
    {
        Vector3 hitPosition = transform.TransformPoint(hitCollider.offset);
        DoHit(hitPosition, hitCollider.radius, Damage);
    }
}
