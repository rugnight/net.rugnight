using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rc.constants
{
#if UNITY_EDITOR
    /// <summary>
    /// エディタ拡張関連の定数
    /// </summary>
    static public class EditorExtension  {
        // エディタ拡張メニューのパスルート
        public const string TOOL_MENU_ROOT = "RC/";
    }
#endif//UTNIY_EDITOR
}
