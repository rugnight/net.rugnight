namespace BattleLogic
{
    public abstract class StatusEffect
    {
        public string Name { get; private set; } = "";

        public StatusEffect(string name)
        {
            Name = name;
        }

        abstract public bool IsActive();

        abstract public void OnModifyParameter();

        abstract public void OnBattleStart();

        abstract public void OnBattleEnd();

        abstract public void OnTurnStart();

        abstract public void OnTurnEnd();
    }
}
