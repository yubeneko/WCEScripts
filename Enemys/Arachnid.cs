using UnityEngine;
using WCE.Damages;
using CurlNoiseParticleSystem.Emitter;
using AIBehavior;
using UniRx;

namespace WCE.Enemys
{
    public class Arachnid : EnemyBase, IDamageApplicable
    {
        private ShapeEmitter emitter;
        private AIBehaviors aiBehavior;

        public Transform dischargePoint;
        public GameObject natureMagic;
        public Transform targetPosition;
        public GameObject magicRing;


        void Awake()
        {
            attribute = Attribute.Nature;
            emitter = GetComponent<ShapeEmitter>();
            aiBehavior = GetComponent<AIBehaviors>();

            aiBehavior.ObserveEveryValueChanged(x => x.currentState)
                .Where(x => x is DeadState)
                .Subscribe(_ => Dead());
        }

        public void ApplyDamage(Damage damage)
        {
            var attackmagnification = GetAttackMagnificationValue(damage.attribute);

            Debug.Log(attackmagnification);
            
            aiBehavior.SubtractHealthValue(damage.attackPower * attackmagnification);
        }

        void Dead()
        {
            emitter.Emit();
            GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 3.0f);
        }

        public void OnAttack(AttackData attackData)
        {
            if (attackData.target == null)
                return;

            dischargePoint.LookAt(targetPosition);
            var magicInstance = Instantiate(
                natureMagic,
                dischargePoint.position,
                dischargePoint.rotation
                );

            var applyDamageData = new Damage
            {
                attribute = this.attribute,
                attackPower = attackData.damage,
            };

            magicInstance.GetComponentInChildren<Magic>().Init(applyDamageData);
        }

        public void OnEnable()
        {
            Observable.Timer(System.TimeSpan.FromSeconds(1))
                .Subscribe(_ => magicRing.SetActive(false));
        }
    }
}