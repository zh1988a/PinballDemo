using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float m_speed;
    public float m_accelerate;

    public float m_baseAttack;
   

    public Vector2 m_direction;

    public bool m_isOnfire = false;

    private void Update()
    {
        if(m_isOnfire)
        {
            backTime += Time.deltaTime;
            UpdatePosition();
        }
    }

    private void FixedUpdate()
    {
        if (m_isOnfire)
        {
            UpdatePosition();
        }
    }

    public void UpdatePosition()
    {
        float moveX = m_direction.x * m_speed * Time.deltaTime;
        float moveY = m_direction.y * m_speed * Time.deltaTime;
        transform.localPosition = new Vector3(transform.localPosition.x + moveX, transform.localPosition.y + moveY, 0);
    }

    float backTime = 0;
    public void OnTriggerEnter(Collider other)
    {
        int layer = other.gameObject.layer;
        if(layer == LayerMask.NameToLayer(LayerName.TopWall))
        {
            if(GameManager.Instance.IsPlayerRound)
            {
                //damage
                DoDamage(false);
            }
            else
            {
                //release
                if(backTime > 1)
                {
                    DoRelease();
                }
            }
        }
        else if(layer == LayerMask.NameToLayer(LayerName.BottomWall))
        {
            if (GameManager.Instance.IsPlayerRound)
            {
                //release
                if (backTime > 1)
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
            bool isHigher = transform.localPosition.y > other.transform.localPosition.y;
            bool isRighter = transform.localPosition.x > other.transform.localPosition.x;
            float vertDis = Mathf.Abs(transform.localPosition.y - other.transform.localPosition.y);
            float horDis = Mathf.Abs(transform.localPosition.x - other.transform.localPosition.x);

            SpriteRenderer sp = other.gameObject.GetComponent<SpriteRenderer>();
            bool useVert = (vertDis - sp.size.y/2) >= (horDis - sp.size.x/2);

            Vector2 normal;
            if(useVert)
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
            AddSkill(skill.m_data);
            other.gameObject.SetActive(false);
        }

    }

    public void DoReflect(Vector2 normal)
    {
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
    }

    public void DoDamage(bool isPlayer)
    {
        m_isOnfire = false;
        GameManager.Instance.DoDamage(this);
    }

    public void DoRelease()
    {
        m_isOnfire = false;
        GameManager.Instance.DoRelease(this);
    }


    public float m_damage
    {
        get
        {
            return m_baseAttack;
        }
    }

    public float m_addAttack = 0;
    public float m_crit = 0;
    public float m_critDamage = 0;
    public float m_vampire = 0;

    //public float m_heal
    
    public void AddSkill(SkillConfigItem skill)
    {
        //switch(skill.Type)
        //{
        //    case SkillType.Attack:
        //        m_addAttack += skill.Value;
        //        break;
        //    case SkillType.Crit:
        //        m_crit += skill.Percent;
        //        m_critDamage += skill.Value;
        //        break;
        //    case SkillType.Vampire:
        //        m_vampire += skill.Value;
        //        break;
        //    case SkillType.Gas:

        //        break;
        //}
    }
}
