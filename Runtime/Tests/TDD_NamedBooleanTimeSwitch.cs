using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TDD_NamedBooleanTimeSwitch : MonoBehaviour
{
    public INamedBooleanTimeSwitchRegister m_register = new DefaultNamedBooleanTimeSwitchRegisterEasyCode();

    public int m_maxQueueLenght = 16;
    public string m_namedBool = "Space";
    public bool m_initialState = false;
    public bool m_isRegistered = false;
    public bool m_hasSwitch = false;
    public int m_switchCount = 0;

    public float m_timeRelativeToNowToTest = 10;
    public bool m_isDateValide;
    public bool m_stateAtIsTrue;
    public bool m_wasTrue;
    public bool m_wasFalse;

    public bool m_currentState;
    public bool m_moreRecentSwitch;
    public bool m_moreOldSwitch;
    public long m_moreRecentTimeNs;
    public long m_moreRecentTimeTick;
    public long m_moreRecentTimeMs;
    public long m_moreRecentTimeSeconds;


    public string m_nowDate;
    public string m_relativeDate;
    public string m_createdDate;
    public bool m_createdStateDate;

    public DebugSwitch m_mostRecentKey;
    public DebugSwitch m_mostOldKey;

    void Start()
    {
        m_register = new DefaultNamedBooleanTimeSwitchRegisterEasyCode(m_maxQueueLenght);
        m_register.CreateSlotIfNotExisting(in m_namedBool, in m_initialState);
        m_register.SetNow(in m_namedBool, in m_initialState);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            m_register.SetNow(in m_namedBool, true);

        if (Input.GetKeyUp(KeyCode.Space))
            m_register.SetNow(in m_namedBool, false);

        DateTime now = DateTime.Now;
        DateTime relativeTestTime = DateTime.Now.AddSeconds(-m_timeRelativeToNowToTest);
        m_nowDate = DateTime.Now.ToString();
        m_relativeDate = relativeTestTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt ") ; 
        m_register.GetStartExistingInitialState(in m_namedBool, out m_createdStateDate);
        m_register.GetStartExistingTime(in m_namedBool, out  DateTime createdDate);
        m_createdDate = createdDate.ToString();
        //Code later when focus
        //void GetStartExistingTime(in string namedboolean, out DateTime dateAtStartExisting);
        //Code later when focus
        //void GetStartExistingInitialState(in string namedboolean, out bool isTrueValueAtStartExisting);


        m_register.IsBooleanRegistered(in m_namedBool, out m_isRegistered);
        //void IsBooleanRegistered(in string namedboolean, out bool existing);

        m_register.HasAnySwitchHappened(in m_namedBool, out m_hasSwitch);
        //void HasAnySwitchHappened(in string namedboolean, out bool hasSomeRecord);

        m_register.GetStateNow(in m_namedBool, out  m_currentState);
        //void GetStateNow(in string namedboolean, out bool state);
        m_register.GetStateAt(in relativeTestTime, in m_namedBool, out m_stateAtIsTrue);
        //void GetStateAt(in DateTime atDate, in string namedboolean, out bool state);



        m_register.GetSwitchCount(in m_namedBool, out m_switchCount);
        //void GetSwitchCount(in string namedboolean, out int switchCount);

        m_register.GetLimitesSwitchOfBoolean(in m_namedBool, out IBooleanDateStateSwitch r, out IBooleanDateStateSwitch o);
        r.GetWhenSwitchHappened(out DateTime firstSwitchDate);
        m_mostRecentKey.Set(r);
        m_mostOldKey.Set(o);

        m_isDateValide = !m_register.IsDateTimeInInvalideTrackZone(in m_namedBool, in relativeTestTime);
        ///bool IsDateTimeInInvalideTrackZone(in m_namedBool, in relativeTestTime);
        if (m_isDateValide && m_hasSwitch)
        {
            m_moreRecentSwitch = r.TurnedTrue();
            m_moreOldSwitch = o.TurnedTrue(); 
            m_moreRecentTimeTick= now.Ticks - firstSwitchDate.Ticks;
            m_moreRecentTimeNs = (long)(m_moreRecentTimeTick * DateTimeTickUtility.tickValueInNanoseconds);
            m_moreRecentTimeMs = (long) (m_moreRecentTimeTick/DateTimeTickUtility.ratioTickInMilliseconds);
            m_moreRecentTimeSeconds = (long)(m_moreRecentTimeTick / DateTimeTickUtility.ratioTickInSeconds);


            m_wasTrue = m_register.WasTrueAt(in relativeTestTime, in m_namedBool);
            //bool WasTrueAt(in DateTime atDate, in string namedboolean); // Throw Exception if not found
            m_wasFalse = m_register.WasFalseAt(in relativeTestTime, in m_namedBool);
            //bool WasFalseAt(in DateTime atDate, in string namedboolean); // Throw Exception if not found


        }



        //void GetSwitchCountAndLimitBetween(in DateTime from, in DateTime to, in string namedboolean, out bool mostRecentValueStart, out bool mostOldValueStart, out int switchToTrue, out int switchToFalse);
        //void GetSwitchCountBetween(in DateTime from, in DateTime to, in string namedboolean, out int switchToTrue, out int switchToFalse);


        //void GetSegmentLimitsAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateSwitch switchMoreRecent, out IBooleanDateStateSwitch switchMostOld);

        //void GetElapsedTimeInNanoSecondsAt(in DateTime atDate, in string namedboolean, out long nanoSeconds, out DateTime switchOldestPart);
        //void GetAllSwitchDateBetween(in DateTime from, in DateTime to, in string namedboolean, out IBooleanDateStateSwitch[] sample);
        //void GetLimitesSwitchOfBoolean(in string namedboolean, out IBooleanDateStateSwitch mostRecent, out IBooleanDateStateSwitch switchMostOld);
        //void GetTrueFalseRatio(in DateTime from, in DateTime to, in string namedboolean, out double pourcentTrue);
        //void GetTrueFalseTimeInNanoseconds(in DateTime from, in DateTime to, in string namedboolean, out long nanoSecondTrue, out long nanoSecondFalse, out long nanoSecondTotalObserved);
        //void GetSegmentBetweenTwoSwitchAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateDualSwitchSegment segment);
        //void GetSegmentOldSwitchSideAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateSwitch switchKeyOld);
        //void GetSegmentRecentSwitchSideAt(in DateTime atDate, in string namedboolean, out IBooleanDateStateSwitch switchKeyRecent);


    }



    [System.Serializable]
    public class DebugSwitch
    {
        public long m_dateAsLongTick;
        public DateTime m_date;
        public string m_dateAsString;
        public BooleanSwithType m_type;
        public bool m_turnTrue;
        public bool m_wasTrue;

        public void Set(IBooleanDateStateSwitch switchKey)
        {
            switchKey.GetWhenSwitchHappened(out m_dateAsLongTick);
            switchKey.GetWhenSwitchHappened(out m_date);
            m_dateAsString = m_date.ToString();
            switchKey.GetSwitchType(out m_type);
            m_turnTrue = switchKey.TurnedTrue();
            m_wasTrue = switchKey.WasTrue();
        }
    }

}
