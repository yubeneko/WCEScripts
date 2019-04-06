using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace WCE.Damages
{
    public class PhysicalAttack : MonoBehaviour
    {
        private Damage damage;
        private Collider collider;
        private IAttacker attacker;

        void Awake()
        {
            collider = GetComponent<Collider>();
            attacker = GetComponentInParent<IAttacker>();
        }

        void Start()
        {
            attacker.StatusObserver.Subscribe(damage =>
            {
                this.damage = damage;
                collider.enabled = true;
                Observable.TimerFrame(10)
                .Subscribe(_ => collider.enabled = false)
                .AddTo(gameObject);
            });

            this.OnTriggerEnterAsObservable()
                .Select(collider => collider.gameObject.GetComponentInParent<IDamageApplicable>())
                .Where(d => d != null)
                .Subscribe(d => d.ApplyDamage(damage));
        }
    }
}