using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private Transform _otherBackground;
    [SerializeField] float _yLoopResetOffset;
    [SerializeField] Vector3 _movementVector;
    [SerializeField] Renderer _renderer;

    private void Start()
    {
        _yLoopResetOffset = _renderer.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + _movementVector * Time.deltaTime;
    }

    private void OnBecameInvisible()
    {
        if(Application.isPlaying)
        {
            Debug.Log("please");
            if (_otherBackground != null)
            {
                transform.position = _otherBackground.transform.position - new Vector3(0, _yLoopResetOffset, 0);
            }           
        }        
    }
}
