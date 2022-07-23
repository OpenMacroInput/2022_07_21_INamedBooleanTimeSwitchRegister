using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefaultNamedBooleanTimeSwitchRegister : INamedBooleanTimeSwitchRegister
{
    public int m_maxKeyPerList=1024;
    public Dictionary<string, BooleanDateStateSwitchClampList> m_register = new Dictionary<string, BooleanDateStateSwitchClampList>();

    public void CreateSlot(in string namedBoolean, in bool startValue)
    {
        if (!m_register.ContainsKey(namedBoolean)) { 
            m_register.Add(namedBoolean, new BooleanDateStateSwitchClampList() { m_maxKey= m_maxKeyPerList });
            m_register[namedBoolean].SetWithNow(startValue);
        }
    }
    public bool IsContaining(in string namedBoolean) { return m_register.ContainsKey(namedBoolean); }
    public bool IsNotContaining(in string namedBoolean) { return !m_register.ContainsKey(namedBoolean); }

    public void GetAllSwitchDateBetween(in DateTime start, in DateTime to, in string namedboolean, out IBooleanDateStateSwitch[] sample)
    {
        if (IsNotContaining(in namedboolean))
        {
            sample = new IBooleanDateStateSwitch[0];
            return;
        }
        else {
            m_register[namedboolean].GetAllSwitchDateBetween(in start, in to, out  sample);
        }
    }


    public void GetLimitesSwitchOfBoolean(in string namedboolean, out IBooleanDateStateSwitch oldest, out IBooleanDateStateSwitch mostRecent)
    {
        oldest = null;
        mostRecent = null;
        if (IsNotContaining(in namedboolean))
            return;
        m_register[namedboolean].GetMostRecent(out BooleanDateStateSwitch recent);
        m_register[namedboolean].GetMostFar(out BooleanDateStateSwitch old);
        oldest = old;
        mostRecent = recent;
    }

   

    public void GetStateNow(in string namedboolean, out bool existing, out bool state)
    {
        if (IsNotContaining(in namedboolean))
        {
            existing = false;
            state = false;
        }
        else {
            existing = true;
            state= m_register[namedboolean].GetState();
        }
    }

   
    public void SetNow(in string namedBoolean, in bool value, in bool createIfNotExisting)
    {
        if (IsContaining(in namedBoolean))
        {
            m_register[namedBoolean].SetWithNow(value);
        }
        else if (createIfNotExisting) {
            CreateSlot(in namedBoolean, in value);
            m_register[namedBoolean].SetWithNow(value);

        }
    }
    public void SetNowWhenActionHappened(in DateTime whenItHappened, in string namedBoolean, in bool value, in bool createIfNotExisting)
    {
        m_register[namedBoolean].SetWithNow(whenItHappened, value);
    }

    public void IsBooleanRegistered(in DateTime date, in string namedboolean, out bool existing)
    {
        throw new NotImplementedException();
    }

    public void HasAnySwitchHappened(in DateTime date, in string namedboolean, out bool hasSomeRecord)
    {
        throw new NotImplementedException();
    }

    public void GetSwitchCount(in DateTime date, in string namedboolean, out int switchCount)
    {
        throw new NotImplementedException();
    }

    public bool IsDateTimeToFarInTime(in DateTime date)
    {
        throw new NotImplementedException();
    }

    public bool WasTrueAt(in DateTime date, in string namedboolean)
    {
        throw new NotImplementedException();
    }

    public bool WasFalseAt(in DateTime date, in string namedboolean)
    {
        throw new NotImplementedException();
    }

    public void GetSwitchCount(in DateTime start, in DateTime to, out bool startValue, out bool endValue, out int switchToTrue, out int switchToFalse)
    {
        throw new NotImplementedException();
    }

    public void GetStateNow(in string namedboolean, out bool state)
    {
        throw new NotImplementedException();
    }

    public void GetStateAt(in DateTime date, in string namedboolean, out bool state)
    {
        throw new NotImplementedException();
    }

    public void GetLimitsOfStateAt(in DateTime date, in string namedboolean, out DateTime switchStart, out DateTime switchEnd)
    {
        throw new NotImplementedException();
    }

    public void GetElapsedTimeInNanoSecondsAt(in DateTime date, in string namedboolean, out long nanoSeconds, in DateTime switchEnd)
    {
        throw new NotImplementedException();
    }

    public void GetTrueFalseRatio(in DateTime start, in DateTime to, in string namedboolean, double pourcentTrue)
    {
        throw new NotImplementedException();
    }

    public void GetTrueFalseTimeInNanoseconds(in DateTime start, in DateTime to, in string namedboolean, out long nanoSecondTrue, out long nanoSecondFalse)
    {
        throw new NotImplementedException();
    }

    public void SetPastSwitchManually(in DateTime previousDate, in string namedBoolean, in bool newValue, in bool createIfNotExisting)
    {
        throw new NotImplementedException();
    }
}


