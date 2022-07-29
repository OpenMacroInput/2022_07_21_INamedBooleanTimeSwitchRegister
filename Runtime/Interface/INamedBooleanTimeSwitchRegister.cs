using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public interface INamedBooleanTimeSwitchRegister : INamedBooleanTimeSwitchRegisterGet, INamedBooleanTimeSwitchRegisterSet
{
}

/// Small warning Nano second is not handle by C# in DateTime but are use as Tick. A tick is 100 nanoseconds. so you have 10 000 tick under the milliseconds or 10000(00) tick in milliseconds a
/// And 10000000 tick in seconds. 
//m_moreRecentTimeTick = now.Ticks - firstSwitchDate.Ticks;
//m_moreRecentTimeMs = (long)(m_moreRecentTimeTick / 10000.0);
//m_moreRecentTimeSeconds = (long)(m_moreRecentTimeTick / 10000000.0);
public interface INamedBooleanTimeSwitchRegisterGet
{
    public void IsBooleanRegistered( in string namedboolean, out bool existing);
    public void HasAnySwitchHappened( in string namedboolean, out bool hasSomeRecord);
    public void GetSwitchCount( in string namedboolean, out int switchCount);
    public bool IsDateTimeInInvalideTrackZone(in string namedboolean, in DateTime date);
    public bool IsDateTimeInValideTrackZone(in string namedboolean, in DateTime date);
    public void GetCollectionExistingTime(in string namedboolean, out  DateTime dateAtStartExisting);
    public void GetCollectionInitialState(in string namedboolean, out bool isTrueValueAtStartExisting);
    public bool WasTrueAt(in DateTime atDate, in string namedboolean); // Throw Exception if not found
    public bool WasFalseAt(in DateTime atDate, in string namedboolean); // Throw Exception if not found

    public void GetSwitchCountAndLimitBetween(in DateTime from, in DateTime to, in string namedboolean, out bool mostRecentValueStart, out bool mostOldValueStart, out int switchToTrue, out int switchToFalse);
    public void GetSwitchCountBetween(in DateTime from, in DateTime to, in string namedboolean , out int switchToTrue, out int switchToFalse);

    public void GetStateNow(in string namedboolean, out bool state);
    public void GetStateAt(in DateTime atDate, in string namedboolean, out bool state);
 
    public void GetSegmentLimitsAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateSwitch switchMoreRecent, out IBooleanDateStateSwitch switchMostOld);
    public void GetElapsedTimeAsTicksAt(in DateTime atDate, in string namedboolean, out long nanoSeconds, out DateTime switchOldestPart);
    public void GetAllSwitchKeyBetween(in DateTime from, in DateTime to, in string namedboolean, out IBooleanDateStateSwitch[] sample);
    public void GetAllSwitchKeyFor( in string namedboolean, out IBooleanDateStateSwitch[] sample);
    public void GetKeyAtBordersOfBoolean(in string namedboolean, out IBooleanDateStateSwitch mostRecent, out IBooleanDateStateSwitch switchMostOld);
    public void GetMostRecentKey(in string namedboolean, out IBooleanDateStateSwitch mostRecent);
    public void GetMostOldestKey(in string namedboolean, out IBooleanDateStateSwitch switchMostOld);
    public void GetTrueFalseRatio(in DateTime from, in DateTime to, in string namedboolean, out double pourcentTrue);
    public void GetTrueFalseTimeInTick(in DateTime from, in DateTime to, in string namedboolean, out long nanoSecondTrue, out long nanoSecondFalse, out long nanoSecondTotalObserved);
    public void GetSegmentOldSwitchSideAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateSwitch switchKeyOld);
    public void GetSegmentRecentSwitchSideAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateSwitch switchKeyRecent);
    public bool IsBeforeFirstKeySwitch(in string namedboolean, in DateTime atDate);
    public bool IsAfterLastKeySwitch(in string namedboolean, in DateTime atDate);
    public bool IsBetweenFirstAndLastKeySwitch(in string namedboolean, in DateTime atDate);

    public void GetCollectionOfSwitch(in string namedboolean, out INamedBooleanTimeSwitchCollectionGet collection);
}

