using UnityEngine;

public class Creature : MonoBehaviour, IDestructable
{
    protected Animator animator;
    protected new Rigidbody2D rigidbody;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] protected float health;
    [SerializeField] protected float dieDelay;

    public float Health { get => health; set => health = value; }
    public float Speed { get => speed; set => speed = value; }
    public float Damage { get => damage; set => damage = value; }

    private void Awake()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    public virtual void Die()
    {
        GameController.S_instance.Killed(this);
    }

    public void RecieveHit(float damage)
    {

        Health -= damage;
        GameController.S_instance.Hit(this);
        if (Health <= 0)
        {
            Die();
        }
    }

    protected void DoHit(Vector3 hitPosition, float hitRadius, float hitDamage)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(hitPosition, hitRadius);

        for (int i = 0; i < hits.Length; i++)
        {
            if (!GameObject.Equals(hits[i].gameObject, gameObject))
            {
                IDestructable destructable = hits[i].gameObject.GetComponent<IDestructable>();

                if (destructable != null)
                {
                    destructable.RecieveHit(hitDamage);
                }
            }
        }
    }
}
