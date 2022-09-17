using UnityEngine;

public class Creature : MonoBehaviour, IDestructable
{
    protected Animator animator;
    protected Rigidbody2D rigidbody;
    [SerializeField] protected float speed;
    [SerializeField] protected float damage;
    [SerializeField] protected float health;
    [SerializeField] protected float dieDelay;

    public float Health { get => health; set => health = value; }

    private void Awake()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void Hit(float damage)
    {

        Health -= damage;
        GameController.S_instance.Hit(this);
        if (Health <= 0)
        {
            Die();
        }
    }
}
