using UnityEngine;
using WCE.Damages;

public abstract class EnemyBase : MonoBehaviour
{
    protected Attribute attribute;

    protected float GetAttackMagnificationValue (Attribute receivedDamageAttribute)
    {
        switch (attribute)
        {
            // 自身の属性が炎
            case Attribute.Fire:
                if (receivedDamageAttribute == Attribute.Ice)
                {
                    return 2.0f;
                }
                else if (receivedDamageAttribute == Attribute.Nature)
                {
                    return 0.5f;
                }
                else
                {
                    return 1.0f;
                }

            // 自身の属性が氷
            case Attribute.Ice:
                if (receivedDamageAttribute == Attribute.Nature)
                {
                    return 2.0f;
                }
                else if (receivedDamageAttribute == Attribute.Fire)
                {
                    return 0.5f;
                }
                else
                {
                    return 1.0f;
                }

            // 自身の属性が自然
            case Attribute.Nature:
                if (receivedDamageAttribute == Attribute.Fire)
                {
                    return 2.0f;
                }
                else if (receivedDamageAttribute == Attribute.Ice)
                {
                    return 0.5f;
                }
                else
                {
                    return 1.0f;
                }
        }
        // 属性がない(None)なら1.0を返す
        return 1.0f;
    }
}
