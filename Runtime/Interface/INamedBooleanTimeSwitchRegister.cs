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
    public void GetStartExistingTime(in string namedboolean, out  DateTime dateAtStartExisting);
    public void GetStartExistingInitialState(in string namedboolean, out bool isTrueValueAtStartExisting);
    public bool WasTrueAt(in DateTime atDate, in string namedboolean); // Throw Exception if not found
    public bool WasFalseAt(in DateTime atDate, in string namedboolean); // Throw Exception if not found

    public void GetSwitchCountAndLimitBetween(in DateTime from, in DateTime to, in string namedboolean, out bool mostRecentValueStart, out bool mostOldValueStart, out int switchToTrue, out int switchToFalse);
    public void GetSwitchCountBetween(in DateTime from, in DateTime to, in string namedboolean , out int switchToTrue, out int switchToFalse);

    public void GetStateNow(in string namedboolean, out bool state);
    public void GetStateAt(in DateTime atDate, in string namedboolean, out bool state);
 
    public void GetSegmentLimitsAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateSwitch switchMoreRecent, out IBooleanDateStateSwitch switchMostOld);
    public void GetElapsedTimeInNanoSecondsAt(in DateTime atDate, in string namedboolean, out long nanoSeconds, out DateTime switchOldestPart);
    public void GetAllSwitchDateBetween(in DateTime from, in DateTime to, in string namedboolean, out IBooleanDateStateSwitch[] sample);
    public void GetAllSwitchDateFor( in string namedboolean, out IBooleanDateStateSwitch[] sample);
    public void GetLimitesSwitchOfBoolean(in string namedboolean, out IBooleanDateStateSwitch mostRecent, out IBooleanDateStateSwitch switchMostOld);
    public void GetTrueFalseRatio(in DateTime from, in DateTime to, in string namedboolean, out double pourcentTrue);
    public void GetTrueFalseTimeInTick(in DateTime from, in DateTime to, in string namedboolean, out long nanoSecondTrue, out long nanoSecondFalse, out long nanoSecondTotalObserved);
    //public void GetSegmentBetweenTwoSwitchAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateDualSwitchSegment segment);
    public void GetSegmentOldSwitchSideAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateSwitch switchKeyOld);
    public void GetSegmentRecentSwitchSideAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateSwitch switchKeyRecent);
 
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
    public void SetNowWhenActionHappened(in DateTime whenItHappened,in string namedBoolean, in bool value);
    /// <summary>
    /// Be Carefull with this one because if you have two time the same switch that are following you can break the code of the developer below the interface or kill it performance.
    /// </summary>
    public void SetPastSwitchManually(in DateTime previousDate, in string namedBoolean, in bool newValue);
}

public enum BooleanSwithType : byte { FalseToTrue = 1, TrueToFalse = 0 }
public interface IBooleanDateStateSwitch
{
    public void GetWhenSwitchHappened(out long dateInTickTimeWhenSwitchHappened);
    public void GetWhenSwitchHappened(out DateTime whenSwitchHappened);
    public void GetSwitchType(out BooleanSwithType switchType);
    public bool WasTrue();
    public bool TurnedTrue();
    public bool WasFalse();
    public bool TurnedFalse();
}


public interface IBooleanDateStateDualSwitchSegment
{
    public void GetSwitchMostRecent(out IBooleanDateStateSwitch switchType);
    public void GetSwitchMostOlder(out IBooleanDateStateSwitch switchType);
    public void GetDurationInNanoseconds(out long duration);
    public void GetDurationInMilliseconds(out double milliseconds);
    public void GetDurationInMilliseconds(out int millisecondsMax24Day);
    public bool WasTrueDuringDuration();
    public bool IsTrueAfterSwitches();
    public bool IsTrueBeforeSwitches();

}

/// //////////////////////////////// IMPLEMENTAITON DATE PART


public interface IBooleanDateStateSwitchUtility
{

    // public bool WasOnlyTrue(in IBooleanDateStateSwitch[] sample, in )
}

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