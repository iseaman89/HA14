public interface IDestructable
{
    float Health { get; set; }
    void RecieveHit(float damage);
    void Die();
}
