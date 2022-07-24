//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class DefaultNamedBooleanTimeSwitchRegisterDraft
//{
//    public int m_maxKeyPerList = 1024;
//    public Dictionary<string, BooleanDateStateSwitchClampList> m_register = new Dictionary<string, BooleanDateStateSwitchClampList>();

//    public void CreateSlotIfNotExisting(in string namedBoolean, in bool startValue)
//    {
//        if (!m_register.ContainsKey(namedBoolean))
//        {
//            m_register.Add(namedBoolean, new BooleanDateStateSwitchClampList() { m_maxKey = m_maxKeyPerList });
//            m_register[namedBoolean].SetWithNow(startValue);
//        }
//    }
//    public bool IsContaining(in string namedBoolean) { return m_register.ContainsKey(namedBoolean); }
//    public bool IsNotContaining(in string namedBoolean) { return !m_register.ContainsKey(namedBoolean); }

//    public void GetAllSwitchDateBetween(in DateTime start, in DateTime to, in string namedboolean, out IBooleanDateStateSwitch[] sample)
//    {
//        if (IsNotContaining(in namedboolean))
//        {
//            sample = new IBooleanDateStateSwitch[0];
//            return;
//        }
//        else
//        {
//            m_register[namedboolean].GetAllSwitchDateBetween(in start, in to, out sample);
//        }
//    }


//    public void GetLimitesSwitchOfBoolean(in string namedboolean, out IBooleanDateStateSwitch oldest, out IBooleanDateStateSwitch mostRecent)
//    {
//        oldest = null;
//        mostRecent = null;
//        if (IsNotContaining(in namedboolean))
//            return;
//        m_register[namedboolean].GetMostRecent(out BooleanDateStateSwitch recent);
//        m_register[namedboolean].GetMostFarInTime(out BooleanDateStateSwitch old);
//        oldest = old;
//        mostRecent = recent;
//    }



//    public void GetStateNow(in string namedboolean, out bool existing, out bool state)
//    {
//        if (IsNotContaining(in namedboolean))
//        {
//            existing = false;
//            state = false;
//        }
//        else
//        {
//            existing = true;
//            state = m_register[namedboolean].GetState();
//        }
//    }


//    public void SetNow(in string namedBoolean, in bool value, in bool createIfNotExisting)
//    {
//        if (IsContaining(in namedBoolean))
//        {
//            m_register[namedBoolean].SetWithNow(value);
//        }
//        else if (createIfNotExisting)
//        {
//            CreateSlotIfNotExisting(in namedBoolean, in value);
//            m_register[namedBoolean].SetWithNow(value);

//        }
//    }
//    public void SetNowWhenActionHappened(in DateTime whenItHappened, in string namedBoolean, in bool value, in bool createIfNotExisting)
//    {
//        m_register[namedBoolean].SetWithNow(whenItHappened, value);
//    }

//    public void IsBooleanRegistered(in string namedboolean, out bool existing)
//    {
//        existing = m_register.ContainsKey(namedboolean);
//    }

//    public void HasAnySwitchHappened(in string namedboolean, out bool hasSomeRecord)
//    {
//        hasSomeRecord = m_register[namedboolean].GetSwitchCount() > 0;
//    }

//    public void GetSwitchCount(in string namedboolean, out int switchCount)
//    {
//        m_register[namedboolean].GetSwitchCount(out switchCount);
//    }

//    public bool IsDateTimeInInvalideTrackZone(in string namedboolean, in DateTime date)
//    {
//        m_register[namedboolean].GetMostFarInTime(out BooleanDateStateSwitch l);
//        return date.Ticks < l.WhenSwitchHappenedLong();
//    }



//    public void GetSwitchCount(in DateTime start, in DateTime to, in string namedboolean,
//        out bool startValue, out bool endValue, out int switchToTrue, out int switchToFalse)
//    {
//        switchToFalse = 0;
//        switchToTrue = 0;
//        //SHOULD BE IN UTILITY.d
//        BooleanDateStateSwitchClampList t = m_register[namedboolean];
//        t.GetState(out startValue);
//        t.GetOldestState(out endValue);
//        t.GetAllSwitchDateBetween(in start, in to, out IBooleanDateStateSwitch[] sample);
//        for (int i = 0; i < sample.Length; i++)
//        {
//            sample[i].GetSwitchType(out BooleanSwithType s);
//            if (s == BooleanSwithType.TrueToFalse)
//                switchToFalse++;
//            else switchToTrue++;
//        }
//    }

//    public void GetStateNow(in string namedboolean, out bool state)
//    {
//        GetStateAt(DateTime.Now, in namedboolean, out state);
//    }






//    public class BooleanDateStateSwitchClampList
//    {
//        public List<BooleanDateStateSwitch> m_listRecentToPast = new List<BooleanDateStateSwitch>();
//        public int m_maxKey = 1024;

