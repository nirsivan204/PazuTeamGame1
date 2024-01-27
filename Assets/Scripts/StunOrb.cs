using System;
using UniRx;
using UnityEngine;

public enum Orb
{
    Orb1,
    Orb2,
    Orb3,
    Orb4,
    Orb5,
    Orb6,
    Orb7,
    Orb8,
}

public class OrbDisableEvent
{
    public Orb OrbNumber;
}

public class StunOrb : MonoBehaviour
{
    public int StunAmount = 10;
    private IDisposable _orbListener;
    [SerializeField] private Orb _orb;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<IStunnable>() is IStunnable stunnable)
        {
            stunnable.OnStun(StunAmount);
            MessageBroker.Default.Publish(new OrbDisableEvent { OrbNumber = _orb });
        }
    }

    private void OnEnable()
    {
        _orbListener = MessageBroker.Default.Receive<OrbDisableEvent>().ObserveOnMainThread().Subscribe(OnOrbStun);
    }

    private void OnDisable()
    {
        _orbListener.Dispose();
    }

    private void OnOrbStun(OrbDisableEvent orbDisableEvent)
    {
        if (orbDisableEvent.OrbNumber != _orb) 
            return;

        FadeAway();
    }

    private void FadeAway()
    {
        this.SetTimer(1, () =>
         {
             Destroy(transform.parent.gameObject);
         });
    }
}
