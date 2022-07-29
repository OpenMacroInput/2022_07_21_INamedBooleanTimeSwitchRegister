using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo_ListenToFPSBoolSwitch : MonoBehaviour
{

    public BooleanCollectionBeanWihtDescription m_55fps;
    public BooleanCollectionBeanWihtDescription m_45fps;
    public BooleanCollectionBeanWihtDescription m_30fps;

    public float m_55fpsAsMS = 1f / 55f;
    public float m_45fpsAsMS = 1f / 45f;
    public float m_30fpsAsMS = 1f / 30f;
    public float m_deltaTime;

    public float m_refresh=0.1f;


    public void Start()
    {
        m_55fps.Init();
        m_45fps.Init();
        m_30fps.Init();
        InvokeRepeating("RefreshDescription", 0, m_refresh);
        InvokeRepeating("UpdateFPS", 0, m_timeBetweenCheck);
        frame = Time.frameCount;
    }
    public float m_timeBetweenCheck=0.2f;
    public int frame = 0;
    public int frameCount = 0;
    public int fps = 0;
    public void UpdateFPS()
    {
         int newFrame = Time.frameCount;
         frameCount = newFrame - frame;
         fps =(int) (frameCount /m_timeBetweenCheck);
        //m_deltaTime = Time.deltaTime;
        //m_55fps.m_history.SetNow(m_deltaTime > m_55fpsAsMS);
        //m_45fps.m_history.SetNow(m_deltaTime > m_45fpsAsMS);
        //m_30fps.m_history.SetNow(m_deltaTime > m_30fpsAsMS);
        m_55fps.m_history.SetNow(fps > 55f);
        m_45fps.m_history.SetNow(fps > 45f);
        m_30fps.m_history.SetNow(fps > 30f);
        frame = newFrame;
    }
    public void RefreshDescription() {
        m_55fps.Refresh();
        m_45fps.Refresh();
        m_30fps.Refresh();
    }
}


[System.Serializable]
public class BooleanCollectionBean : INamedBooleanTimeSwitchCollectionHolder
{
    public int m_maxKey = 64;
    public bool m_startValue = false;
    public BooleandDateSwitchCollectionDefault m_history= new BooleandDateSwitchCollectionDefault(64, false);
    public bool m_isInit;
    public void ResetWith(int maxKey, bool startValue) {
        m_maxKey = maxKey;
        m_startValue = startValue;
    }
    public void Init() {
        if (!m_isInit) {
            m_isInit = true;
            m_history = new BooleandDateSwitchCollectionDefault(m_maxKey, m_startValue);
        }
    }

    public void GetCollection(out INamedBooleanTimeSwitchCollection collection)
    {
        Init();
        collection = m_history;
    }

    public INamedBooleanTimeSwitchCollection GetCollection()
    {
        Init();
        return m_history;
    }
}
[System.Serializable]
public class BooleanCollectionBeanWihtDescription : BooleanCollectionBean
{
    public string m_descriptionLine;
    public string m_descriptionTime;

    public void Refresh()
    {
        NamedBooleanTimeSwitchStringUtility.GetStringDescriptionByMillisecondsSegment(
            m_history, out m_descriptionTime);
        NamedBooleanTimeSwitchStringUtility.GetStringDescriptionCharPerSeconds(
            m_history, out m_descriptionLine);
    }

}
