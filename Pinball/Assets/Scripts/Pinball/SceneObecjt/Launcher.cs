using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public float m_rotRange = 30;
    public float m_rotSpeed = 5;

    public float m_curRot = 0;
    public bool m_isToLeft = true;
    public bool m_isRoting = false;
    
    // Update is called once per frame
    void Update()
    {
        m_curRot += m_rotSpeed * Time.deltaTime * (m_isToLeft ? 1 : -1);
        //right max
        if(m_curRot <= -m_rotRange)
        {
            m_curRot = -m_rotRange;
            m_isToLeft = true;
        }
        //left max
        else if(m_curRot >= m_rotRange)
        {
            m_curRot = m_rotRange;
            m_isToLeft = false;
        }

        transform.localRotation = Quaternion.Euler(0, 0, m_curRot);
    }

    public void DoRot()
    {
        m_isRoting = true;
    }

    public void OnFire()
    {
        m_isRoting = false;
        GameManager.Instance.FireBall(new Vector2(transform.forward.x, transform.forward.y));
         //transform.forward
    }
}
