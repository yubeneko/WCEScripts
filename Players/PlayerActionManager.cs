using UnityEngine;
using UniRx;
using UniRx.Triggers;
using WCE.Damages;
using Attribute = WCE.Damages.Attribute;

namespace WCE.Players
{
    public class PlayerActionManager : MonoBehaviour
    {
        public Transform magicDischargePoint;
        IInputProvider inputProvider;

        private GameObject[] magicArr;
        private int currentAttributePointer = 0;

        private Attribute currentAttribute;
        private float currentAttackPower = 1.0f;

        void Start()
        {
            magicArr = new GameObject[]
            {
                (GameObject)Resources.Load("Prefabs/Magics/FireMagic"),
                (GameObject)Resources.Load("Prefabs/Magics/IceMagic"),
                (GameObject)Resources.Load("Prefabs/Magics/NatureMagic"),
            };

            this.UpdateAsObservable()
                .Select(_ => currentAttributePointer)
                .Subscribe(x =>
                {
                    switch (x)
                    {
                        case 0:
                            currentAttribute = Attribute.Fire;
                            break;
                        case 1:
                            currentAttribute = Attribute.Ice;
                            break;
                        case 2:
                            currentAttribute = Attribute.Nature;
                            break;
                    }
                });

            inputProvider = GetComponent<IInputProvider>();

            inputProvider.OnDischarge
                .Subscribe(_ =>
                {
                    var magicInstance = Instantiate(
                        magicArr[currentAttributePointer],
                        magicDischargePoint.position,
                        magicDischargePoint.rotation
                        );

                    var applyDamage = new Damage
                    {
                        attribute = currentAttribute,
                        attackPower = currentAttackPower,
                    };

                    magicInstance.GetComponentInChildren<Magic>().Init(applyDamage);
                });

            inputProvider.OnPause
                .Where(x => x)
                .Subscribe(_ => Debug.Log("ポーズボタン"));

            inputProvider.OnAttributeChange
                .Subscribe(x =>
                {
                    switch (x)
                    {
                        case "right":
                            if (currentAttributePointer == 2)
                                currentAttributePointer = 0;
                            else
                                currentAttributePointer++;
                            break;
                        case "left":
                            if (currentAttributePointer == 0)
                                currentAttributePointer = 2;
                            else
                                currentAttributePointer--;
                            break;
                        default:
                            Debug.LogError("判断できない文字列を受け取りました。");
                            break;
                    }
                });
        }
    }
}