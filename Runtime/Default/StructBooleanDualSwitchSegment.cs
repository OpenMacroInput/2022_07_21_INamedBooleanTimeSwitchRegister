using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructBooleanDualSwitchSegment : IBooleanDateStateDualSwitchSegment
{
    public IBooleanDateStateSwitch m_mostRecent;
    public IBooleanDateStateSwitch m_mostOld;
    public void GetDurationInMilliseconds(out int millisecondsMax24Day)
    {
        GetDurationInNanoseconds(out long nano);
        millisecondsMax24Day = (int)(nano / 1000000.0);
    }
    public void GetDurationInMilliseconds(out double milliseconds)
    {
        GetDurationInNanoseconds(out long nano);
        milliseconds = nano / 1000000.0;
    }
    public void GetDurationInNanoseconds(out long duration)
    {
        m_mostRecent.GetWhenSwitchHappened(out long recent);
        m_mostOld.GetWhenSwitchHappened(out long old);
        duration = recent - old;
    }
    public void GetSwitchMostOlder(out IBooleanDateStateSwitch switchTypeRecent)
    {
        switchTypeRecent = m_mostRecent;
    }

    public void GetSwitchMostRecent(out IBooleanDateStateSwitch switchTypeOld)
    {
        switchTypeOld = m_mostOld;
    }

    public bool WasTrueDuringDuration()
    {
       return m_mostOld.TurnedTrue();
    }
    public bool IsTrueAfterSwitches()
    {
        return m_mostRecent.TurnedTrue();
    }
    public bool IsTrueBeforeSwitches()
    {
        return m_mostOld.WasTrue();
    }
}
