using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<HandsCenter>())
        {
            collision.gameObject.GetComponentInParent<TopPlayerController>().OnGap();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<HandsCenter>())
        {
            collision.gameObject.GetComponentInParent<TopPlayerController>().OffGap();
        }
    }
}
