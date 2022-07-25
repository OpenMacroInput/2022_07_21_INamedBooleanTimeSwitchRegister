using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class TDD_DebugTimeSwitchRegisterAsTextWaveTime : MonoBehaviour
{
    public TDD_NamedBooleanTimeSwitch m_toDebug;
    [TextArea(0,5)]
    public string m_debugText;
    public StringEvent m_debugTextDisplay;
    [System.Serializable]
    public class StringEvent : UnityEvent<string> { } 

   
    public string m_falseToTrue= "▁";
    public string m_trueToFalse= "▔";
    public int m_charStopCheckPoint = 100;
    public float m_autoRefresh = 0.1f;

    public void Awake()
    {
        InvokeRepeating("Refresh", 0, m_autoRefresh);
    }

    //↓↕↑
    public void Refresh() {
        NamedBooleanTimeSwitchStringUtility
            .GetStringDescriptionByMilliscondsSegment(
            m_toDebug.m_register,
            in m_toDebug.m_namedBool,
            out m_debugText,
            in m_falseToTrue,
            in m_trueToFalse,
            in m_charStopCheckPoint
            );
        m_debugTextDisplay.Invoke(m_debugText);
    }
}
