using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rc
{
    /// <summary>
    /// マスターデータクラスの生成
    /// 読み込みコード中に埋め込むと汚いので分離
    /// </summary>
    static public class MasterClassCreater 
    {

        /// <summary>
        /// クラス本体のソースコードを生成
        /// </summary>
        /// <param name="className"></param>
        /// <param name="paramClassName"></param>
        /// <param name="paramBody"></param>
        /// <param name="afterSerializeBody"></param>
        /// <returns></returns>
        static public string CreateClassBodyCode(string className, string paramClassName, string paramBody, string afterSerializeBody)
        {
            return string.Format(@"
/// <summary>
/// 
/// </summary>
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Master
{{
	[System.Serializable]
	public class {0} : ScriptableObject
	{{
        // --------------------------------------------------
        // マスター読み込み＆アクセス
        //
        static {0} self = null;
        static public List<{1}> DataList
        {{

            get
            {{
                if (self == null)
                {{
                    self = Resources.Load<{0}>(""Master/Field"");
                }}
                return self.list;
            }}

        }}

		public List<{1}> list = new List<{1}>();

		[System.Serializable]
		public class {1} : ISerializationCallbackReceiver
		{{
{2}
            // --------------------------------------------------
            // ISerializationCallbackReceiver
            //
            public void OnAfterDeserialize()
            {{
{3}
            }}
            public void OnBeforeSerialize()
            {{
            }}
        }}
    }}
}} // namespace Master"
            , className
            , paramClassName
            , paramBody
            , afterSerializeBody
            );
        }

        /// <summary>
        /// データ部分のソースコードを生成
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="varName"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        static public string CreateDataCode(string dataType, string varName, string propName)
        {
            return string.Format(@"
            [SerializeField]
            {0} {1} = default({0});
            public {0} {2} {{ get {{ return {1}; }} }}"
            , dataType
            , varName
            , propName
            );
        }

        /// <summary>
        /// Enumデータの読み込み部分のソースコードを生成
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="propName"></param>
        /// <param name="dataValue"></param>
        /// <param name="varNameParsed"></param>
        /// <returns></returns>
        static public string CreateEnumDataCode(string varName, string propName, string dataValue, string varNameParsed)
        {
            return string.Format(@"
            [SerializeField]
            string {0} = """";

            [SerializeField]
            {1} {2} = default({1});
            public {1} {3} {{ get {{ return {2}; }} }}"
            , varName
            , dataValue
            , varNameParsed
            , propName
            );
        }

        /// <summary>
        /// DateTimeデータの読み込み部分のソースコードを生成
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="propName"></param>
        /// <param name="varNameParsed"></param>
        /// <returns></returns>
        static public string CreateDateTimeDataCode(string varName, string propName, string varNameParsed)
        {
            return string.Format(@"
            [SerializeField]
            string {0} = """";

            [SerializeField]
            DateTime {1} = default(DateTime);
            public DateTime {2} {{ get {{ return {1}; }}}}
            "
            , varName
            , varNameParsed
            , propName
            );
        }

        /// <summary>
        /// DateTime文字列を型にシリアライズするコードを生成
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="varNameParsed"></param>
        /// <returns></returns>
        static public string CreateDateTimeSerializeCode(string varName, string varNameParsed)
        {
            return string.Format(@"
                {0} = DateTime.Parse({1});
            "
            , varNameParsed
            , varName
            );
        }

        /// <summary>
        /// Enum文字列を型にシリアライズするコードを生成
        /// </summary>
        /// <param name="varName"></param>
        /// <param name="varNameParsed"></param>
        /// <param name="dataValue"></param>
        /// <returns></returns>
        static public string CreateEnumSerializeCode(string varName, string varNameParsed, string dataValue)
        {
            return string.Format(@"
                {0} = ({1})Enum.Parse(typeof({1}), {2});
            "
            , varNameParsed
            , dataValue
            , varName);
        }
    }
} // namespace rc
