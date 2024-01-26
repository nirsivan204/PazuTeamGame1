using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chaser : MonoBehaviour
{
    [SerializeField] float _speed = 1;
    [SerializeField] Rigidbody2D _rb;
    bool isMoving = false;

    public static UnityEvent<bool> OnHit;

    public void Start()
    {
        StartMove();
    }
    void SetSpeed(float speed)
    {
        _speed = speed;
    }
    // Start is called before the first frame update
    void StartMove()
    {
        isMoving = true;
        _rb.velocity = Vector2.up * _speed;
    }

    // Update is called once per frame
    void StopMove()
    {
        isMoving = false ;
        _rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Legs")
            OnHit?.Invoke(true);
        if (collision.gameObject.name == "Hands")
            OnHit?.Invoke(false);
    }
}
