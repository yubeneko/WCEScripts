using System;
using UniRx;
using UnityEngine;
using WCE.Damages;
using CurlNoiseParticleSystem.Emitter;
using AIBehavior;
using Attribute = WCE.Damages.Attribute;

namespace WCE.Enemys
{
    public class Skelton : EnemyBase, IDamageApplicable, IAttacker
    {
        [SerializeField]
        private FloatReactiveProperty _hp = new FloatReactiveProperty ();
        public IReadOnlyReactiveProperty<float> HP { get { return _hp; } }

        public GameObject magicRing;
        private ShapeEmitter emitter;
        private Subject<Damage> _statusSubject = new Subject<Damage>();
        public IObservable <Damage> StatusObserver { get { return _statusSubject; } }

        void Awake()
        {
            attribute = Attribute.None;
            emitter = GetComponent<ShapeEmitter>();

            _hp.Where(x => x <= 0)
                .Subscribe(_ => Dead());
        }

        public void ApplyDamage (Damage damage)
        {
            _hp.Value -= damage.attackPower;
        }

        void Dead()
        {
            emitter.Emit();
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 3.0f);
        }

        public void OnAttack (AttackData attackData)
        {
            var damageData = new Damage
            {
                attribute = this.attribute,
                attackPower = attackData.damage
            };
            _statusSubject.OnNext(damageData);   
        }

        public void OnEnable()
        {
            Observable.Timer(TimeSpan.FromSeconds(1))
                .Subscribe(_ => magicRing.SetActive(false));
        }
    }
}