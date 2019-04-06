using UnityEngine;
using UniRx;
using WCE.Damages;

namespace WCE.Players
{
    public class PlayerCore : MonoBehaviour, IDamageApplicable
    {
        [SerializeField]
        private FloatReactiveProperty _hp = new FloatReactiveProperty(10);
        public IReadOnlyReactiveProperty<float> Hp { get { return _hp; } }

        void Start ()
        {
            _hp.Where(x => x <= 0)
                .Subscribe(_ => Dead());
        }

        public void ApplyDamage(Damage damage)
        {
            _hp.Value -= damage.attackPower;
        }

        void Dead()
        {
            Debug.Log("you are Dead!");
        }
    }
}