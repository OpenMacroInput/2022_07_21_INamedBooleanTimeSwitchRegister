

using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class BooleanDateStateSwitchKeyClampList
{
    public List<BooleanDateStateSwitchKey> m_listRecentToPast = new List<BooleanDateStateSwitchKey>();
    public int m_maxKey = 1024;
    public bool m_whenCreatedValue;
    public DateTime m_whenCreatedDate;

    public BooleanDateStateSwitchKeyClampList(int maxKey, bool startValue)
    {
        m_maxKey = maxKey;
        m_whenCreatedValue = startValue;
    }

    public BooleanDateStateSwitchKeyClampList(int maxKey, bool startValue, DateTime now) : this(maxKey, startValue)
    {
        m_whenCreatedDate = now;
    }


    public void SetWithNow(bool isTrue)
    {
        SetWithNow(DateTime.Now, isTrue);
    }

    public void SetWithNow(DateTime date, bool isTrue)
    {
        DateTime now = DateTime.Now;
        if (m_listRecentToPast.Count == 0)
        {
            m_listRecentToPast.Add(new BooleanDateStateSwitchKey(now, isTrue));
            return;
        }
        GetState(out bool currentState);
        if (currentState == isTrue)
            return;

        if (m_listRecentToPast.Count >= m_maxKey)
        {
            BooleanDateStateSwitchKey reused = m_listRecentToPast[m_listRecentToPast.Count - 1];
            m_listRecentToPast.RemoveAt(m_listRecentToPast.Count - 1);
            reused.SetValue(now);
            reused.SetWitchType(isTrue);
            m_listRecentToPast.Insert(0, reused);
        }
        else
        {
            m_listRecentToPast.Insert(0, new BooleanDateStateSwitchKey(now, isTrue));
        }
    }

    public void GetSwitchCount(out int count) => count = m_listRecentToPast.Count;
    public int GetSwitchCount() { return m_listRecentToPast.Count; }
    public void GetMostRecent(out BooleanDateStateSwitchKey first)
    {
        if (m_listRecentToPast.Count == 0)
            throw new Exception("");
        first = m_listRecentToPast[0];

    }
    public void GetMostFarInTime(out BooleanDateStateSwitchKey last)
    {
        {
            if (m_listRecentToPast.Count == 0)
                throw new Exception("");
            last = m_listRecentToPast[m_listRecentToPast.Count - 1];
        }
    }

    public void GetAllSwitchDateBetween(in DateTime recentDate, in DateTime oldDate, out IBooleanDateStateSwitch[] sample)
    {
        long recentLong = recentDate.Ticks, oldLong = oldDate.Ticks;
        if (recentLong < oldLong)
        {
            long tmp = recentLong;
            recentLong = oldLong;
            oldLong = tmp;
        }
        sample = m_listRecentToPast
            .Where(k => k.WhenSwitchHappenedLong() <= recentLong && k.WhenSwitchHappenedLong() >= oldLong)
            .OrderByDescending(k => k.WhenSwitchHappenedLong()).ToArray();
    }
    public void GetAllSwitchDateInMemory(out IBooleanDateStateSwitch[] sample)
    {
        sample = m_listRecentToPast.ToArray();
    }


    public void GetStateWhenCreated(out bool isTrueValueAtStartExisting)
    {
        isTrueValueAtStartExisting = m_whenCreatedValue;
    }

    public void GetAllSwitchDateBetween(in DateTime from, in DateTime to, out BooleanDateStateSwitchKey[] sample)
    {
        long recentLong = from.Ticks, oldLong = to.Ticks;
        if (recentLong < oldLong)
        {
            long tmp = recentLong;
            recentLong = oldLong;
            oldLong = tmp;
        }
        sample = m_listRecentToPast
            .Where(k => k.WhenSwitchHappenedLong() <= recentLong && k.WhenSwitchHappenedLong() >= oldLong)
            .OrderByDescending(k => k.WhenSwitchHappenedLong()).ToArray();
    }

    internal void GetDateWhenCreated(out DateTime dateAtStartExisting)
    {
        dateAtStartExisting = m_whenCreatedDate;
    }

    public void GetState(out bool startValue)
    {
        PushCantBeZeroExceptionIfNeeded();
        startValue = m_listRecentToPast[0].TurnedTrue();

    }
    public bool GetState()
    {
        PushCantBeZeroExceptionIfNeeded();
        return m_listRecentToPast[0].TurnedTrue();

    }
    public void GetOldestState(out bool endValue)
    {
        PushCantBeZeroExceptionIfNeeded();
        endValue = m_listRecentToPast[m_listRecentToPast.Count - 1].TurnedTrue();
    }

    private void PushCantBeZeroExceptionIfNeeded()
    {
        if (m_listRecentToPast.Count == 0)
            throw new Exception("Can't be zero when accessing");
    }


    public bool IsTrueOnRecentPart(in DateTime date, in BooleanDateStateSwitchKey target)
    {
        long d = date.Ticks;
        if (d >= target.WhenSwitchHappenedLong())
        {
            target.GetSwitchType(out BooleanSwithType t);
            return t == BooleanSwithType.TrueToFalse;

        }
        else
        {
            target.GetSwitchType(out BooleanSwithType t);
            return !(t == BooleanSwithType.TrueToFalse);
        }
    }

    public void GetStateAt(in DateTime date, out bool state)
    {
        PushCantBeZeroExceptionIfNeeded();
        long d = date.Ticks;
        if (m_listRecentToPast.Count() == 1)
        {
            BooleanDateStateSwitchKey val = m_listRecentToPast[0];
            state = IsTrueOnRecentPart(in date, in val);
        }
        else
        {
            GetLimitsOfStateAt(in date, out BooleanDateStateSwitchKey recent, out BooleanDateStateSwitchKey oldest);
            oldest.GetSwitchType(out BooleanSwithType type);
            state = type == BooleanSwithType.FalseToTrue;
        }

    }

    public void GetLimitsOfStateAt(in DateTime date, out DateTime moreRecent, out DateTime moreOldest)
    {
        //please check beforeif date out of key
        PushCantBeZeroExceptionIfNeeded();
        GetLimitsOfStateAt(in date, out BooleanDateStateSwitchKey recent, out BooleanDateStateSwitchKey oldest);
        moreRecent = recent.WhenSwitchHappenedDate();
        moreOldest = oldest.WhenSwitchHappenedDate();
    }
    public void GetLimitsOfStateAt(in DateTime date, out BooleanDateStateSwitchKey moreRecent, out BooleanDateStateSwitchKey moreOldest)
    {
        // i<n<i+1
        //Recent < Date < Old
        //999 < 500 < 200

        PushCantBeZeroExceptionIfNeeded();
        if (m_listRecentToPast.Count == 1)
        {
            moreRecent = m_listRecentToPast[0];
            moreOldest = m_listRecentToPast[0];
            return;
        }

        long t = date.Ticks;
        for (int i = 0; i < m_listRecentToPast.Count - 1; i++)
        {
            if (t <= m_listRecentToPast[i].WhenSwitchHappenedLong()
                && t > m_listRecentToPast[i + 1].WhenSwitchHappenedLong())
            {
                moreRecent = m_listRecentToPast[i];
                moreOldest = m_listRecentToPast[i + 1];
                return;
            }
        }
        moreRecent = m_listRecentToPast[m_listRecentToPast.Count - 1];
        moreOldest = m_listRecentToPast[m_listRecentToPast.Count - 1];
    }

    public void IsTimeMoreRecentThatKeys(in DateTime time, out bool mostRecent)
    {
        PushCantBeZeroExceptionIfNeeded();
        mostRecent = time.Ticks > m_listRecentToPast[0].WhenSwitchHappenedLong();
    }
    public void IsTimeMoreOlderThatKeys(in DateTime time, out bool mostOlder)
    {
        PushCantBeZeroExceptionIfNeeded();
        mostOlder = time.Ticks < m_listRecentToPast[m_listRecentToPast.Count - 1].WhenSwitchHappenedLong();
    }

    public void SortCollection()
    {
        m_listRecentToPast = m_listRecentToPast.OrderByDescending(k => k.WhenSwitchHappenedLong()).ToList();
    }


    public void GetElapsedTimeInNanoSecondsAt(in DateTime date, out bool found, out long nanoSeconds, out DateTime switchEnd)
    {
        GetSegmentAt(in date, out found, out int index);
        if (!found)
            throw new Exception("No switch found");
        GetSegmentIndexAt(in index, out BooleanDateStateSwitchKey target);
        switchEnd = target.WhenSwitchHappenedDate();
        GetSegmentInfoOf(in date, out nanoSeconds, out bool state);
    }

    public void GetTrueFalseRatio(in DateTime start, in DateTime to, out double pourcentTrue)
    {
        GetTrueFalseTimeInTickRef(start, to, out long nanoTrue, out long nanoFalse, out long total);
        pourcentTrue = ((double)nanoTrue) / ((double)total);
    }

    public void GetTrueFalseTimeInTickRef(DateTime startRecent, DateTime toOlder,
         out long nanoSecondTrue, out long nanoSecondFalse,
         out long nanoSecondsTotalObserved)
    {

        if (startRecent < toOlder)
        {
            DateTime tmp = startRecent;
            startRecent = toOlder;
            toOlder = tmp;
        }

        GetAllSwitchDateBetween(in startRecent, in toOlder,  out IBooleanDateStateSwitch[] sample);
        long startRecentLong = startRecent.Ticks;
        long startOldLong = toOlder.Ticks;
        nanoSecondsTotalObserved = startRecentLong - startOldLong;
        nanoSecondTrue = nanoSecondsTotalObserved;


        for (int i = 1; i < sample.Length - 1; i++)
        {
            GetSegmentInfoOf(in sample[i], in sample[i + 1], out long tickTime, out bool isTrueState);
            if (!isTrueState)
            {
                nanoSecondTrue -= tickTime;
            }
        }
        if (sample.Length > 0)
        {
            IBooleanDateStateSwitch start = sample[0];
            if (sample[0].TurnedFalse())
            {
                sample[0].GetWhenSwitchHappened(out long l);
                nanoSecondTrue -= startRecent.Ticks - l;
            }
            if (sample[sample.Length - 1].WasFalse())
            {
                sample[sample.Length - 1].GetWhenSwitchHappened(out long l);
                nanoSecondTrue -= l - toOlder.Ticks;
            }
            nanoSecondFalse = nanoSecondsTotalObserved - nanoSecondTrue;
        }
        else
        {
            nanoSecondsTotalObserved = startRecentLong - startOldLong;
            DateTime d =new DateTime( startOldLong + nanoSecondsTotalObserved / 2);
            GetStateAt(in d, out bool state);
            nanoSecondTrue = state ? nanoSecondsTotalObserved : 0;
            nanoSecondFalse = state ? 0 : nanoSecondsTotalObserved;
        }
    }

    public void SetPastSwitchManually(in DateTime dateToInject, in bool newValue)
    {
        int c = m_listRecentToPast.Count;
        long l = dateToInject.Ticks;
        if (c == 0)
        {
            SetWithNow(dateToInject, newValue);
            return;
        }
        for (int i = 0; i < c; i++)
        {
            if (m_listRecentToPast[i].WhenSwitchHappenedLong() > l)
            {
                m_listRecentToPast.Insert(i, new BooleanDateStateSwitchKey(dateToInject, newValue));
                return;
            }
        }
    }
    public bool WasTrueAt(in DateTime date)
    {
        GetSegmentInfoOf(in date, out bool state);
        return state;
    }

    public bool WasFalseAt(in DateTime date)
    {
        GetSegmentInfoOf(in date, out bool state);
        return !state;
    }
    public void GetSegmentInfoOf(in DateTime date, out bool stateIsTrue)
    {
        GetMostFarInTime(out BooleanDateStateSwitchKey last);
        last.GetWhenSwitchHappened(out long lk);
        if (date.Ticks < lk) {
            stateIsTrue = last.WasTrue();
            return;
        }

        GetSegmentAt(in date, out bool found, out int index);

        if (index > 1)
        {
            GetSegmentIndexAt(in index, out BooleanDateStateSwitchKey target);
            stateIsTrue = target.GetSwitchType() == BooleanSwithType.TrueToFalse;
            return;
        }
        else if (index == 1)
        {
            GetSegmentIndexAt(in index, out BooleanDateStateSwitchKey target);
            stateIsTrue = target.GetSwitchType() == BooleanSwithType.TrueToFalse;
            return;
        }
        throw new Exception("Cant use if zero key");
    }

    public void GetSergmentInfoOf(in int indexOld, out long tickTime, out bool state)
    {
        GetSegmentInfoOf(indexOld - 1, indexOld, out tickTime, out state);
    }
    public void GetSegmentInfoOf(in int IndexRecent, in int indexOld, out long tickTime, out bool state)
    {
        GetSegmentInfoOf(m_listRecentToPast[IndexRecent], m_listRecentToPast[indexOld], out tickTime, out state);
    }
    public void GetSegmentInfoOf(in IBooleanDateStateSwitch recent, in IBooleanDateStateSwitch old, out long tickTime, out bool state)
    {
        recent.GetWhenSwitchHappened(out long rl); 
        old.GetWhenSwitchHappened(out long ol);
        tickTime =  rl - ol;
        old.GetSwitchType(out BooleanSwithType sw);
        state = sw == BooleanSwithType.FalseToTrue;
    }

    public void GetSegmentInfoOf(in DateTime date, out long tickTimeOne, out bool state)
    {
        GetSegmentAt(in date, out bool found, out int index);

        if (index > 1)
        {
            GetSegmentIndexAt(in index, out BooleanDateStateSwitchKey target);
            GetSegmentIndexAt(index - 1, out BooleanDateStateSwitchKey inFrontRecentOne);
            tickTimeOne = inFrontRecentOne.WhenSwitchHappenedLong() - target.WhenSwitchHappenedLong();
            state = target.GetSwitchType() == BooleanSwithType.TrueToFalse;
            return;
        }
        else if (index == 1)
        {
            GetSegmentIndexAt(in index, out BooleanDateStateSwitchKey target);
            tickTimeOne = (DateTime.Now.Ticks - target.WhenSwitchHappenedLong());
            state = target.GetSwitchType() == BooleanSwithType.TrueToFalse;
            return;
        }
        throw new Exception("Cant use if zero key");
    }

    public void GetSegmentIndexAt(in int index, out BooleanDateStateSwitchKey target)
    {
        target = m_listRecentToPast[index];
    }


    public void GetSegmentAt(in DateTime date, out bool foundMatch, out int indexOldSide)
    {
        long t = date.Ticks;
        if (m_listRecentToPast.Count == 0)
        {
            foundMatch = false;
            indexOldSide = -1;
            return;
        }
        else if (m_listRecentToPast.Count == 1)
        {
            if (t >= m_listRecentToPast[0].WhenSwitchHappenedLong())
            {
                foundMatch = true;
                indexOldSide = 0;
                return;
            }
            else
            {
                foundMatch = false;
                indexOldSide = -1;
                return;
            }
        }
        else
        {
            if (t >= m_listRecentToPast[0].WhenSwitchHappenedLong())
            {
                foundMatch = true;
                indexOldSide = 0;
                return;
            }
            for (int i = 0; i < m_listRecentToPast.Count - 1; i++)
            {
                if (t <= m_listRecentToPast[i].WhenSwitchHappenedLong()
                    && t >= m_listRecentToPast[i + 1].WhenSwitchHappenedLong())
                {
                    foundMatch = true;
                    indexOldSide = i + 1;
                    return;
                }
            }
        }
        foundMatch = false;
        indexOldSide = -1;
        return;
    }
    public void GetSegmentRecentSideAt(in DateTime atDate, out bool found, out int index, out BooleanDateStateSwitchKey target)
    {
        GetSegmentAt(in atDate, out bool foundOld, out int indexOld);
        index = indexOld - 1;
        if (!foundOld || index < 0 ) {
            found = false;
            target = null;
            return;
        }
        found = true;
        target = m_listRecentToPast[index];

    }
    public void GetSegmentOldSideAt(in DateTime atDate, out bool found, out int index, out BooleanDateStateSwitchKey target)
    {
        GetSegmentAt(in atDate, out found, out  index);
        if (!found) { 
            target = null;
            return;
        }
        target = m_listRecentToPast[index];
    }



    internal void SetStartValue(bool startValue)
    {
        throw new NotImplementedException();
    }

}