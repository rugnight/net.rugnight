
/// <summary>
/// 
/// </summary>
using System;
using System.Collections.Generic;
using UnityEngine;
namespace A00.Master
{
	[System.Serializable]
	public class UnitStatus : ScriptableObject
	{
        // --------------------------------------------------
        // マスター読み込み＆アクセス
        //
        static UnitStatus self = null;
        static public List<Model> DataList
        {

            get
            {
                if (self == null)
                {
                    self = Resources.Load<UnitStatus>("Master/Field");
                }
                return self.list;
            }

        }

        [SerializeField]
		public List<Model> list = new List<Model>();

		[System.Serializable]
		public class Model : ISerializationCallbackReceiver
		{

            [SerializeField]
            int id = default(int);
            public int Id { get { return id; } }

            [SerializeField]
            string name = default(string);
            public string Name { get { return name; } }

            [SerializeField]
            int hp = default(int);
            public int Hp { get { return hp; } }

            [SerializeField]
            int atk = default(int);
            public int Atk { get { return atk; } }

            [SerializeField]
            int def = default(int);
            public int Def { get { return def; } }

            [SerializeField]
            int spd = default(int);
            public int Spd { get { return spd; } }

            // --------------------------------------------------
            // ISerializationCallbackReceiver
            //
            public virtual void OnAfterDeserialize()
            {

            }
            public virtual void OnBeforeSerialize()
            {
            }
        }
    }
} // namespace Master