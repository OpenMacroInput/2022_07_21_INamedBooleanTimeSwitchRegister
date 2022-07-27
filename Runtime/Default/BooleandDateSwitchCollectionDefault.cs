

using System;
using System.Collections.Generic;
using System.Linq;

//INamedBooleanTimeSwitchCollectionGet
[System.Serializable]
public abstract class BooleandDateSwitchCollectionDefault: INamedBooleanTimeSwitchCollection
{



    public bool m_whenCreatedValue;
    public DateTime m_whenCreatedDate;
    public BooleandDateSwitchCollectionDefault(bool startValue)
    {
        m_whenCreatedValue = startValue;
    }

    public BooleandDateSwitchCollectionDefault( bool startValue, DateTime now) : this( startValue)
    {
        m_whenCreatedDate = now;
    }

    #region Not In Interface



    public static bool IsMoreOldThat(in DateTime date, in IBooleanDateStateSwitch key, bool orEqual = true)
    {
        key.GetWhenSwitchHappened(out long l);
        if (orEqual)
            return date.Ticks <= l;
        else
            return date.Ticks < l;
    }

    public static bool IsMoreRecentThat(in DateTime date, in IBooleanDateStateSwitch key, bool orEqual = true)
    {
        key.GetWhenSwitchHappened(out long l);
        if (orEqual)
            return date.Ticks >= l;
        else
            return date.Ticks > l;
    }





    public void IsTimeMoreRecentThatKeys(in DateTime time, out bool mostRecent)
    {
        GetMostRecentKey(out IBooleanDateStateSwitch recent);
        if (recent == null) PushWarningToHaveAtListOneKeyIsNeeded();
        mostRecent = time.Ticks > recent.GetWhenSwitchHappenedAsLong();
    }
    public void IsTimeMoreOlderThatKeys(in DateTime time, out bool mostOlder)
    {
        GetMostOldestKey(out IBooleanDateStateSwitch oldest);
        if (oldest == null) PushWarningToHaveAtListOneKeyIsNeeded();
        mostOlder = time.Ticks < oldest.GetWhenSwitchHappenedAsLong();
    }

    protected void PushWarningToHaveAtListOneKeyIsNeeded()
    {
        throw new Exception("The methode request to have at least a key swith to works properly");
    }
    #endregion



    #region SET
    public abstract void SetNow(in bool isTrue);
    public abstract void SetNowWhenActionHappened(in DateTime whenItHappened, in bool isTrue);
    public abstract void SetPastSwitchManually(in DateTime atDate, in bool newValue);
    //{

    //}
    #endregion

    public abstract void GetAllSwitchKeyBetween(in DateTime from, in DateTime to, out IBooleanDateStateSwitch[] sample)
    ;

    public abstract void GetAllSwitchKeyInCollection(out IBooleanDateStateSwitch[] sample)
    ;

    public void GetCollectionExistingTime(out DateTime dateAtStartExisting)
    {
        dateAtStartExisting = m_whenCreatedDate;
    }

    public void GetCollectionInitialState(out bool isTrueValueAtStartExisting)
    {
        isTrueValueAtStartExisting = m_whenCreatedValue;
    }

   

    public void GetKeyAtBordersOfCollection(out IBooleanDateStateSwitch mostRecent, out IBooleanDateStateSwitch mostOld)
    {
        GetMostRecentKey(out mostRecent);
        GetMostOldestKey(out mostOld);
    }
    public abstract void GetMostOldestKey(out IBooleanDateStateSwitch switchMostOld);
    public abstract void GetMostRecentKey(out IBooleanDateStateSwitch mostRecent);


    public void GetSegmentLimitsAt(in DateTime atDate, out IBooleanDateStateSwitch switchMoreRecent, out IBooleanDateStateSwitch switchMostOld)
    {
        GetSegmentOldSwitchSideAt(in atDate,  out switchMostOld);
        GetSegmentRecentSwitchSideAt(in atDate,  out switchMoreRecent);
    }

