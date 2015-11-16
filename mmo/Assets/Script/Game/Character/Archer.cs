using UnityEngine;
using System.Collections;

public class Archer : PlayerChar {
    protected override void Attack()
    {
        base.Attack();
    }

    protected override void Damage()
    {

    }

    protected override void Attacking()
    {

    }

    protected override void Dead()
    {

    }

    protected override void EndOfAttack()
    {

    }

    protected override void Normal()
    {

    }

    protected override void Revive()
    {

    }

    public override bool UseSkill(int skillNumber, SkillBase skill)
    {
        // throw new System.NotImplementedException();
        return false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void Start()
    {
        base.Start();
    }
}