//        public void SetWithNow(bool isTrue)
//        {
//            SetWithNow(DateTime.Now, isTrue);
//        }

//        public void SetWithNow(DateTime date, bool isTrue)
//        {
//            DateTime now = DateTime.Now;
//            if (m_listRecentToPast.Count == 0)
//            {
//                m_listRecentToPast.Add(new BooleanDateStateSwitch(now, isTrue));
//            }
//            else if (m_listRecentToPast.Count >= m_maxKey)
//            {
//                BooleanDateStateSwitch reused = m_listRecentToPast[m_listRecentToPast.Count - 1];
//                m_listRecentToPast.RemoveAt(m_listRecentToPast.Count - 1);
//                reused.SetValue(now);
//                reused.SetWitchType(isTrue);
//                m_listRecentToPast.Insert(0, reused);
//            }
//            else
//            {
//                m_listRecentToPast.Insert(0, new BooleanDateStateSwitch(now, isTrue));
//            }
//        }

//        public void GetSwitchCount(out int count) => count = m_listRecentToPast.Count;
//        public int GetSwitchCount() { return m_listRecentToPast.Count; }
//        public void GetMostRecent(out BooleanDateStateSwitch first)
//        {
//            if (m_listRecentToPast.Count == 0)
//                throw new Exception("");
//            first = m_listRecentToPast[0];

//        }
//        public void GetMostFarInTime(out BooleanDateStateSwitch last)
//        {
//            {
//                if (m_listRecentToPast.Count == 0)
//                    throw new Exception("");
//                last = m_listRecentToPast[m_listRecentToPast.Count - 1];
//            }
//        }

//        public void GetAllSwitchDateBetween(in DateTime from, in DateTime to, out IBooleanDateStateSwitch[] sample)
//        {
//            long s = from.Ticks, t = to.Ticks;
//            if (s < t)
//            {
//                long tmp = s;
//                s = t;
//                t = tmp;
//            }
//            sample = m_listRecentToPast
//                .Where(k => k.WhenSwitchHappenedLong() >= s && k.WhenSwitchHappenedLong() <= t)
//                .OrderByDescending(k => k.WhenSwitchHappenedLong()).ToArray();
//        }
//        public void GetAllSwitchDateBetween(in DateTime from, in DateTime to, out BooleanDateStateSwitch[] sample)
//        {
//            long s = from.Ticks, t = to.Ticks;
//            if (s < t)
//            {
//                long tmp = s;
//                s = t;
//                t = tmp;
//            }
//            sample = m_listRecentToPast
//                .Where(k => k.WhenSwitchHappenedLong() >= s && k.WhenSwitchHappenedLong() <= t)
//                .OrderByDescending(k => k.WhenSwitchHappenedLong()).ToArray();
//        }
//        public void GetState(out bool startValue)
//        {
//            PushCantBeZeroExceptionIfNeeded();
//            startValue = m_listRecentToPast[0].IsTrue();

//        }
//        public bool GetState()
//        {
//            PushCantBeZeroExceptionIfNeeded();
//            return m_listRecentToPast[0].IsTrue();

//        }
//        public void GetOldestState(out bool endValue)
//        {
//            PushCantBeZeroExceptionIfNeeded();
//            endValue = m_listRecentToPast[m_listRecentToPast.Count - 1].IsTrue();
//        }

//        private void PushCantBeZeroExceptionIfNeeded()
//        {
//            if (m_listRecentToPast.Count == 0)
//                throw new Exception("Can't be zero when accessing");
//        }


//        public bool IsTrueOnRecentPart(in DateTime date, in BooleanDateStateSwitch target)
//        {
//            long d = date.Ticks;
//            if (d >= target.WhenSwitchHappenedLong())
//            {
//                target.GetSwitchType(out BooleanSwithType t);
//                return t == BooleanSwithType.TrueToFalse;

//            }
//            else
//            {
//                target.GetSwitchType(out BooleanSwithType t);
//                return !(t == BooleanSwithType.TrueToFalse);
//            }
//        }

//        public void GetStateAt(in DateTime date, out bool state)
//        {
//            PushCantBeZeroExceptionIfNeeded();
//            long d = date.Ticks;
//            if (m_listRecentToPast.Count() == 1)
//            {
//                BooleanDateStateSwitch val = m_listRecentToPast[0];
//                state = IsTrueOnRecentPart(in date, in val);
//            }
//            else
//            {
//                GetLimitsOfStateAt(in date, out BooleanDateStateSwitch recent, out BooleanDateStateSwitch oldest);
//                oldest.GetSwitchType(out BooleanSwithType type);
//                state = type == BooleanSwithType.FalseToTrue;
//            }

