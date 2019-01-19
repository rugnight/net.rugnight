using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBehaviour<Type> : MonoBehaviour where Type : MonoBehaviour
{
    static Type m_self = null;
    static public Type Instance {
        get {
            if (m_self == null)
            {
                m_self = FindObjectOfType<Type>();
            }
            return m_self;
        }
    }
}
