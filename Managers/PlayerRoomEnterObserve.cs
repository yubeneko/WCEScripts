using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class PlayerRoomEnterObserve : MonoBehaviour
{
    private readonly Subject<Unit> _roomEnterSubject = new Subject<Unit>();
    public IObservable <Unit> OnPlayerRoomEnter { get { return _roomEnterSubject; } }

    void Start()
    {
        this.OnTriggerEnterAsObservable()
            .Where(collider => collider.gameObject.tag == "Player")
            .Take(1)
            .Subscribe(_ =>
            {
                _roomEnterSubject.OnNext(Unit.Default);
                _roomEnterSubject.OnCompleted();
            });
    }
}
