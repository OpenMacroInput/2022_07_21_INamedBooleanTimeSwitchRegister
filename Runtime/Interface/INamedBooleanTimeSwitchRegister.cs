using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INamedBooleanTimeSwitchRegister: INamedBooleanTimeSwitchRegisterGet, INamedBooleanTimeSwitchRegisterSet
{

}
public interface INamedBooleanTimeSwitchRegisterGet
{

    public void IsBooleanRegistered(in DateTime date, in string namedboolean, out bool existing);
    public void HasAnySwitchHappened(in DateTime date, in string namedboolean, out bool hasSomeRecord);
    public void GetSwitchCount(in DateTime date, in string namedboolean, out int switchCount);

    public bool IsDateTimeToFarInTime(in DateTime date);
    public bool WasTrueAt(in DateTime date, in string namedboolean); // Throw Exception if not found
    public bool WasFalseAt(in DateTime date, in string namedboolean); // Throw Exception if not found

    public void GetSwitchCount(in DateTime start, in DateTime to, out bool startValue, out bool endValue, out int switchToTrue, out int switchToFalse);

    public void GetStateNow(in string namedboolean, out bool state);
    public void GetStateAt(in DateTime date, in string namedboolean, out bool state);
    public void GetLimitsOfStateAt(in DateTime date, in string namedboolean, out DateTime switchStart, out DateTime switchEnd);
    public void GetElapsedTimeInNanoSecondsAt(in DateTime date, in string namedboolean, out long nanoSeconds, in DateTime switchEnd);
    public void GetAllSwitchDateBetween(in DateTime start, in DateTime to, in string namedboolean, out IBooleanDateStateSwitch[] sample);
    public void GetLimitesSwitchOfBoolean(in string namedboolean, out IBooleanDateStateSwitch oldest, out IBooleanDateStateSwitch mostRecent);
    public void GetTrueFalseRatio(in DateTime start, in DateTime to, in string namedboolean, double pourcentTrue);
    public void GetTrueFalseTimeInNanoseconds(in DateTime start, in DateTime to, in string namedboolean, out long nanoSecondTrue, out long nanoSecondFalse);
}



public interface INamedBooleanTimeSwitchRegisterSet
{

    public void CreateSlot(in string namedBoolean, in bool startValue);
    public void SetNow(in string namedBoolean, in bool value, in bool createIfNotExisting);
    public void SetNowWhenActionHappened(in DateTime whenItHappened,in string namedBoolean, in bool value, in bool createIfNotExisting);
    /// <summary>
    /// Be Carefull with this one because if you have two time the same switch that are following you can break the code of the developer below the interface or kill it performance.
    /// </summary>
    public void SetPastSwitchManually(in DateTime previousDate, in string namedBoolean, in bool newValue, in bool createIfNotExisting);
}

public enum BooleanSwithType : byte { FalseToTrue = 1, TrueToFalse = 0 }
public interface IBooleanDateStateSwitch
{
    public void WhenSwitchHappened(out long dateInTickTimeWhenSwitchHappened);
    public void WhenSwitchHappened(out DateTime whenSwitchHappened);
    public void GetBooleanStateSwitchType(out BooleanSwithType switchType);
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