//        }

//        public void GetLimitsOfStateAt(in DateTime date, out DateTime moreRecent, out DateTime moreOldest)
//        {
//            //please check beforeif date out of key
//            PushCantBeZeroExceptionIfNeeded();
//            GetLimitsOfStateAt(in date, out BooleanDateStateSwitch recent, out BooleanDateStateSwitch oldest);
//            moreRecent = recent.WhenSwitchHappenedDate();
//            moreOldest = oldest.WhenSwitchHappenedDate();
//        }
//        public void GetLimitsOfStateAt(in DateTime date, out BooleanDateStateSwitch moreRecent, out BooleanDateStateSwitch moreOldest)
//        {
//            // i<n<i+1
//            //Recent < Date < Old
//            //999 < 500 < 200

//            PushCantBeZeroExceptionIfNeeded();
//            if (m_listRecentToPast.Count == 1)
//            {
//                moreRecent = m_listRecentToPast[0];
//                moreOldest = m_listRecentToPast[0];
//                return;
//            }

//            long t = date.Ticks;
//            for (int i = 0; i < m_listRecentToPast.Count - 1; i++)
//            {
//                if (t <= m_listRecentToPast[i].WhenSwitchHappenedLong()
//                    && t > m_listRecentToPast[i + 1].WhenSwitchHappenedLong())
//                {
//                    moreRecent = m_listRecentToPast[i];
//                    moreOldest = m_listRecentToPast[i + 1];
//                    return;
//                }
//            }
//            moreRecent = m_listRecentToPast[m_listRecentToPast.Count - 1];
//            moreOldest = m_listRecentToPast[m_listRecentToPast.Count - 1];
//        }

//        public void IsTimeMoreRecentThatKeys(in DateTime time, out bool mostRecent)
//        {
//            PushCantBeZeroExceptionIfNeeded();
//            mostRecent = time.Ticks > m_listRecentToPast[0].WhenSwitchHappenedLong();
//        }
//        public void IsTimeMoreOlderThatKeys(in DateTime time, out bool mostOlder)
//        {
//            PushCantBeZeroExceptionIfNeeded();
//            mostOlder = time.Ticks < m_listRecentToPast[m_listRecentToPast.Count - 1].WhenSwitchHappenedLong();
//        }

//        public void SortCollection()
//        {
//            m_listRecentToPast = m_listRecentToPast.OrderByDescending(k => k.WhenSwitchHappenedLong()).ToList();
//        }


//        public void GetElapsedTimeInNanoSecondsAt(in DateTime date, out bool found, out long nanoSeconds, out DateTime switchEnd)
//        {
//            GetSegmentAt(in date, out found, out int index, out BooleanDateStateSwitch t);
//            if (!found)
//                throw new Exception("No switch found");
//            switchEnd = t.WhenSwitchHappenedDate();
//            GetSegmentInfoOf(in date, out nanoSeconds, out bool state);
//        }

//        public void GetTrueFalseRatio(in DateTime start, in DateTime to, out double pourcentTrue)
//        {
//            GetTrueFalseTimeInNanoseconds(start, to, out long nanoTrue, out long nanoFalse, out long total);
//            pourcentTrue = ((double)nanoTrue) / ((double)total);
//        }

//        public void GetTrueFalseTimeInNanoseconds(DateTime startRecent, DateTime toOlder, out long nanoSecondTrue, out long nanoSecondFalse, out long nanoSecondsTotalObserved)
//        {

//            if (startRecent < toOlder)
//            {
//                DateTime tmp = startRecent;
//                startRecent = toOlder;
//                toOlder = tmp;
//            }

//            GetAllSwitchDateBetween(in startRecent, in toOlder, out BooleanDateStateSwitch[] sample);
//            long startRecentLong = startRecent.Ticks;
//            long startOldLong = toOlder.Ticks;
//            nanoSecondTrue = startRecentLong - startOldLong;
//            nanoSecondsTotalObserved = startRecentLong - startOldLong;


//            for (int i = 1; i < sample.Length - 1; i++)
//            {
//                GetSegmentInfoOf(in sample[i], in sample[i + 1], out long tickTime, out bool isTrueState);
//                if (!isTrueState)
//                {
//                    nanoSecondTrue -= tickTime;
//                }
//            }
//            BooleanDateStateSwitch start = sample[0];
//            if (sample[0].IsFalse())
//            {
//                nanoSecondTrue -= startRecent.Ticks - sample[0].WhenSwitchHappenedLong();
//            }
//            if (sample[sample.Length - 1].IsTrue())
//            {
//                nanoSecondTrue -= sample[sample.Length - 1].WhenSwitchHappenedLong() - toOlder.Ticks;
//            }
//            nanoSecondFalse = nanoSecondsTotalObserved - nanoSecondTrue;
//        }

