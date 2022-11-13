using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rc
{
    static public class StringUtils
    {
        /// <summary>
        /// 先頭大文字に変換
        /// <returns></returns>
        static public string ToTitleCase(string str)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
        }

        /// <summary>
        /// 先頭小文字に変換
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public string ToUnTitleCase(string str)
        {
            var returnText = string.Empty;
            for (var i = 0; i < str.Length; i++)
            {
                if (i == 0) returnText += str[i].ToString().ToLower();
                else returnText += str[i];
            }

            return returnText;
        }
    }

} // namespace rc 
