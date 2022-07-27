

using System;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// If the methode need to have access to the list it is not generic.
/// </summary>
[System.Serializable]
public class AAA : BooleandDateSwitchCollectionDefault
{
    public List<BooleanDateStateSwitchKey> m_listRecentToPast = new List<BooleanDateStateSwitchKey>();
    public int m_maxKey = 1024;

    public AAA(int maxKey, bool startValue): base(startValue)
    {
        m_maxKey = maxKey;
    }

    public AAA(int maxKey, bool startValue, DateTime now) : base( startValue, now)
    {
        m_maxKey = maxKey;
    }

    public void SortCollection()
    {
        m_listRecentToPast = m_listRecentToPast.OrderByDescending(k => k.GetWhenSwitchHappenedAsLong()).ToList();
    }




    public override void GetSegmentInfoOf(in DateTime date, out bool stateIsTrue)
    {
        GetMostOldestKey(out IBooleanDateStateSwitch last);
        last.GetWhenSwitchHappened(out long lk);
        if (date.Ticks < lk)
        {
            stateIsTrue = last.WasTrue();
            return;
        }

        GetSegmentOldSideAt(in date, out bool found, out int index);

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


    public override void GetMostOldestKey(out IBooleanDateStateSwitch switchMostOld)
    {
        if (GetSwitchCount() == 0)
            PushWarningToHaveAtListOneKeyIsNeeded();
        switchMostOld = m_listRecentToPast[m_listRecentToPast.Count - 1];
    }

    public override void GetMostRecentKey(out IBooleanDateStateSwitch mostRecent)
    {
        if (GetSwitchCount() == 0)
            PushWarningToHaveAtListOneKeyIsNeeded();
        mostRecent = m_listRecentToPast[0];
    }

    public void GetSergmentInfoOf(in int indexOld, out long tickTime, out bool state)
    {
        GetSegmentInfoOf(indexOld - 1, indexOld, out tickTime, out state);
    }
    public void GetSegmentInfoOf(in int IndexRecent, in int indexOld, out long tickTime, out bool state)
    {
        GetSegmentInfoOf(m_listRecentToPast[IndexRecent], m_listRecentToPast[indexOld], out tickTime, out state);
    }
    public void GetSegmentIndexAt(in int index, out BooleanDateStateSwitchKey target)
    {
        target = m_listRecentToPast[index];
    }

    public void GetSegmentOldSideAt(in DateTime atDate, out bool found, out int index)
    {

        index = -1;
        found = false;
        if (GetSwitchCount() == 0) { found = false; }
        else if (IsAfterLastKeySwitch(in atDate))
        { }
        else if (IsBeforeFirstKeySwitch(in atDate))
        {
            index = 0;
            found = true;
        }
        else
        {
            for (int i = 0; i < GetSwitchCount() - 1; i++)
            {
                if (m_listRecentToPast[i].IsMoreOldThatKey(in atDate, in equalFalse)
                    && m_listRecentToPast[i + 1].IsMoreRecentThatKey(in atDate, in equalTrue))
                {
                    index = i + 1;
                    found = true;
                    break;
                }
            }
        }
    }
    public void GetSegmentOldSideAt(in DateTime atDate, out bool found, out int index, out IBooleanDateStateSwitch target)
    {
        GetSegmentOldSideAt(in atDate, out found, out index);
        if (!found || index < 0)
            target = null;
        else target = m_listRecentToPast[index];
    }
    public void GetSegmentRecentSideAt(in DateTime atDate, out bool found, out int index)
    {
        index = -1;
        found = false;
        if (GetSwitchCount() == 0) { found = false; }

        else if (IsAfterLastKeySwitch(in atDate))
        {
            index = m_listRecentToPast.Count - 1;
            found = true;
        }
        else if (IsBeforeFirstKeySwitch(in atDate))
        { }
        else
        {
            for (int i = 0; i < GetSwitchCount() - 1; i++)
            {
                if (m_listRecentToPast[i].IsMoreOldThatKey(in atDate, in equalFalse)
                    && m_listRecentToPast[i + 1].IsMoreRecentThatKey(in atDate, in equalTrue))
                {
                    index = i;
                    found = true;
                    break;
                }
            }
        }
    }
    public void GetSegmentRecentSideAt(in DateTime atDate, out bool found, out int index, out IBooleanDateStateSwitch target)
    {
        GetSegmentRecentSideAt(in atDate, out found, out index);
        if (!found || index < 0)
            target = null;
        else target = m_listRecentToPast[index];
    }
    public void GetSegmentOldSideAt(in DateTime atDate, out bool found, out int index, out BooleanDateStateSwitchKey target)
    {
        GetSegmentOldSideAt(in atDate, out found, out index);
        if (!found)
        {
            target = null;
            return;
        }
        target = m_listRecentToPast[index];
    }
    public void GetSegmentInfoOf(in DateTime date, out long tickTimeOne, out bool state)
    {
        GetSegmentOldSideAt(in date, out bool found, out int index);

        if (index > 1)
        {
            GetSegmentIndexAt(in index, out BooleanDateStateSwitchKey target);
            GetSegmentIndexAt(index - 1, out BooleanDateStateSwitchKey inFrontRecentOne);
            tickTimeOne = inFrontRecentOne.GetWhenSwitchHappenedAsLong() - target.GetWhenSwitchHappenedAsLong();
            state = target.GetSwitchType() == BooleanSwithType.TrueToFalse;
            return;
        }
        else if (index == 1)
        {
            GetSegmentIndexAt(in index, out BooleanDateStateSwitchKey target);
            tickTimeOne = (DateTime.Now.Ticks - target.GetWhenSwitchHappenedAsLong());
            state = target.GetSwitchType() == BooleanSwithType.TrueToFalse;
            return;
        }
        throw new Exception("Cant use if zero key");
    }

    public override void GetSwitchCount(out int count) => count = m_listRecentToPast.Count;

    public override void GetSegmentOldSwitchSideAt(in DateTime atDate, out IBooleanDateStateSwitch switchKeyOld)
    {
        GetSegmentOldSideAt(in atDate, out bool found, out int index, out switchKeyOld);
    }

    public override void GetSegmentRecentSwitchSideAt(in DateTime atDate, out IBooleanDateStateSwitch switchKeyRecent)
    {
        GetSegmentRecentSideAt(in atDate, out bool found, out int index, out switchKeyRecent);
    }


    #region SET
    public override void SetNow(in bool isTrue)
    {
        SetNowWhenActionHappened(DateTime.Now, isTrue);
    }

    public override void SetNowWhenActionHappened(in DateTime whenItHappened, in bool isTrue)
    {


        DateTime now = DateTime.Now;
        if (GetSwitchCount() == 0)
        {
            m_listRecentToPast.Add(new BooleanDateStateSwitchKey(now, isTrue));
            return;
        }
        GetStateNow(out bool currentState);
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


    public override void SetPastSwitchManually(in DateTime atDate, in bool newValue)
    {
        int c = GetSwitchCount();
        GetMostRecentKey(out IBooleanDateStateSwitch r);
        long l = r.GetWhenSwitchHappenedAsLong();
        if (c == 0)
        {
            SetNowWhenActionHappened(atDate, newValue);
            return;
        }
        for (int i = 0; i < c; i++)
        {
            if (m_listRecentToPast[i].GetWhenSwitchHappenedAsLong() > l)
            {
                m_listRecentToPast.Insert(i,
                    new BooleanDateStateSwitchKey(atDate, newValue));
                return;
            }
        }
    }

    public override void GetAllSwitchKeyBetween(in DateTime from, in DateTime to, out IBooleanDateStateSwitch[] sample)
    {
        DateTimeSwitchUtility.GetDateLong(in from, in to, out long recentLong, out long oldLong);
        sample = m_listRecentToPast
           .Where(k => k.GetWhenSwitchHappenedAsLong() <= recentLong && k.GetWhenSwitchHappenedAsLong() >= oldLong)
           .OrderByDescending(k => k.GetWhenSwitchHappenedAsLong()).ToArray();
    }

    public override void GetAllSwitchKeyInCollection(out IBooleanDateStateSwitch[] sample)
    {
        sample = m_listRecentToPast.ToArray();
    }
    #endregion
}