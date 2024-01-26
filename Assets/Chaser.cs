using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Chaser : MonoBehaviour
{
    [SerializeField] float _speed_1 = 1;
    [SerializeField] float _speed_2 = 1.5f;
    [SerializeField] float _speed_3 = 2;
    [SerializeField] float _startTime = 20;
    [SerializeField] float _speedTime_1 = 60;
    [SerializeField] float _speedTime_2 = 100;
    [SerializeField] Rigidbody2D _rb;
    bool isMoving = false;
    private float _speed;

    public static UnityEvent<bool> OnHit;

    public void Start()
    {
        StartMove(_speed_1);

        this.SetTimer(_speedTime_1, () =>
        {
            SetSpeed(_speed_2);
        });

        this.SetTimer(_speedTime_2, () =>
        {
            SetSpeed(_speed_3);
        });
    }
    void SetSpeed(float speed)
    {
        if (isMoving)
        {
            _speed = speed;
            _rb.velocity = Vector2.up * _speed;
        }
    }
    // Start is called before the first frame update
    void StartMove(float _startTime)
    {
        this.SetTimer(_startTime, () =>
         {
             isMoving = true;
             _rb.velocity = Vector2.up * _speed;
         });
    }

    // Update is called once per frame
    void StopMove()
    {
        isMoving = false ;
        _rb.velocity = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<AbstractPlayerMovement>())
            OnHit?.Invoke(true);
    }
}
