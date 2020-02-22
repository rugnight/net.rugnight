using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace rc
{
    public class OpenTerminal 
    {
        [MenuItem("Window/Run Terminal %t")]
        static void RunTerminal()
        {
            Process p = new Process();
            p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
            p.StartInfo.Arguments = "/k cd Assets";
            p.Start();
        }
    }
}
