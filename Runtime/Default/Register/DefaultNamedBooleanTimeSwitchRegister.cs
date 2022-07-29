using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DefaultNamedBooleanTimeSwitchRegister : INamedBooleanTimeSwitchRegister
{
    public int m_maxKeyPerList = 512;
    public Dictionary<string, AbstractBooleandDateSwitchCollectionDefault> m_register = new Dictionary<string, AbstractBooleandDateSwitchCollectionDefault>();
    public DateTime m_createdDate;
    public bool m_stateWhenCreated;

    public DefaultNamedBooleanTimeSwitchRegister(int maxKeyPerList = 512)
    {
        m_maxKeyPerList = maxKeyPerList;
        m_createdDate = DateTime.Now;
    }

    public void CreateSlotIfNotExisting(in string namedBoolean, in bool startValue)
    {
        if (!m_register.ContainsKey(namedBoolean))
        {
            BooleandDateSwitchCollectionDefault toAdd = new BooleandDateSwitchCollectionDefault(
                m_maxKeyPerList, startValue, DateTime.Now);
            m_register.Add(namedBoolean, toAdd);
        }
    }

    public void GetElapsedTimeAsTicksAt(in DateTime atDate, in string namedboolean, out long nanoSeconds, out  DateTime switchOldestPart)
    {
        m_register[namedboolean].GetElapsedTimeAsTicksAt(in atDate, out  nanoSeconds, out switchOldestPart);
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

    public void GetTrueFalseRatio(in DateTime from, in DateTime to, in string namedboolean, out double percentTrue)
    {
        m_register[namedboolean].GetTrueFalseRatio(in from, in to, out percentTrue);
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
        return m_register[namedboolean].IsDateTimeInInvalideTrackZone(in date);
    }
    public bool IsDateTimeInValideTrackZone(in string namedboolean, in DateTime date)
    {
        return m_register[namedboolean].IsDateTimeInValideTrackZone(in date);
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

        m_register[namedboolean].GetTrueFalseTimeInTick(in from,in  to, 
            
           out tickTimeTrue, out tickTimeFalse,
           out tickTotalObserved);
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
       return m_register[namedboolean].IsBeforeFirstKeySwitch(in atDate);    }

    public bool IsAfterLastKeySwitch(in string namedboolean, in DateTime atDate)
    {
        return m_register[namedboolean].IsAfterLastKeySwitch(in atDate);
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
}
