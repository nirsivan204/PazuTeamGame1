using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private Transform _otherBackground;
    [SerializeField] float _ySize;
    [SerializeField] Vector3 _movementVector;
    [SerializeField] Renderer _renderer;

    public float YSize { get => _ySize; }

    private void Start()
    {
        _ySize = _renderer.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + _movementVector * Time.deltaTime;
    }

    
}
