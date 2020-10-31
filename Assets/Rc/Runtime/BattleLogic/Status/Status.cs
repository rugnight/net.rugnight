using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleLogic
{
    public class Status
    {
        public string Name { get; private set; } = "";

        Parameter[] Parameters = new Parameter[0];

        List<StatusEffect> StatusEffects = new List<StatusEffect>();

        public Status(string name, int parameterNum)
        {
            Name = name;
            Parameters = new Parameter[parameterNum];
            for (int i = 0; i < parameterNum; ++i)
            {
                Parameters[i] = new Parameter();
            }
        }

        public void Standby()
        {
            for (int i = 0; i < Parameters.Length; ++i)
            {
                Parameters[i].ClearBonusValue();
            }
            //Hp = GetParameter(ParameterType.Hp).Value;

            StatusEffects.Clear();
        }

        public void SetParameter(int parameterId, int value)
        {
            Parameters[parameterId].SetBaseValue(value);
        }

        public Parameter GetParameter(int parameterId)
        {
            return Parameters[parameterId];
        }

        public void AddStatusEffect(StatusEffect statusEffect)
        {
            StatusEffects.Add(statusEffect);
            ModifyParameters();
        }

        public void OnBattleStart()
        {
            StatusEffects.ForEach(o => o.OnBattleStart());
        }

        public void OnBattleEnd()
        {
            StatusEffects.ForEach(o => o.OnBattleEnd());
        }

        public void OnTurnStart()
        {
            StatusEffects.ForEach(o => o.OnTurnStart());
        }

        public void OnTurnEnd()
        {
            StatusEffects.ForEach(o => o.OnTurnEnd());
            StatusEffects.RemoveAll(o => !o.IsActive());
            ModifyParameters();
        }

        void ModifyParameters()
        {
            for (int i = 0; i < Parameters.Length; ++i)
            {
                Parameters[i].ClearBonusValue();
            }

            for (int i = 0; i < StatusEffects.Count; ++i)
            {
                StatusEffects[i].OnModifyParameter();
            }
        }

    }
}
