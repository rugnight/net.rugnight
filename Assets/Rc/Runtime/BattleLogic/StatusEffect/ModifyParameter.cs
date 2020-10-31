using System.Collections.Generic;

namespace BattleLogic
{
    public class ModifyParameter : StatusEffect
    {
        const int INFINITY_DURATION_TURN = -1;

        public Status Owner { get; private set; }

        List<ModifyParameterData> modifyParameterDatas = new List<ModifyParameterData>();
        public IReadOnlyList<ModifyParameterData> ModifyParameterDatas => modifyParameterDatas;

        int DurationTurns = INFINITY_DURATION_TURN;

        public ModifyParameter(string name) : base(name)
        {
        }

        public ModifyParameter(string name, Status owner, int durationTurns, params ModifyParameterData[] datas) : base(name)
        {
            Owner = owner;
            DurationTurns = durationTurns;

            for (int i = 0; i < datas.Length; ++i)
            {
                modifyParameterDatas.Add(datas[i]);
            }
        }

        public override void OnModifyParameter()
        {
            if (!IsActive())
            {
                return;
            }

            for (int i = 0; i < modifyParameterDatas.Count; ++i)
            {
                var parameter = Owner.GetParameter(modifyParameterDatas[i].parameterId);
                parameter.AddBonusValue(modifyParameterDatas[i].value);
            }
        }

        public override bool IsActive()
        {
            if (DurationTurns == INFINITY_DURATION_TURN)
            {
                return true;
            }
            return (0 < DurationTurns);
        }

        public override void OnBattleStart()
        {
        }

        public override void OnTurnStart()
        {
        }

        public override void OnTurnEnd()
        {
            if (0 < DurationTurns)
            {
                --DurationTurns;
            }
        }

        public override void OnBattleEnd()
        {
        }
    }
}