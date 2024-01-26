using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunOrb : MonoBehaviour
{
    public Collider2D Collider;
    public int StunAmount = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D: " + collision.gameObject.name);
        if(collision is IStunnable stunnable)
        {
            stunnable.OnStun(StunAmount);
        }
    }
}
