#define OPEN_TERMINAL_POWERSHELL
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Rc
{
    public class OpenTerminal 
    {
        [MenuItem("Window/Run Terminal %t")]
        static void RunTerminal()
        {
            Process p = new Process();
#if OPEN_TERMINAL_POWERSHELL
            p.StartInfo.FileName = "powershell";
#else
            p.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
            p.StartInfo.Arguments = "/k cd Assets";
#endif
            p.Start();
        }
    }
}
