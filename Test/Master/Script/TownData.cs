
/// <summary>
/// 
/// </summary>
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Master
{
	[System.Serializable]
	public class TownData : ScriptableObject
	{
        // --------------------------------------------------
        // マスター読み込み＆アクセス
        //
        static TownData self = null;
        static public List<TownDataParameters> DataList
        {

            get
            {
                if (self == null)
                {
                    self = Resources.Load<TownData>("Master/Field");
                }
                return self.list;
            }

        }

		public List<TownDataParameters> list = new List<TownDataParameters>();

		[System.Serializable]
		public class TownDataParameters : ISerializationCallbackReceiver
		{

            [SerializeField]
            uint id = default(uint);
            public uint idData { get { return id; } }

            [SerializeField]
            string name = default(string);
            public string nameData { get { return name; } }

            [SerializeField]
            string desc = default(string);
            public string descData { get { return desc; } }

            [SerializeField]
            string startDate = "";

            [SerializeField]
            DateTime startDateParsed = default(DateTime);
            public DateTime startDateData { get { return startDateParsed; }}
            

            // --------------------------------------------------
            // ISerializationCallbackReceiver
            //
            public void OnAfterDeserialize()
            {

                startDateParsed = DateTime.Parse(startDate);
            
            }
            public void OnBeforeSerialize()
            {
            }
        }
    }
} // namespace Master