namespace BattleLogic
{
    public class Parameter
    {
        public int Value => (BaseValue + BonusValue - DamageValue);
        public int BaseValue => baseValue;
        public int BonusValue => bonusValue;
        public int DamageValue => damageValue;

        int baseValue = 0;
        int bonusValue = 0;
        int damageValue = 0;

        public Parameter() { }

        public Parameter(int baseValue)
        {
            this.baseValue = baseValue;
        }

        public void SetBaseValue(int value)
        {
            baseValue = value;
        }

        public void AddBonusValue(int value)
        {
            bonusValue += value;
        }

        public void ClearBonusValue()
        {
            bonusValue = 0;
        }

        public void AddDamageValue(int value)
        {
            damageValue += value;
        }

        public void ClearDamageValue()
        {
            damageValue = 0;
        }
    }
}
