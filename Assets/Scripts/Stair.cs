using UnityEngine;

public class Stair : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Knight knight = collision.gameObject.GetComponent<Knight>();

        if (knight != null)
        {
            knight.OnStair = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Knight knight = collision.gameObject.GetComponent<Knight>();

        if (knight != null)
        {
            knight.OnStair = false;
        }
    }
}
