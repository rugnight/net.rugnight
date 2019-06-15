using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rc
{
    /// <summary>
    /// 正当性確認
    /// </summary>
    static public class Validation
    {
        /// <summary>
        /// 小数かどうかを判別
        /// </summary>
        /// <param name="dValue"></param>
        /// <returns></returns>
        public static bool IsDecimal(double dValue)
        {
            if (dValue - System.Math.Floor(dValue) != 0)
            {
                return true;
            }

            return false;
        }

    }
}
