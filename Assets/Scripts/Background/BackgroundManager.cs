using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private BackgroundScroller _bottomBackground;
    private float _yLoopResetOffset;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        BackgroundScroller bgToMoveToBottom = collision.GetComponent<BackgroundScroller>();
        if (bgToMoveToBottom != null)
        {
            _yLoopResetOffset = _bottomBackground.YSize;
            bgToMoveToBottom.transform.position = _bottomBackground.transform.position - new Vector3(0, _yLoopResetOffset, 0);

            _bottomBackground = bgToMoveToBottom;

        }
    }
}
