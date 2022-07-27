

using System;
using System.Collections.Generic;
using System.Linq;

//INamedBooleanTimeSwitchCollectionGet
[System.Serializable]
public class BooleanDateStateSwitchKeyClampImp 
{
    public List<BooleanDateStateSwitchKey> m_listRecentToPast = new List<BooleanDateStateSwitchKey>();
    public int m_maxKey = 1024;
    public bool m_whenCreatedValue;
    public DateTime m_whenCreatedDate;

    public BooleanDateStateSwitchKeyClampImp(int maxKey, bool startValue)
    {
        m_maxKey = maxKey;
        m_whenCreatedValue = startValue;
    }

    public BooleanDateStateSwitchKeyClampImp(int maxKey, bool startValue, DateTime now) : this(maxKey, startValue)
    {
        m_whenCreatedDate = now;
    }

    /**
     
    private void PushCantBeZeroExceptionIfNeeded()
    {
        if (m_listRecentToPast.Count == 0)
            throw new Exception("Can't be zero when accessing");
    }

     */


    /*
    
 

    


    public bool IsTrueOnRecentPart(in DateTime date, in BooleanDateStateSwitchKey target)
    {
        long d = date.Ticks;
        if (d >= target.GetWhenSwitchHappenedAsLong())
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

    

    public void GetLimitsOfStateAt(in DateTime date, out DateTime moreRecent, out DateTime moreOldest)
    {
        //please check beforeif date out of key
        PushCantBeZeroExceptionIfNeeded();
        GetLimitsOfStateAt(in date, out IBooleanDateStateSwitch recent, out IBooleanDateStateSwitch oldest);
        recent.GetWhenSwitchHappened(out moreRecent);
        oldest.GetWhenSwitchHappened(out moreOldest);
    }
    public void GetLimitsOfStateAt(in DateTime date, out IBooleanDateStateSwitch moreRecent, out IBooleanDateStateSwitch moreOldest)
    {
        // i<n<i+1
        //Recent < Date < Old
        //999 < 500 < 200

        PushCantBeZeroExceptionIfNeeded();
        if (m_listRecentToPast.Count == 0)
        {
            moreRecent = null;
            moreOldest = null;
            return;
        }
        if (m_listRecentToPast.Count == 1)
        {
            m_listRecentToPast[0].GetWhenSwitchHappened(out DateTime dt);
            if (date >= dt)
            {
                moreRecent = null;
                moreOldest = m_listRecentToPast[0];
            }
            else {
                moreRecent = m_listRecentToPast[0];
                moreOldest = null;
            }
            return;
        }
        if (IsBeforeFirstKeySwitch(in date))
        {
            moreRecent = null;
            moreOldest = GetFirstKey();
            return;

        }
        if (IsAfterLastKeySwitch(in date))
        {

            moreRecent = GetLastKey();
            moreOldest = null;
            return;
        }
        long t = date.Ticks;
        for (int i = 0; i < m_listRecentToPast.Count - 1; i++)
        {
            if (m_listRecentToPast[i].IsMoreOldThatKey(in t, in equalTrue)
                && m_listRecentToPast[i + 1].IsMoreRecentThatKey(in t, in equalTrue))
            {
                moreRecent = m_listRecentToPast[i];
                moreOldest = m_listRecentToPast[i + 1];
                return;
            }
        }
        moreRecent = null;
        moreOldest = null;
    }
    bool equalTrue = true;

    private bool IsAfterLastKeySwitch(in DateTime date)
    {
        GetLastKey().GetWhenSwitchHappened(out long tick);
        return date.Ticks < tick;
    }

    private BooleanDateStateSwitchKey GetFirstKey()
    {
        return m_listRecentToPast[0];
    }

    private BooleanDateStateSwitchKey GetLastKey()
    {
        return m_listRecentToPast[m_listRecentToPast.Count - 1];
    }

    private bool IsBeforeFirstKeySwitch(in DateTime date)
    {
        GetFirstKey().GetWhenSwitchHappened(out long tick);
        return date.Ticks > tick;
    }




    public void GetElapsedTimeInNanoSecondsAt(in DateTime date, out bool found, out long nanoSeconds, out DateTime switchEnd)
    {
        GetSegmentOldSideAt(in date, out found, out int index);
        if (!found)
            throw new Exception("No switch found");
        GetSegmentIndexAt(in index, out BooleanDateStateSwitchKey target);
        switchEnd = target.GetWhenSwitchHappenedAsDate();
        GetSegmentInfoOf(in date, out nanoSeconds, out bool state);
    }

    public void GetTrueFalseRatio(in DateTime start, in DateTime to, out double pourcentTrue)
    {
       
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
            if (m_listRecentToPast[i].GetWhenSwitchHappenedAsLong() > l)
            {
                m_listRecentToPast.Insert(i, new BooleanDateStateSwitchKey(dateToInject, newValue));
                return;
            }
        }
    }
    
    
    
    //*/


}