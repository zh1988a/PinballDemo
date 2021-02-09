using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float m_speed;
    public float m_accelerate;

    public int m_baseAttack;
   

    public Vector2 m_direction;

    public bool m_isOnfire = false;
    private bool m_isPerfect = false;

    private void Update()
    {
        //if(m_isOnfire)
        //{
        //    backTime += Time.deltaTime;
        //    UpdatePosition();
        //}
    }

    private void FixedUpdate()
    {
        if (m_isOnfire)
        {
            backTime += Time.fixedDeltaTime;
            UpdatePosition();
        }
    }

    public void UpdatePosition()
    {
        float moveX = m_direction.x * m_speed * Time.fixedDeltaTime;
        float moveY = m_direction.y * m_speed * Time.fixedDeltaTime;
        transform.localPosition = new Vector3(transform.localPosition.x + moveX, transform.localPosition.y + moveY, 0);
    }

    float backTime = 0;
    public void OnTriggerEnter(Collider other)
    {
        //m_isInWall = true;
        Trigger(other);
    }

    //private bool m_isInWall = false;
    //public void OnTriggerStay(Collider other)
    //{
    //    if(!m_isInWall)
    //    {
    //        m_isInWall = true;
    //        Trigger(other);
    //    }
    //}

    //public void OnTriggerExit(Collider other)
    //{
    //    m_isInWall = false;
    //}

    public void Trigger(Collider other)
    {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer(LayerName.TopWall))
        {
            if (GameManager.Instance.IsPlayerRound)
            {
                //damage
                DoDamage(false);
            }
            else
            {
                //release
                if (backTime > 0.1)
                {
                    DoRelease();
                }
            }
        }
        else if (layer == LayerMask.NameToLayer(LayerName.BottomWall))
        {
            if (GameManager.Instance.IsPlayerRound)
            {
                //release
                if (backTime > 0.1)
                {
                    DoRelease();
                };
            }
            else
            {
                //damage
                DoDamage(true);
            }
        }
        else if (layer == LayerMask.NameToLayer(LayerName.LeftWall))
        {
            //set dir
            DoReflect(Vector2.right);
        }
        else if (layer == LayerMask.NameToLayer(LayerName.RightWall))
        {
            //set dir
            DoReflect(Vector2.left);
        }
        else if (layer == LayerMask.NameToLayer(LayerName.Obstacle))
        {
            //check pos
            bool isHigher = transform.position.y > other.transform.position.y;
            bool isRighter = transform.position.x > other.transform.position.x;
            float vertDis = Mathf.Abs(transform.position.y - other.transform.position.y);
            float horDis = Mathf.Abs(transform.position.x - other.transform.position.x);

            SpriteRenderer sp = other.gameObject.GetComponent<SpriteRenderer>();
            bool useVert = (vertDis - sp.size.y / 2) >= (horDis - sp.size.x / 2);

            Vector2 normal;
            if (useVert)
            {
                normal = isHigher ? Vector2.up : Vector2.down;
            }
            else
            {
                normal = isRighter ? Vector2.right : Vector2.left;
            }
            //set dir
            DoReflect(normal);
        }
        else if (layer == LayerMask.NameToLayer(LayerName.Skill))
        {
            //get skill
            SkillObject skill = other.GetComponent<SkillObject>();
            m_skills.Add(skill.m_data);
            other.gameObject.SetActive(false);

            if(m_skills.Count >= 4)
            {
                GameManager.Instance.GetTrails(m_skills.Count - 3, this);
            }
            

            if(m_skills.Count == 6)
            {
                m_isPerfect = true;
                GameManager.Instance.m_ingameUI.ShowPerfect(true);
            }
        }
    }

    public void DoReflect(Vector2 normal)
    {
        AudioPlayer.Instance.PlayCollid();
        m_direction = Vector2.Reflect(m_direction, normal);
    }

    public void SetDirection(Vector2 dir)
    {
        m_direction = new Vector2(dir.x, dir.y);
    }

    public void Fire(Vector2 dir)
    {
        m_baseAttack = Defines.Instance.BaseDamage;

        m_direction = dir;
        m_isOnfire = true;
        backTime = 0;
        m_skills.Clear();
        m_isPerfect = false;
    }

    public void DoDamage(bool isPlayer)
    {
        if(m_isPerfect)
        {
            AudioPlayer.Instance.PlayPerfect();
        }
        else
        {
            AudioPlayer.Instance.PlayDamage();
        }
        m_isOnfire = false;
        GameManager.Instance.DoDamage(this);
    }

    public void DoRelease()
    {
        m_isOnfire = false;
        GameManager.Instance.DoRelease(this);
    }


    public int m_damage
    {
        get
        {
            return m_baseAttack;
        }
    }

    public List<SkillConfigItem> m_skills = new List<SkillConfigItem>();

}
