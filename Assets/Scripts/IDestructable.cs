public interface IDestructable
{
    float Health { get; set; }
    void Hit(float damage);
    void Die();
}
