
/// <summary>
/// 
/// </summary>
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Master
{
	[System.Serializable]
	public class TestData : ScriptableObject
	{
        // --------------------------------------------------
        // マスター読み込み＆アクセス
        //
        static TestData self = null;
        static public List<TestDataParameters> DataList
        {

            get
            {
                if (self == null)
                {
                    self = Resources.Load<TestData>("Master/Field");
                }
                return self.list;
            }

        }

		public List<TestDataParameters> list = new List<TestDataParameters>();

		[System.Serializable]
		public class TestDataParameters : ISerializationCallbackReceiver
		{

            [SerializeField]
            string Title = default(string);
            public string TitleData { get { return Title; } }

            [SerializeField]
            string StringTest = default(string);
            public string StringTestData { get { return StringTest; } }

            [SerializeField]
            uint UintTest = default(uint);
            public uint UintTestData { get { return UintTest; } }

            [SerializeField]
            int IntTest = default(int);
            public int IntTestData { get { return IntTest; } }

            [SerializeField]
            uint LongTest = default(uint);
            public uint LongTestData { get { return LongTest; } }

            [SerializeField]
            ulong UlongTest = default(ulong);
            public ulong UlongTestData { get { return UlongTest; } }

            [SerializeField]
            double DoubleTest = default(double);
            public double DoubleTestData { get { return DoubleTest; } }

            [SerializeField]
            float FloatTest = default(float);
            public float FloatTestData { get { return FloatTest; } }

            [SerializeField]
            List<int> IntArrayTest = default(List<int>);
            public List<int> IntArrayTestData { get { return IntArrayTest; } }

            [SerializeField]
            List<uint> UintArrayTest = default(List<uint>);
            public List<uint> UintArrayTestData { get { return UintArrayTest; } }

            [SerializeField]
            List<float> FloatArrayTest = default(List<float>);
            public List<float> FloatArrayTestData { get { return FloatArrayTest; } }

            [SerializeField]
            List<double> DoubleArrayTest = default(List<double>);
            public List<double> DoubleArrayTestData { get { return DoubleArrayTest; } }

            [SerializeField]
            List<string> StringArrayTest = default(List<string>);
            public List<string> StringArrayTestData { get { return StringArrayTest; } }

            [SerializeField]
            string DateTimeTest = "";

            [SerializeField]
            DateTime DateTimeTestParsed = default(DateTime);
            public DateTime DateTimeTestData { get { return DateTimeTestParsed; }}
            

            [SerializeField]
            string EnumTest = "";

            [SerializeField]
            RuntimePlatform EnumTestParsed = default(RuntimePlatform);
            public RuntimePlatform EnumTestData { get { return EnumTestParsed; } }

            // --------------------------------------------------
            // ISerializationCallbackReceiver
            //
            public void OnAfterDeserialize()
            {

                DateTimeTestParsed = DateTime.Parse(DateTimeTest);
            
                EnumTestParsed = (RuntimePlatform)Enum.Parse(typeof(RuntimePlatform), EnumTest);
            
            }
            public void OnBeforeSerialize()
            {
            }
        }
    }
} // namespace Master