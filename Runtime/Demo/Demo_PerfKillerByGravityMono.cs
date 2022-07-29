using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo_PerfKillerByGravityMono : MonoBehaviour
{
    public Rigidbody m_rig;
    public float m_forcePower=1;
    public ForceMode m_forcemode;

    private void Start()
    {
        m_rig.AddForce(
            new Vector3(Random.value, Random.value, Random.value)
            * -m_forcePower*5, m_forcemode);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            GameObject.Instantiate(this);
        if (Input.GetMouseButtonDown(1)) {
            if (Random.value < 0.5f)
                Destroy(this.gameObject);
        }
        m_rig.AddForce(transform.localPosition * -m_forcePower, m_forcemode);
    }
}
