using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Princess : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Knight knight = collision.gameObject.GetComponent<Knight>();
        if (knight != null)
        {
            GameController.S_instance.PrincessFound();
        }
    }
}