public interface INamedBooleanTimeSwitchCollectionHolder
{
    public void GetCollection(out INamedBooleanTimeSwitchCollection collection);
    public INamedBooleanTimeSwitchCollection GetCollection();
}
public interface INamedBooleanTimeSwitchCollection : INamedBooleanTimeSwitchCollectionGet, INamedBooleanTimeSwitchCollectionSet { }

public interface INamedBooleanTimeSwitchCollectionGet
{
    public void HasAnySwitchHappened(  out bool hasSomeRecord);
    public void GetSwitchCount(out int switchCount);
    public int GetSwitchCount();
    public bool IsDateTimeInInvalideTrackZone(  in DateTime date);
    public bool IsDateTimeInValideTrackZone(  in DateTime date);
    public void GetCollectionExistingTime(  out DateTime dateAtStartExisting);
    public void GetCollectionInitialState(  out bool isTrueValueAtStartExisting);
    public bool WasTrueAt(in DateTime atDate, in string namedboolean); // Throw Exception if not found
    public bool WasFalseAt(in DateTime atDate, in string namedboolean); // Throw Exception if not found
    public void GetSwitchCountAndLimitBetween(in DateTime from, in DateTime to,   out bool mostRecentValueStart, out bool mostOldValueStart, out int switchToTrue, out int switchToFalse);
    public void GetSwitchCountBetween(in DateTime from, in DateTime to,   out int switchToTrue, out int switchToFalse);
    public void GetStateNow(  out bool state);
    public void GetStateAt(in DateTime atDate,   out bool state);
    public void GetSegmentLimitsAt(in DateTime atDate,   out IBooleanDateStateSwitch switchMoreRecent, out IBooleanDateStateSwitch switchMostOld);
    public void GetElapsedTimeAsTicksAt(in DateTime atDate,   out long nanoSeconds, out DateTime switchOldestPart);
    public void GetAllSwitchKeyBetween(in DateTime from, in DateTime to,   out IBooleanDateStateSwitch[] sample);
    public void GetAllSwitchKeyInCollection(  out IBooleanDateStateSwitch[] sample);
    public void GetKeyAtBordersOfCollection(  out IBooleanDateStateSwitch mostRecent, out IBooleanDateStateSwitch switchMostOld);
    public void GetMostRecentKey(  out IBooleanDateStateSwitch mostRecent);
    public void GetMostOldestKey(  out IBooleanDateStateSwitch switchMostOld);
    public void GetTrueFalseRatio(in DateTime from, in DateTime to,   out double pourcentTrue);
    public void GetTrueFalseTimeInTick(in DateTime from, in DateTime to,   out long nanoSecondTrue, out long nanoSecondFalse, out long nanoSecondTotalObserved);
    public void GetSegmentOldSwitchSideAt(in DateTime atDate,   out IBooleanDateStateSwitch switchKeyOld);
    public void GetSegmentRecentSwitchSideAt(in DateTime atDate,   out IBooleanDateStateSwitch switchKeyRecent);
    public bool IsBeforeFirstKeySwitch(  in DateTime atDate);
    public bool IsAfterLastKeySwitch(  in DateTime atDate);
    public bool IsBetweenFirstAndLastKeySwitch(  in DateTime atDate);
}

/// <summary>
/// App memory is not illimited. but in some application you need to have continus track for hours of the boolean history. 
/// In those case if the developer use this classe you can recover a copy of the value that are not used any more.
/// Why a struct copy, because for performance the programmer creatin a register should use a pool of object. So you don't want a value that will change soon or already changed.
/// </summary>
public interface INamedBooleanTimeSwitchRegisterBinHandle {

