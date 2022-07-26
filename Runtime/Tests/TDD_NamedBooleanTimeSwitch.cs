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
    public long m_segmentTime;
    public double m_segmentTimeSeconds;

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

    public DebugSwitch m_mostRecentKeyAround;
    public DebugSwitch m_mostOldKeyAround;


    [Header("From To Action")]
    public float m_relativeFrom=0;
    public float m_relativeTo=3;
    public bool m_isOldRelativeValide ;
    public bool m_isRecentRelativeValide ;

    public int      m_switchCountR;
    public bool     m_isTrueAtRecentLimitR;
    public int      m_trueCountR;
    public int      m_falseCountR;
    public bool     m_isTrueAtOldLimitR;
    public double   m_trueSecondsObservedR;
    public double   m_falseSecondsObservedR;
    public double   m_totalSecondsObservedR;
    public double    m_percentTrueR;


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

        DateTime recentRelativeTimeBetween = now.AddSeconds(-m_relativeFrom);
        DateTime oldRelativeTimeBetween = now.AddSeconds(-m_relativeTo);


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

        m_isDateValide = m_register.IsDateTimeInValideTrackZone(in m_namedBool, in relativeTestTime);
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

            m_register.GetElapsedTimeInNanoSecondsAt(
                   in relativeTestTime,
                   in m_namedBool,
                   out m_segmentTime,
                   out DateTime switchOldestPart);
            DateTimeTickUtility.TickToSeconds(in m_segmentTime, out m_segmentTimeSeconds);



            if (m_switchCount > 1) {
                m_register.GetSegmentLimitsAt(in relativeTestTime, in  m_namedBool, out IBooleanDateStateSwitch switchKeyRecent, out IBooleanDateStateSwitch switchKeyOld );
                if (switchKeyOld == null) m_mostRecentKeyAround.ResetEmpty();
                else m_mostRecentKeyAround.Set(switchKeyOld);
                if (switchKeyRecent == null) m_mostOldKeyAround.ResetEmpty();
                else m_mostOldKeyAround.Set(switchKeyRecent);
            }
        }


        //////////////////
       

        if (oldRelativeTimeBetween.Ticks < createdDate.Ticks)
            oldRelativeTimeBetween = createdDate;
        if (recentRelativeTimeBetween.Ticks > now.Ticks)
            recentRelativeTimeBetween = now;

        m_register.GetAllSwitchDateBetween(in recentRelativeTimeBetween, in oldRelativeTimeBetween
           , m_namedBool, out IBooleanDateStateSwitch[] sampleBetween);

         m_isOldRelativeValide    =  m_register.IsDateTimeInValideTrackZone(in m_namedBool, in oldRelativeTimeBetween);
         m_isRecentRelativeValide = m_register.IsDateTimeInValideTrackZone(in m_namedBool, in recentRelativeTimeBetween);


        if (m_isRecentRelativeValide && m_isOldRelativeValide) { 
        
            m_switchCountR = sampleBetween.Length;
            if (sampleBetween.Length == 0)
            {
                m_trueCountR = 0;
                m_falseCountR = 0;
                m_register.GetStateAt(in recentRelativeTimeBetween,
                    in m_namedBool, out bool state);
                m_isTrueAtRecentLimitR = state;
                m_isTrueAtOldLimitR = state;
                DateTimeTickUtility.TickToSeconds(
                    recentRelativeTimeBetween.Ticks - oldRelativeTimeBetween.Ticks, out m_totalSecondsObservedR);
                m_trueSecondsObservedR = state ? m_totalSecondsObservedR : 0;
                m_falseSecondsObservedR = state ? 0 : m_totalSecondsObservedR;
                m_percentTrueR = state ? 1f : 0f;
            }
            else if (sampleBetween.Length > 0)
            {
                //void GetSwitchCountBetween(in DateTime from, in DateTime to, in string namedboolean, out int switchToTrue, out int switchToFalse);
                m_register.GetSwitchCountBetween(
                    in recentRelativeTimeBetween,
                    in oldRelativeTimeBetween,
                    in m_namedBool,
                    out m_trueCountR,
                    out m_falseCountR);
                //void GetSwitchCountAndLimitBetween(in DateTime from, in DateTime to, in string namedboolean, out bool mostRecentValueStart, out bool mostOldValueStart, out int switchToTrue, out int switchToFalse);
                m_register.GetSwitchCountAndLimitBetween(
                   in recentRelativeTimeBetween,
                   in oldRelativeTimeBetween,
                   in m_namedBool,
                   out m_isTrueAtRecentLimitR,
                   out m_isTrueAtOldLimitR,
                   out m_trueCountR,
                   out m_falseCountR);
            }

            //void GetTrueFalseTimeInNanoseconds(in DateTime from, in DateTime to, in string namedboolean, out long nanoSecondTrue, out long nanoSecondFalse, out long nanoSecondTotalObserved);
            m_register.GetTrueFalseTimeInTick(
               in recentRelativeTimeBetween,
               in oldRelativeTimeBetween,
               in m_namedBool,
               out long truetick,
               out long falsetick,
               out long totaltick
               );
            DateTimeTickUtility.TickToSeconds(in truetick, out m_trueSecondsObservedR);
            DateTimeTickUtility.TickToSeconds(in falsetick, out m_falseSecondsObservedR);
            DateTimeTickUtility.TickToSeconds(in totaltick, out m_totalSecondsObservedR);

            m_register.GetTrueFalseRatio(
                in recentRelativeTimeBetween,
               in oldRelativeTimeBetween,
               in m_namedBool, out m_percentTrueR);

        }

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

        public void ResetEmpty()
        {
            m_dateAsLongTick = 0;
            m_date = DateTime.Now;
            m_dateAsString="x";
            m_type = BooleanSwithType.TrueToFalse;
            m_turnTrue = false;
            m_wasTrue = false;
        }
    }

}
