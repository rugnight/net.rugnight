namespace BattleLogic
{
    public struct ModifyParameterData
    {
        public int parameterId;
        public int value;

        public ModifyParameterData(int _parameterId, int _value)
        {
            parameterId = _parameterId;
            value = _value;
        }
    }
}