    public delegate void SwitchPushedOutOfMemory(out IBooleanDateStateSwitch outOfLimitCopyOfValue);
    public bool HasGlobalBinListener();
    public void AddBinListener(in SwitchPushedOutOfMemory binListener);
    public void RemoveBinListener(in SwitchPushedOutOfMemory binListener);
    public bool HasLocalBinListener(in string namedboolean);
    public void AddBinListener(in string namedboolean, in SwitchPushedOutOfMemory binListener);
    public void RemoveBinListener(in string namedboolean, in SwitchPushedOutOfMemory binListener);
}



public interface INamedBooleanTimeSwitchRegisterSet
{
    public void CreateSlotIfNotExisting(in string namedBoolean, in bool startValue);
    public void SetNow(in string namedBoolean, in bool value);
    public void SetNowWhenActionHappened(in DateTime whenItHappened, in string namedBoolean, in bool value);
    /// <summary>
    /// Be Carefull with this one because if you have two time the same switch that are following you can break the code of the developer below the interface or kill it performance.
    /// </summary>
    public void SetPastSwitchManually(in DateTime previousDate, in string namedBoolean, in bool newValue);
}

public interface INamedBooleanTimeSwitchCollectionSet
{

    public void SetNow( in bool value);
    public void SetNowWhenActionHappened(in DateTime whenItHappened, in bool value);
    /// <summary>
    /// Be Carefull with this one because if you have two time the same switch that are following you can break the code of the developer below the interface or kill it performance.
    /// </summary>
    public void SetPastSwitchManually(in DateTime previousDate, in bool newValue);
}

public enum BooleanSwithType : byte { FalseToTrue = 1, TrueToFalse = 0 }
public interface IBooleanDateStateSwitch
{
    public void GetWhenSwitchHappened(out long dateInTickTimeWhenSwitchHappened);
    public void GetWhenSwitchHappened(out DateTime whenSwitchHappened);
    public long GetWhenSwitchHappenedAsLong();
    public DateTime GetWhenSwitchHappenedAsDate();
    public void GetSwitchType(out BooleanSwithType switchType);
    public bool WasTrue();
    public bool TurnedTrue();
    public bool WasFalse();
    public bool TurnedFalse();
    public bool IsMoreRecentThatKey(in DateTime date, in bool orEqual);
    public bool IsMoreRecentThatKey(in long dateAsTicks, in bool orEqual);
    public bool IsMoreOldThatKey(in DateTime date, in bool orEqual);
    public bool IsMoreOldThatKey(in long dateAsTicks, in bool orEqual);
}


//public interface IBooleanDateStateDualSwitchSegment
//{
//    public void GetSwitchMostRecent(out IBooleanDateStateSwitch switchType);
//    public void GetSwitchMostOlder(out IBooleanDateStateSwitch switchType);
//    public void GetDurationInNanoseconds(out long duration);
//    public void GetDurationInMilliseconds(out double milliseconds);
//    public void GetDurationInMilliseconds(out int millisecondsMax24Day);
//    public bool WasTrueDuringDuration();
//    public bool IsTrueAfterSwitches();
//    public bool IsTrueBeforeSwitches();
//}

/// //////////////////////////////// IMPLEMENTAITON DATE PART


//public interface IBooleanDateStateSwitchUtility
//{

//    // public bool WasOnlyTrue(in IBooleanDateStateSwitch[] sample, in )
//}

//public interface IBooleanDateStateSwitchInContext: IBooleanDateStateSwitch
//{
//    public void GetElapsedMillisecondsBeforeSwitching(out int milliseconds_max24Days);
//    public void GetElapsedMillisecondsAfterSwitching(out int milliseconds_max24Days);
//    public void GetElapsedNanosecondsBeforeSwitching(out int nanoseconds_max2Days);
//    public void GetElapsedNanoSecondsAfterSwitching(out int nanoseconds_max2Days);
//}

//public interface IBooleanIntElapsedStateSwitch
//{
//    public void GetElapsedMillisecondsBeforeSwitching(out int milliseconds);
//    public void GetElapsedMillisecondsAfterSwitching(out int milliseconds);
//    public void GetBooleanStateSwitchType(out BooleanSwithType dateInMilliseconds);
//}