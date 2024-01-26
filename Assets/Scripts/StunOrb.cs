using UnityEngine;

public class StunOrb : MonoBehaviour
{
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
