using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefaultNamedBooleanTimeSwitchRegisterEasyCode : INamedBooleanTimeSwitchRegister
{
    public int m_maxKeyPerList = 512;
    public Dictionary<string, BooleandDateSwitchCollectionDefault> m_register = new Dictionary<string, BooleandDateSwitchCollectionDefault>();
    public DateTime m_createdDate;
    public bool m_stateWhenCreated;

    public DefaultNamedBooleanTimeSwitchRegisterEasyCode(int maxKeyPerList = 512)
    {
        m_maxKeyPerList = maxKeyPerList;
            m_createdDate = DateTime.Now;
    }

    public void CreateSlotIfNotExisting(in string namedBoolean, in bool startValue)
    {
        if (!m_register.ContainsKey(namedBoolean))
        {
            AAA toAdd = new AAA(
                m_maxKeyPerList, startValue, DateTime.Now);
            m_register.Add(namedBoolean, toAdd);
        }
    }

    public void GetElapsedTimeAsTicksAt(in DateTime atDate, in string namedboolean, out long nanoSeconds, out  DateTime switchOldestPart)
    {
        GetSegmentLimitsAt(in atDate, in namedboolean, out IBooleanDateStateSwitch recent, out IBooleanDateStateSwitch old);
        if (recent == null && old == null
            || recent != null && old == null) {
            throw new Exception("Check that the time is valide before using this methode");
        }
        else if (recent == null && old != null)
        {
            old.GetWhenSwitchHappened(out long o);
            nanoSeconds = DateTime.Now.Ticks - o;
        }
        else  {
            recent.GetWhenSwitchHappened(out long r);
            old.GetWhenSwitchHappened(out long o);
            nanoSeconds = r - o;
        }
        old.GetWhenSwitchHappened(out switchOldestPart);
    }

    public void GetKeyAtBordersOfBoolean(in string namedboolean, out IBooleanDateStateSwitch mostRecent, out IBooleanDateStateSwitch switchMostOld)
    {
        m_register[namedboolean].GetMostRecentKey(out IBooleanDateStateSwitch recent);
        mostRecent = recent;
        m_register[namedboolean].GetMostOldestKey(out IBooleanDateStateSwitch old);
        switchMostOld = old;
    }


    public void GetAllSwitchKeyBetween(in DateTime from, in DateTime to, in string namedboolean, out IBooleanDateStateSwitch[] sample)
    {
               m_register[namedboolean].GetAllSwitchKeyBetween(in from, in to, out sample);
    }
   


    public void GetCollectionInitialState(in string namedboolean, out bool isTrueValueAtStartExisting)
    {
        m_register[namedboolean].GetCollectionInitialState(out isTrueValueAtStartExisting);
    }

    public void GetCollectionExistingTime(in string namedboolean, out DateTime dateAtStartExisting)
    {
        m_register[namedboolean].GetCollectionExistingTime(out dateAtStartExisting);
    }

    public void GetStateAt(in DateTime atDate, in string namedboolean, out bool state)
    {
        m_register[namedboolean].GetStateAt(in atDate, out state);
    }

    public void GetStateNow(in string namedboolean, out bool state)
    {
        m_register[namedboolean].GetStateNow(out state);
    }
    public void GetSwitchCount(in string namedboolean, out int switchCount)
    {
        switchCount = m_register[namedboolean].GetSwitchCount();
    }
   

    public void GetSwitchCountAndLimitBetween(in DateTime from, in DateTime to, in string namedboolean, out bool mostRecentValueStart, out bool mostOldValueStart, out int switchToTrue, out int switchToFalse)
    {
        m_register[namedboolean].GetSwitchCountAndLimitBetween(
            in from, in to,  out  mostRecentValueStart, out  mostOldValueStart, out  switchToTrue, out  switchToFalse);
    }

    public void GetSwitchCountBetween(in DateTime from, in DateTime to, 
        in string namedboolean, out int switchToTrue, out int switchToFalse)
    {
        m_register[namedboolean].GetSwitchCountBetween(in from, in to, out  switchToTrue, out  switchToFalse);
    }

    public void GetTrueFalseRatio(in DateTime from, in DateTime to, in string namedboolean, out double pourcentTrue)
    {
        GetTrueFalseTimeInTick(in from, in to, in namedboolean, out long nanoTrue, out long nanoFalse, out long total);
        pourcentTrue = ((double)nanoTrue) / ((double)total);
    }

  

    public void HasAnySwitchHappened(in string namedboolean, out bool hasSomeRecord)
    { 
        IsBooleanRegistered(in namedboolean, out bool existing);
        if (!existing) { hasSomeRecord = false; return; }
        hasSomeRecord = m_register[namedboolean].GetSwitchCount() > 0;
    }

    public void IsBooleanRegistered(in string namedboolean, out bool existing)
    {
        existing = m_register.ContainsKey(namedboolean);
    }

    public bool IsDateTimeInInvalideTrackZone(in string namedboolean, in DateTime date)
    {
        return !(date <= DateTime.Now && date.Ticks >= m_createdDate.Ticks);
    }
    public bool IsDateTimeInValideTrackZone(in string namedboolean, in DateTime date)
    {
        return date <= DateTime.Now && date.Ticks >= m_createdDate.Ticks;
    }

    public void SetNow(in string namedBoolean, in bool value)
    {
        IsBooleanRegistered(in namedBoolean, out bool exists);
        if (!exists) CreateSlotIfNotExisting(in namedBoolean, in value);
        else m_register[namedBoolean].SetNow(value);
    }

    public void SetNowWhenActionHappened(in DateTime whenItHappened, in string namedBoolean, in bool value)
    {
        IsBooleanRegistered(in namedBoolean, out bool exists);
        if (!exists) CreateSlotIfNotExisting(in namedBoolean, in value);
        else m_register[namedBoolean].SetNowWhenActionHappened(whenItHappened, value);
    }

    public void SetPastSwitchManually(in DateTime previousDate, in string namedBoolean, in bool newValue)
    {
        IsBooleanRegistered(in namedBoolean, out bool exists);
        if (!exists) throw new Exception("You must be sure to have slot before using a pas switch manually.");
        m_register[namedBoolean].SetPastSwitchManually(previousDate, newValue);
    }

    public bool WasFalseAt(in DateTime atDate, in string namedboolean)
    {
        m_register[namedboolean].IsTimeMoreOlderThatKeys(in atDate, out bool mostOlder);
        if (mostOlder)
        {
            m_register[namedboolean].GetMostOldestKey(out IBooleanDateStateSwitch last);
            return last.WasFalse();
        }
        else
        {
            GetSegmentOldSwitchSideAt(in atDate, in namedboolean, out IBooleanDateStateSwitch key);
            return key.TurnedFalse();
        }
    }

    public bool WasTrueAt(in DateTime atDate, in string namedboolean)
    {

        m_register[namedboolean].IsTimeMoreOlderThatKeys(in atDate, out bool mostOlder);
        if (mostOlder)
        {
            m_register[namedboolean].GetMostOldestKey(out IBooleanDateStateSwitch last);
            return last.WasTrue();
        }
        else { 
            GetSegmentOldSwitchSideAt(in atDate, in namedboolean, out IBooleanDateStateSwitch key);
            return key.TurnedTrue();
        }
    }

   
    

    public void GetSegmentLimitsAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateSwitch switchMoreRecent, out IBooleanDateStateSwitch switchMostOld)
    {
        GetSegmentOldSwitchSideAt(in atDate, in namedboolean, out switchMostOld);
        GetSegmentRecentSwitchSideAt(in atDate, in namedboolean, out switchMoreRecent);
    }
    public void GetSegmentOldSwitchSideAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateSwitch key)
    {

        m_register[namedboolean].GetSegmentOldSwitchSideAt(in atDate, out key);
    
    }

    public void GetSegmentRecentSwitchSideAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateSwitch key)
    {
        m_register[namedboolean].GetSegmentRecentSwitchSideAt(in atDate, out key);
        
    }

    public void GetTrueFalseTimeInTick(in DateTime from, in DateTime to,
        in string namedboolean, out long tickTimeTrue, out long tickTimeFalse,
        out long tickTotalObserved)
    {
        GetTrueFalseTimeInTickRef( from,  to, in namedboolean, 
           out tickTimeTrue, out tickTimeFalse,
           out tickTotalObserved);
    }
        public void GetTrueFalseTimeInTickRef( DateTime startRecent,  DateTime toOlder,
        in string namedboolean, out long nanoSecondTrue, out long nanoSecondFalse,
        out long nanoSecondsTotalObserved)
        {

        if (startRecent < toOlder)
        {
            DateTime tmp = startRecent;
            startRecent = toOlder;
            toOlder = tmp;
        }

        GetAllSwitchKeyBetween(in startRecent, in toOlder, in namedboolean, out IBooleanDateStateSwitch[] sample);
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
        else {
            GetStateAt(in startRecent, in namedboolean, out bool state);
            nanoSecondsTotalObserved = startRecentLong - startOldLong;
            nanoSecondTrue = state ? nanoSecondsTotalObserved : 0;
            nanoSecondFalse = state ? 0:nanoSecondsTotalObserved ;
        }
    }

    private void GetSegmentInfoOf(in IBooleanDateStateSwitch recent, in IBooleanDateStateSwitch old, 
        out long tickTime, out bool isTrueState)
    {
        isTrueState = old.TurnedTrue();
        old.GetWhenSwitchHappened(out long o);
        recent.GetWhenSwitchHappened(out long r);
        tickTime = r - o;
    }

    public void GetAllSwitchKeyFor(in string namedboolean, out IBooleanDateStateSwitch[] sample)
    {
        m_register[namedboolean].GetAllSwitchKeyInCollection(out sample);
    }

    public bool IsBeforeFirstKeySwitch(in string namedboolean, in DateTime atDate)
    {
        m_register[namedboolean].GetMostRecentKey(out IBooleanDateStateSwitch first);
        if (first == null) throw new Exception("Check that one key is recorded first");
        return BooleandDateSwitchCollectionDefault.IsMoreRecentThat(in atDate, in first);
    }

    public bool IsAfterLastKeySwitch(in string namedboolean, in DateTime atDate)
    {
        m_register[namedboolean].GetMostOldestKey(out IBooleanDateStateSwitch first);
        if (first == null) throw new Exception("Check that one key is recorded first");
        return BooleandDateSwitchCollectionDefault.IsMoreOldThat(in atDate, in first);
    }

    public bool IsBetweenFirstAndLastKeySwitch(in string namedboolean, in DateTime atDate)
    {
        return !(IsBeforeFirstKeySwitch(in namedboolean, in atDate) || IsAfterLastKeySwitch(in namedboolean, in atDate));
    }

    public void GetMostRecentKey(in string namedboolean, out IBooleanDateStateSwitch mostRecent)
    {
        m_register[namedboolean].GetMostRecentKey(out mostRecent);
    }

    public void GetMostOldestKey(in string namedboolean, out IBooleanDateStateSwitch switchMostOld)
    {
        m_register[namedboolean].GetMostOldestKey(out switchMostOld);
    }

    public void GetCollectionOfSwitch(in string namedboolean, out INamedBooleanTimeSwitchCollectionGet collection)
    {
        if (m_register.ContainsKey( namedboolean))
            collection = m_register[namedboolean];
        else collection = null;
    }




    //public void CreateSlotIfNotExisting(in string namedBoolean, in bool startValue)
    //{
    //    if (!m_register.ContainsKey(namedBoolean))
    //    {
    //        m_register.Add(namedBoolean, new BooleanDateStateSwitchClampList() { m_maxKey = m_maxKeyPerList });
    //        m_register[namedBoolean].SetWithNow(startValue);
    //    }
    //}
    //public bool IsContaining(in string namedBoolean) { return m_register.ContainsKey(namedBoolean); }
    //public bool IsNotContaining(in string namedBoolean) { return !m_register.ContainsKey(namedBoolean); }

    //public void GetAllSwitchDateBetween(in DateTime start, in DateTime to, in string namedboolean, out IBooleanDateStateSwitch[] sample)
    //{
    //    if (IsNotContaining(in namedboolean))
    //    {
    //        sample = new IBooleanDateStateSwitch[0];
    //        return;
    //    }
    //    else
    //    {
    //        m_register[namedboolean].GetAllSwitchDateBetween(in start, in to, out sample);
    //    }
    //}


    //public void GetLimitesSwitchOfBoolean(in string namedboolean, out IBooleanDateStateSwitch oldest, out IBooleanDateStateSwitch mostRecent)
    //{
    //    oldest = null;
    //    mostRecent = null;
    //    if (IsNotContaining(in namedboolean))
    //        return;
    //    m_register[namedboolean].GetMostRecent(out BooleanDateStateSwitchKey recent);
    //    m_register[namedboolean].GetMostFarInTime(out BooleanDateStateSwitchKey old);
    //    oldest = old;
    //    mostRecent = recent;
    //}



    //public void GetStateNow(in string namedboolean, out bool existing, out bool state)
    //{
    //    if (IsNotContaining(in namedboolean))
    //    {
    //        existing = false;
    //        state = false;
    //    }
    //    else
    //    {
    //        existing = true;
    //        state = m_register[namedboolean].GetState();
    //    }
    //}


    //public void SetNow(in string namedBoolean, in bool value, in bool createIfNotExisting)
    //{
    //    if (IsContaining(in namedBoolean))
    //    {
    //        m_register[namedBoolean].SetWithNow(value);
    //    }
    //    else if (createIfNotExisting)
    //    {
    //        CreateSlotIfNotExisting(in namedBoolean, in value);
    //        m_register[namedBoolean].SetWithNow(value);

    //    }
    //}
    //public void SetNowWhenActionHappened(in DateTime whenItHappened, in string namedBoolean, in bool value, in bool createIfNotExisting)
    //{
    //    m_register[namedBoolean].SetWithNow(whenItHappened, value);
    //}

    //public void IsBooleanRegistered(in string namedboolean, out bool existing)
    //{
    //    existing = m_register.ContainsKey(namedboolean);
    //}

    //public void HasAnySwitchHappened(in string namedboolean, out bool hasSomeRecord)
    //{
    //    hasSomeRecord = m_register[namedboolean].GetSwitchCount() > 0;
    //}

    //public void GetSwitchCount(in string namedboolean, out int switchCount)
    //{
    //    m_register[namedboolean].GetSwitchCount(out switchCount);
    //}

    //public bool IsDateTimeInInvalideTrackZone(in string namedboolean, in DateTime date)
    //{
    //    m_register[namedboolean].GetMostFarInTime(out BooleanDateStateSwitch l);
    //    return date.Ticks < l.WhenSwitchHappenedLong();
    //}



    //public void GetSwitchCount(in DateTime start, in DateTime to, in string namedboolean,
    //    out bool startValue, out bool endValue, out int switchToTrue, out int switchToFalse)
    //{
    //    switchToFalse = 0;
    //    switchToTrue = 0;
    //    //SHOULD BE IN UTILITY.d
    //    BooleanDateStateSwitchClampList t = m_register[namedboolean];
    //    t.GetState(out startValue);
    //    t.GetOldestState(out endValue);
    //    t.GetAllSwitchDateBetween(in start, in to, out IBooleanDateStateSwitch[] sample);
    //    for (int i = 0; i < sample.Length; i++)
    //    {
    //        sample[i].GetSwitchType(out BooleanSwithType s);
    //        if (s == BooleanSwithType.TrueToFalse)
    //            switchToFalse++;
    //        else switchToTrue++;
    //    }
    //}

    //public void GetStateNow(in string namedboolean, out bool state)
    //{
    //    GetStateAt(DateTime.Now, in namedboolean, out state);
    //}




}
