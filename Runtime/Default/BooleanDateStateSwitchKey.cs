using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BooleanDateStateSwitchKey : IBooleanDateStateSwitch
{

    [SerializeField] long m_whenItSwithcAsDateTimeLong;
    [SerializeField] BooleanSwithType m_switch;

  
    public BooleanDateStateSwitchKey(DateTime whenItSwithcAsDateTimeLong, BooleanSwithType switchtype)
    {
        m_whenItSwithcAsDateTimeLong = whenItSwithcAsDateTimeLong.Ticks;
        m_switch = switchtype;
    }
    public BooleanDateStateSwitchKey(long whenItSwithcAsDateTimeLong, BooleanSwithType switchtype)
    {
        m_whenItSwithcAsDateTimeLong = whenItSwithcAsDateTimeLong;
        m_switch = switchtype;
    }
    public BooleanDateStateSwitchKey(DateTime whenItSwithcAsDateTimeLong, bool isTrue)
    {
        m_whenItSwithcAsDateTimeLong = whenItSwithcAsDateTimeLong.Ticks;
        m_switch = isTrue ? BooleanSwithType.FalseToTrue : BooleanSwithType.TrueToFalse;
    }
    public BooleanDateStateSwitchKey(long whenItSwithcAsDateTimeLong, bool isTrue)
    {
        m_whenItSwithcAsDateTimeLong = whenItSwithcAsDateTimeLong;
        m_switch = isTrue ? BooleanSwithType.FalseToTrue : BooleanSwithType.TrueToFalse;
    }

    public void GetSwitchType(out BooleanSwithType switchType) =>
        switchType = m_switch;
    public void GetWhenSwitchHappened(out long dateInTickTimeWhenSwitchHappened) =>
        dateInTickTimeWhenSwitchHappened = m_whenItSwithcAsDateTimeLong;
    public void GetWhenSwitchHappened(out DateTime whenSwitchHappened) =>
        whenSwitchHappened = new DateTime(m_whenItSwithcAsDateTimeLong);
    public BooleanSwithType GetSwitchType()
    {
        return m_switch;
    }
    public long WhenSwitchHappenedLong()
    {
        return m_whenItSwithcAsDateTimeLong;
    }
    public DateTime WhenSwitchHappenedDate()
    {
        return new DateTime(m_whenItSwithcAsDateTimeLong);
    }

    public void SetValue(long time) => m_whenItSwithcAsDateTimeLong = time;
    public void SetValue(DateTime time) => m_whenItSwithcAsDateTimeLong = time.Ticks;
    public void SetWitchType(BooleanSwithType switchType) => m_switch = switchType;
    public void SetWitchType(bool isTrue) => m_switch = isTrue ? BooleanSwithType.FalseToTrue : BooleanSwithType.TrueToFalse;

    public bool WasTrue()
    {
        return m_switch == BooleanSwithType.TrueToFalse;
    }
    public bool WasFalse()
    {
        return m_switch == BooleanSwithType.FalseToTrue;
    }

    public bool TurnedTrue()
    {
        return m_switch == BooleanSwithType.FalseToTrue;
    }

    public bool TurnedFalse()
    {
        return m_switch == BooleanSwithType.TrueToFalse;
    }
}