    public abstract void GetSegmentOldSwitchSideAt(in DateTime atDate, out IBooleanDateStateSwitch switchKeyOld)
 ;
    public abstract void GetSegmentRecentSwitchSideAt(in DateTime atDate, out IBooleanDateStateSwitch switchKeyRecent)
  ;
    public void GetStateAt(in DateTime atDate, out bool stateIsTrue)
    {
        if (GetSwitchCount() == 0) { 
            GetCollectionInitialState(out stateIsTrue);
            return;
        }
        long dateAsLong = atDate.Ticks;
        if (GetSwitchCount() == 1)
        {
            GetMostRecentKey(out IBooleanDateStateSwitch mostRecent);
            mostRecent.GetWhenSwitchHappened(out long mostRecentLong);
            if (dateAsLong >= mostRecentLong)
            {
                stateIsTrue = mostRecent.TurnedTrue();
            }
            else
            {

                stateIsTrue = mostRecent.WasTrue();
            }
        }
        else
        {
            GetSegmentLimitsAt(in atDate, out IBooleanDateStateSwitch recent, out IBooleanDateStateSwitch oldest);
            if (oldest != null)
            {
                stateIsTrue = oldest.TurnedTrue();
            }
            else if (recent != null)
            {
                stateIsTrue = recent.WasTrue();
            }
            else throw new Exception("recent and oldest shoud not be null");
        }
    }

    public void GetStateNow(out bool state)
    {
        if (GetSwitchCount() == 0)
            GetCollectionInitialState(out state);
        else {
            GetMostRecentKey(out IBooleanDateStateSwitch r);
            state = r.TurnedTrue();
        }
    }


    public abstract void GetSwitchCount(out int count);
    public int GetSwitchCount() { GetSwitchCount(out int count); return count; }

    public void GetSwitchCountAndLimitBetween(in DateTime from, in DateTime to, out bool mostRecentValueStart, out bool mostOldValueStart, out int switchToTrue, out int switchToFalse)
    {
        switchToTrue = 0;
        switchToFalse = 0;
        GetAllSwitchKeyBetween(in from, in to, out IBooleanDateStateSwitch[] sample);
        if (sample == null || sample.Length == 0)
        {
            switchToTrue = 0;
            switchToFalse = 0;
            // If sample is null the it is out invalide or in a true or false state all along.
            // If invalide, they should be an exception trigger somewhere. So it is consider a "flat line"
            GetStateAt(in from,  out bool state);
            mostOldValueStart = state;
            mostRecentValueStart = state;
            return;
        }

        mostOldValueStart = false;
        mostRecentValueStart = false;
        for (int i = 0; i < sample.Length; i++)
        {
            if (i == 0)
            {
                mostRecentValueStart = sample[0].TurnedTrue();
            }
            if (sample[i].TurnedTrue())
            {
                switchToTrue++;
            }
            else if (sample[i].TurnedFalse())
            {
                switchToFalse++;
            }
            if (i == sample.Length - 1)
            {
                mostOldValueStart = sample[sample.Length - 1].WasTrue();
            }
        }
    }

    public void GetSwitchCountBetween(in DateTime from, in DateTime to, out int switchToTrue, out int switchToFalse)
    {
        switchToTrue = 0;
        switchToFalse = 0;
        GetAllSwitchKeyBetween(in from, in to, out IBooleanDateStateSwitch[] sample);
        for (int i = 0; i < sample.Length; i++)
        {
            if (sample[i].TurnedTrue())
                switchToTrue++;
            else if (sample[i].TurnedFalse())
                switchToFalse++;
        }
    }

    public void GetTrueFalseRatio(in DateTime from, in DateTime to, out double pourcentTrue)
    {
        GetTrueFalseTimeInTick(from, to, out long nanoTrue, out long nanoFalse, out long total);
        pourcentTrue = ((double)nanoTrue) / ((double)total);
    }
    
