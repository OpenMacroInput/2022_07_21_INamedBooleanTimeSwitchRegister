using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefaultNamedBooleanTimeSwitchRegisterEasyCode : INamedBooleanTimeSwitchRegister
{
    public int m_maxKeyPerList = 512;
    public Dictionary<string, BooleanDateStateSwitchKeyClampList> m_register = new Dictionary<string, BooleanDateStateSwitchKeyClampList>();

    public DefaultNamedBooleanTimeSwitchRegisterEasyCode(int maxKeyPerList = 512)
    {
        m_maxKeyPerList = maxKeyPerList;
    }

    public void CreateSlotIfNotExisting(in string namedBoolean, in bool startValue)
    {
        if (!m_register.ContainsKey(namedBoolean))
        {
            BooleanDateStateSwitchKeyClampList toAdd = new BooleanDateStateSwitchKeyClampList(m_maxKeyPerList);
            toAdd.SetWithNow(startValue);
            m_register.Add(namedBoolean, toAdd);
        }
    }

    

    public void GetElapsedTimeInNanoSecondsAt(in DateTime atDate, in string namedboolean, out long nanoSeconds, out  DateTime switchOldestPart)
    {
        GetSegmentLimitsAt(in atDate, in namedboolean, out IBooleanDateStateSwitch recent, out IBooleanDateStateSwitch old);
        recent.GetWhenSwitchHappened(out long r);
        old.GetWhenSwitchHappened(out long o);
        nanoSeconds = r - o;
        old.GetWhenSwitchHappened(out switchOldestPart);

    }

    public void GetLimitesSwitchOfBoolean(in string namedboolean, out IBooleanDateStateSwitch mostRecent, out IBooleanDateStateSwitch switchMostOld)
    {
        m_register[namedboolean].GetMostRecent(out BooleanDateStateSwitchKey recent);
        mostRecent = recent;
        m_register[namedboolean].GetMostFarInTime(out BooleanDateStateSwitchKey old);
        switchMostOld = old;
    }


    public void GetAllSwitchDateBetween(in DateTime from, in DateTime to, in string namedboolean, out IBooleanDateStateSwitch[] sample)
    {
               m_register[namedboolean].GetAllSwitchDateBetween(in from, in to, out sample);
    }
   


    public void GetStartExistingInitialState(in string namedboolean, out bool isTrueValueAtStartExisting)
    {
        m_register[namedboolean].GetStateWhenCreated(out isTrueValueAtStartExisting);
    }

    public void GetStartExistingTime(in string namedboolean, out DateTime dateAtStartExisting)
    {
        m_register[namedboolean].GetDateWhenCreated(out dateAtStartExisting);
    }

    public void GetStateAt(in DateTime atDate, in string namedboolean, out bool state)
    {
        m_register[namedboolean].GetStateAt(in atDate, out state);
    }

    public void GetStateNow(in string namedboolean, out bool state)
    {
        m_register[namedboolean].GetState(out state);
    }
    public void GetSwitchCount(in string namedboolean, out int switchCount)
    {
        switchCount = m_register[namedboolean].GetSwitchCount();
    }
   

    public void GetSwitchCountAndLimitBetween(in DateTime from, in DateTime to, in string namedboolean, out bool mostRecentValueStart, out bool mostOldValueStart, out int switchToTrue, out int switchToFalse)
    {
        switchToTrue = 0;
        switchToFalse = 0;
        GetAllSwitchDateBetween(in from, in to, in namedboolean, out IBooleanDateStateSwitch[] sample);
        if (sample == null || sample.Length == 0)
        {
            switchToTrue = 0;
            switchToFalse = 0;
            // If sample is null the it is out invalide or in a true or false state all along.
            // If invalide, they should be an exception trigger somewhere. So it is consider a "flat line"
            GetStateAt(in from, in namedboolean, out bool state);
            mostOldValueStart = state;
            mostRecentValueStart = state;
            return;
        }

        mostOldValueStart = false;
        mostRecentValueStart = false;
        for (int i = 0; i < sample.Length; i++)
        {
            if (i == 0) {
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

    public void GetSwitchCountBetween(in DateTime from, in DateTime to, in string namedboolean, out int switchToTrue, out int switchToFalse)
    {
        switchToTrue = 0;
        switchToFalse = 0;
        GetAllSwitchDateBetween(in from, in to, in namedboolean, out IBooleanDateStateSwitch[] sample);
        for (int i = 0; i < sample.Length; i++)
        {
            if (sample[i].TurnedTrue())
                switchToTrue++;
            else if (sample[i].TurnedFalse())
                switchToFalse++;
        }
    }

    public void GetTrueFalseRatio(in DateTime from, in DateTime to, in string namedboolean, out double pourcentTrue)
    {
        GetTrueFalseTimeInNanoseconds(in from, in to, in namedboolean, out long nanoTrue, out long nanoFalse, out long total);
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
        if (date > DateTime.Now)
            return false;
        GetLimitesSwitchOfBoolean(in namedboolean, out IBooleanDateStateSwitch recent, out IBooleanDateStateSwitch mostOld);
        mostOld.GetWhenSwitchHappened(out long dateLong);
        return date.Ticks < dateLong;
    }

    public void SetNow(in string namedBoolean, in bool value)
    {
        IsBooleanRegistered(in namedBoolean, out bool exists);
        if (!exists) CreateSlotIfNotExisting(in namedBoolean, in value);
        else m_register[namedBoolean].SetWithNow(value);
    }

    public void SetNowWhenActionHappened(in DateTime whenItHappened, in string namedBoolean, in bool value)
    {
        IsBooleanRegistered(in namedBoolean, out bool exists);
        if (!exists) CreateSlotIfNotExisting(in namedBoolean, in value);
        else m_register[namedBoolean].SetWithNow(whenItHappened, value);
    }

    public void SetPastSwitchManually(in DateTime previousDate, in string namedBoolean, in bool newValue)
    {
        IsBooleanRegistered(in namedBoolean, out bool exists);
        if (!exists) throw new Exception("You must be sure to have slot before using a pas switch manually.");
        m_register[namedBoolean].SetPastSwitchManually(previousDate, newValue);
    }

    public bool WasFalseAt(in DateTime atDate, in string namedboolean)
    {
        GetSegmentOldSwitchSideAt(in atDate, in namedboolean, out IBooleanDateStateSwitch key);
        return key.TurnedFalse();

    }

    public bool WasTrueAt(in DateTime atDate, in string namedboolean)
    {
        GetSegmentOldSwitchSideAt(in atDate, in namedboolean, out IBooleanDateStateSwitch key);
        return key.TurnedTrue();
    }

    public void GetSegmentOldSwitchSideAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateSwitch key)
    {
        m_register[namedboolean].GetSegmentOldSideAt(in atDate, out bool found, out int index, out BooleanDateStateSwitchKey target);
        if (!found) new Exception("Maybe check first if the date is valide.");
        key = target;
    }
    public void GetSegmentLimitsAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateSwitch switchMoreRecent, out IBooleanDateStateSwitch switchMostOld)
    {
        GetSegmentOldSwitchSideAt(in atDate, in namedboolean, out switchMostOld);
        GetSegmentRecentSwitchSideAt(in atDate, in namedboolean, out switchMoreRecent);
    }

    public void GetSegmentRecentSwitchSideAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateSwitch switchKeyRecent)
    {
        m_register[namedboolean].GetSegmentRecentSideAt(in atDate, out bool found, out int index, out BooleanDateStateSwitchKey recentSide);
        if (!found) new Exception("Maybe check first if the date is valide.");
        switchKeyRecent = recentSide;
    }

    public void GetSegmentBetweenTwoSwitchAt(in DateTime atDate, out IBooleanDateStateDualSwitchSegment segment)
    {
        throw new NotImplementedException();
    }

    public void GetSegmentBetweenTwoSwitchAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateDualSwitchSegment segment)
    {
        throw new NotImplementedException();
    }
    public void GetTrueFalseTimeInNanoseconds(in DateTime from, in DateTime to, in string namedboolean, out long nanoSecondTrue, out long nanoSecondFalse, out long nanoSecondTotalObserved)
    {

        throw new NotImplementedException();
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
