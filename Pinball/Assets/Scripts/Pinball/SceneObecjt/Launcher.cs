using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public float m_rotRange = 30;
    public float m_rotSpeed = 5;
    public Transform Turret;

    public float m_curRot = 0;
    public bool m_isToLeft = true;
    public bool m_isRoting = false;
    public bool m_isInAI = false;
    
    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        m_curRot += m_rotSpeed * Time.fixedDeltaTime * (m_isToLeft ? 1 : -1);
        //right max
        if (m_curRot <= -m_rotRange)
        {
            m_curRot = -m_rotRange;
            m_isToLeft = true;
        }
        //left max
        else if (m_curRot >= m_rotRange)
        {
            m_curRot = m_rotRange;
            m_isToLeft = false;
        }

        Turret.localRotation = Quaternion.Euler(0, 0, m_curRot);

        if (m_isInAI)
        {
            Vector3 ori = new Vector3(GameManager.Instance.m_npcFirePos.position.x, GameManager.Instance.m_npcFirePos.position.y, 0);
            Vector3 tar = new Vector3(Turret.up.x, Turret.up.y, 0);
            //Debug.DrawRay(ori, tar, Color.red);
            Ray ray = new Ray(ori, tar);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit/*,100,LayerMask.NameToLayer("FirePoint")*/))
            {
                //Debug.Log("")
                if(hit.collider.gameObject == GameManager.Instance.m_curMap.TargetPoint.gameObject)
                {
                    OnFire();
                }
            }
        }

        
    }

    public void DoRot()
    {
        m_isRoting = true;
    }

    public void OnFire()
    {
        m_isRoting = false;
        m_isInAI = false;
        GameManager.Instance.FireBall(new Vector2(Turret.up.x, Turret.up.y));
        //transform.forward
    }

    public void StartAI()
    {
        if(GameManager.Instance.IsPlayerRound)
        {
            return;
        }

        m_isInAI = true;
    }
}