    public void GetTrueFalseTimeInTick(in DateTime startRecent, in DateTime toOlder,
         out long nanoSecondTrue, out long nanoSecondFalse,
         out long nanoSecondsTotalObserved)
    {
        DateTimeSwitchUtility.GetDateLong(in startRecent, in toOlder, out long startRecentLong, out long startOldLong);
        GetAllSwitchKeyBetween(in startRecent, in toOlder, out IBooleanDateStateSwitch[] sample);
        nanoSecondsTotalObserved = startRecentLong - startOldLong;
        nanoSecondTrue = 0;
        nanoSecondFalse = 0;
        if (sample.Length > 0)
        {
            IBooleanDateStateSwitch start = sample[0];
            sample[0].GetWhenSwitchHappened(out long ls);
            long statl = startRecent.Ticks - ls;
            if (sample[0].TurnedFalse())
                nanoSecondTrue += statl;
            else nanoSecondFalse += statl;
            if (sample.Length > 1)
            {
                for (int i = 0; i < sample.Length - 1; i++)
                {
                    GetSegmentInfoOf(in sample[i], in sample[i + 1], out long tickTime, out bool isTrueState);
                    if (isTrueState)
                        nanoSecondTrue += tickTime;
                    else nanoSecondFalse += tickTime;
                }
            }

            sample[sample.Length - 1].GetWhenSwitchHappened(out long le);
            if (sample[sample.Length - 1].WasTrue())
                nanoSecondTrue += le;
            else nanoSecondFalse += le;
        }
        else
        {
            nanoSecondsTotalObserved = startRecentLong - startOldLong;
            DateTime d = new DateTime(startOldLong + nanoSecondsTotalObserved / 2);
            GetStateAt(in d, out bool state);
            nanoSecondTrue = state ? nanoSecondsTotalObserved : 0;
            nanoSecondFalse = state ? 0 : nanoSecondsTotalObserved;
        }
    }


    public void HasAnySwitchHappened(out bool hasSomeRecord)
    {
        hasSomeRecord = GetSwitchCount() > 0;
    }

    public bool IsAfterLastKeySwitch(in DateTime atDate)
    {
        GetMostOldestKey(out IBooleanDateStateSwitch last);
        if (last == null) throw new NullReferenceException("Check that one key is recorded first");
        return last.IsMoreOldThatKey(in atDate, false);
    }

    public bool IsBeforeFirstKeySwitch(in DateTime atDate)
    {
        GetMostRecentKey(out IBooleanDateStateSwitch first);
        if (first == null) throw new NullReferenceException("Check that one key is recorded first");
        return first.IsMoreRecentThatKey(in atDate, false);
    }

    public bool IsBetweenFirstAndLastKeySwitch(in DateTime atDate)
    {
        return !(IsBeforeFirstKeySwitch( in atDate) || IsAfterLastKeySwitch( in atDate));
    }


    public bool IsDateTimeInInvalideTrackZone(in DateTime date)
    {
        return !IsDateTimeInValideTrackZone(in date);
    }

    public bool IsDateTimeInValideTrackZone(in DateTime date)
    {
        if (date > DateTime.Now)
            return false;
        GetCollectionExistingTime(out DateTime maxTime);
        if (date < maxTime)
            return false;
        return true;
    }

  
    public bool WasFalseAt(in DateTime atDate, in string namedboolean)
    {
        GetSegmentInfoOf(in atDate, out bool state);
        return !state;
    }

    public bool WasTrueAt(in DateTime atDate, in string namedboolean)
    {
        GetSegmentInfoOf(in atDate, out bool state);
        return state;
    }

    public abstract void GetSegmentInfoOf(in DateTime atDate, out bool state);

    public void GetElapsedTimeAsTicksAt(in DateTime atDate, out long ticksDuration, out DateTime switchOldestPart)
    {

        GetSegmentOldSwitchSideAt(in atDate, out IBooleanDateStateSwitch okey);
        GetSegmentRecentSwitchSideAt(in atDate, out IBooleanDateStateSwitch rkey);
        if (okey == null)
            throw new Exception("Check that date is between now and lost key");
        if (rkey == null)
        {
           okey.GetWhenSwitchHappened(out switchOldestPart);
           ticksDuration = DateTime.Now.Ticks- switchOldestPart.Ticks;
            return;
        }
        okey.GetWhenSwitchHappened(out switchOldestPart);
        rkey.GetWhenSwitchHappened(out long rl);
        ticksDuration = rl - switchOldestPart.Ticks;
    }



    #region GET SEGMENT INFO
    public bool equalTrue;
    public bool equalFalse;

    public void GetSegmentInfoOf(in IBooleanDateStateSwitch recent, in IBooleanDateStateSwitch old, out long tickTime, out bool state)
    {
        recent.GetWhenSwitchHappened(out long rl);
        old.GetWhenSwitchHappened(out long ol);
        tickTime = rl - ol;
        old.GetSwitchType(out BooleanSwithType sw);
        state = sw == BooleanSwithType.FalseToTrue;
    }

   

    #endregion
}
