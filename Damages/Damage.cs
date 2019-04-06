namespace WCE.Damages
{
    public struct Damage
    {
        public Attribute attribute;
        public float attackPower;
    }

    public enum Attribute
    {
        None,
        Fire,
        Nature,
        Ice
    }
}