public class BooleanDateStateSwitchClampList {
    public List<BooleanDateStateSwitch> m_listRecentToPast = new List<BooleanDateStateSwitch>();
    public int m_maxKey = 1024;

    public void SetWithNow(bool isTrue) {
        SetWithNow(DateTime.Now, isTrue);
    }

    public void SetWithNow( DateTime date, bool isTrue) {
        DateTime now = DateTime.Now;
        if (m_listRecentToPast.Count == 0)
        {
            m_listRecentToPast.Add(new BooleanDateStateSwitch(now, isTrue));
        }
        else if (m_listRecentToPast.Count >= m_maxKey)
        {
            BooleanDateStateSwitch reused = m_listRecentToPast[m_listRecentToPast.Count - 1];
            m_listRecentToPast.RemoveAt(m_listRecentToPast.Count - 1);
            reused.SetValue(now);
            reused.SetWitchType(isTrue);
            m_listRecentToPast.Insert(0, reused);
        }
        else {
            m_listRecentToPast.Insert(0 , new BooleanDateStateSwitch(now, isTrue) );
        }
    }

    public void GetCount(out int count) => count = m_listRecentToPast.Count;
    public void GetMostRecent(out BooleanDateStateSwitch first) {
        if (m_listRecentToPast.Count == 0)
            throw new Exception("");
        first = m_listRecentToPast[0];
    
    }
    public void GetMostFar(out BooleanDateStateSwitch last) {
        {
            if (m_listRecentToPast.Count == 0)
                throw new Exception("");
            last = m_listRecentToPast[m_listRecentToPast.Count - 1];
    }
}

    public void GetAllSwitchDateBetween(in DateTime from, in DateTime to, out IBooleanDateStateSwitch[] sample)
    {
        long s = from.Ticks,  t = to.Ticks;
        if (s < t)
        {
            long tmp = s;
            s = t;
            t = tmp;
        }
        sample = m_listRecentToPast
            .Where(k => k.WhenSwitchHappenedLong() >= s && k.WhenSwitchHappenedLong() <= t)
            .OrderBy(k => k.WhenSwitchHappenedLong()).ToArray();
    }
    internal bool GetState()
    {
        PushCantBeZeroIfNeeded();
        return m_listRecentToPast[0].IsTrue();

    }

    private void PushCantBeZeroIfNeeded()
    {
        if (m_listRecentToPast.Count == 0)
            throw new Exception("Can't be zero when accessing");
    }
}
[System.Serializable]
public class BooleanDateStateSwitch : IBooleanDateStateSwitch
{
    public void SetValue(long time) => m_whenItSwithcAsDateTimeLong = time;
    public void SetValue(DateTime time) => m_whenItSwithcAsDateTimeLong = time.Ticks;
    public void SetWitchType(BooleanSwithType switchType) => m_switch = switchType;
    public void SetWitchType(bool isTrue) => m_switch = isTrue ? BooleanSwithType.FalseToTrue : BooleanSwithType.TrueToFalse;

    public BooleanDateStateSwitch(DateTime whenItSwithcAsDateTimeLong, BooleanSwithType switchtype)
    {
        m_whenItSwithcAsDateTimeLong = whenItSwithcAsDateTimeLong.Ticks;
        m_switch = switchtype;
    }
    public BooleanDateStateSwitch(long whenItSwithcAsDateTimeLong, BooleanSwithType switchtype)
    {
        m_whenItSwithcAsDateTimeLong = whenItSwithcAsDateTimeLong;
        m_switch = switchtype;
    }
    public BooleanDateStateSwitch(DateTime whenItSwithcAsDateTimeLong, bool isTrue)
    {
        m_whenItSwithcAsDateTimeLong = whenItSwithcAsDateTimeLong.Ticks;
        m_switch = isTrue ? BooleanSwithType.FalseToTrue : BooleanSwithType.TrueToFalse;
    }
    public BooleanDateStateSwitch(long whenItSwithcAsDateTimeLong, bool isTrue)
    {
        m_whenItSwithcAsDateTimeLong = whenItSwithcAsDateTimeLong;
        m_switch = isTrue ? BooleanSwithType.FalseToTrue : BooleanSwithType.TrueToFalse;
    }

    [SerializeField] long m_whenItSwithcAsDateTimeLong;
    [SerializeField] BooleanSwithType m_switch;
    public void GetBooleanStateSwitchType(out BooleanSwithType switchType) =>
        switchType = m_switch;
    public void WhenSwitchHappened(out long dateInTickTimeWhenSwitchHappened) =>
        dateInTickTimeWhenSwitchHappened = m_whenItSwithcAsDateTimeLong;
    public void WhenSwitchHappened(out DateTime whenSwitchHappened) =>
        whenSwitchHappened = new DateTime(m_whenItSwithcAsDateTimeLong);
    public BooleanSwithType GetBooleanStateSwitchType()
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

    public bool IsTrue()
    {
        return m_switch == BooleanSwithType.FalseToTrue;
    }
    public bool IsFalse()
    {
        return m_switch == BooleanSwithType.TrueToFalse;
    }
}