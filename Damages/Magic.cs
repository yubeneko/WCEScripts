using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace WCE.Damages
{
    public class Magic : MonoBehaviour
    {
        private Damage damage;

        public void Init (Damage damage)
        {
            this.damage = damage;
        }

        void Start ()
        {
            Observable.Timer(System.TimeSpan.FromSeconds(7))
                        .Subscribe(_ => Destroy(transform.parent.gameObject));

            this.OnTriggerEnterAsObservable()
                .Select(collider => collider.gameObject.GetComponentInParent<IDamageApplicable>())
                .Where(d => d != null)
                .Subscribe(d => d.ApplyDamage(damage));
        }
    }
}