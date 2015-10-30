using UnityEngine;
using System.Collections;

public class Rat : LoiteringEnemyBase {
    /// <summary>
    /// 名前を設定する
    /// </summary>
    protected override void SetName()
    {
        // ラットにする
        enemyName = "ラット";
    }

    protected override void RotateDirection()
    {

    }

    protected override void NearPlayerAction()
    {

    }

    protected override bool IsEndDamage()
    {
        return true;
    }

    protected override bool IsEndAttack()
    {
        return true;
    }
}