using UnityEngine;

public class StunOrb : MonoBehaviour
{
    public int StunAmount = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<IStunnable>() is IStunnable stunnable)
        {
            stunnable.OnStun(StunAmount);
        }
    }
}
