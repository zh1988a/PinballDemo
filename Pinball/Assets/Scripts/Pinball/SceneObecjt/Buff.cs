using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public int LeftRound;
    public SkillConfigItem Skill;
    public Player Player;

    public Buff(SkillConfigItem skill)
    {
        Skill = skill;
        LeftRound = Skill.Round;
    }
}