//        public void SetPastSwitchManually(in DateTime dateToInject, in bool newValue)
//        {
//            int c = m_listRecentToPast.Count;
//            long l = dateToInject.Ticks;
//            if (c == 0)
//            {
//                SetWithNow(dateToInject, newValue);
//                return;
//            }
//            for (int i = 0; i < c; i++)
//            {
//                if (m_listRecentToPast[i].WhenSwitchHappenedLong() > l)
//                {
//                    m_listRecentToPast.Insert(i, new BooleanDateStateSwitch(dateToInject, newValue));
//                    return;
//                }
//            }
//        }
//        public bool WasTrueAt(in DateTime date)
//        {
//            GetSegmentInfoOf(in date, out bool state);
//            return state;
//        }

//        public bool WasFalseAt(in DateTime date)
//        {
//            GetSegmentInfoOf(in date, out bool state);
//            return !state;
//        }
//        public void GetSegmentInfoOf(in DateTime date, out bool stateIsTrue)
//        {
//            GetSegmentAt(in date, out bool found, out int index, out BooleanDateStateSwitch target);

//            if (index > 1)
//            {
//                stateIsTrue = target.GetBooleanStateSwitchType() == BooleanSwithType.TrueToFalse;
//                return;
//            }
//            else if (index == 1)
//            {
//                stateIsTrue = target.GetBooleanStateSwitchType() == BooleanSwithType.TrueToFalse;
//                return;
//            }
//            throw new Exception("Cant use if zero key");
//        }

//        public void GetSergmentInfoOf(in int indexOld, out long tickTime, out bool state)
//        {
//            GetSegmentInfoOf(indexOld - 1, indexOld, out tickTime, out state);
//        }
//        public void GetSegmentInfoOf(in int IndexRecent, in int indexOld, out long tickTime, out bool state)
//        {
//            GetSegmentInfoOf(m_listRecentToPast[IndexRecent], m_listRecentToPast[indexOld], out tickTime, out state);
//        }
//        public void GetSegmentInfoOf(in BooleanDateStateSwitch recent, in BooleanDateStateSwitch old, out long tickTime, out bool state)
//        {
//            tickTime = recent.WhenSwitchHappenedLong() - old.WhenSwitchHappenedLong();
//            state = old.GetBooleanStateSwitchType() == BooleanSwithType.FalseToTrue;
//        }

//        public void GetSegmentInfoOf(in DateTime date, out long tickTimeOne, out bool state)
//        {
//            GetSegmentAt(in date, out bool found, out int index, out BooleanDateStateSwitch target);

//            if (index > 1)
//            {
//                GetSegmentIndexAt(index - 1, out BooleanDateStateSwitch inFrontRecentOne);
//                tickTimeOne = inFrontRecentOne.WhenSwitchHappenedLong() - target.WhenSwitchHappenedLong();
//                state = target.GetBooleanStateSwitchType() == BooleanSwithType.TrueToFalse;
//                return;
//            }
//            else if (index == 1)
//            {
//                tickTimeOne = (DateTime.Now.Ticks - target.WhenSwitchHappenedLong());
//                state = target.GetBooleanStateSwitchType() == BooleanSwithType.TrueToFalse;
//                return;
//            }
//            throw new Exception("Cant use if zero key");
//        }

//        public void GetSegmentIndexAt(in int index, out BooleanDateStateSwitch target)
//        {
//            target = m_listRecentToPast[index];
//        }


//        public void GetSegmentAt(in DateTime date, out bool foundMatch, out int index, out BooleanDateStateSwitch target)
//        {
//            long t = date.Ticks;
//            if (m_listRecentToPast.Count == 0)
//            {
//                target = null;
//                foundMatch = false;
//                index = -1;
//                return;
//            }
//            else if (m_listRecentToPast.Count == 1)
//            {
//                if (t >= m_listRecentToPast[0].WhenSwitchHappenedLong())
//                {
//                    target = m_listRecentToPast[0];
//                    foundMatch = true;
//                    index = 0;
//                    return;
//                }
//                else
//                {
//                    target = null;
//                    foundMatch = false;
//                    index = -1;
//                    return;
//                }
//            }
//            else
//            {
//                for (int i = 0; i < m_listRecentToPast.Count - 1; i++)
//                {
//                    if (t < m_listRecentToPast[i].WhenSwitchHappenedLong()
//                        && t >= m_listRecentToPast[i + 1].WhenSwitchHappenedLong())
//                    {
//                        target = m_listRecentToPast[i + 1];
//                        foundMatch = true;
//                        index = i + 1;
//                        return;
//                    }
//                }
//            }
//            target = null;
//            foundMatch = false;
//            index = -1;
//            return;
//        }
//    }
//}
