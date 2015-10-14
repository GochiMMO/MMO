using UnityEngine;
using System.Collections;

/// <summary>
/// Item kind.
/// </summary>
public enum KIND_ITEM
{
    ITEM = 0,
    EQUIP = 1
}


/// <summary>
/// All item have this struct.
/// </summary>
public struct ItemData
{
    public int id;          // 識別番号
    public KIND_ITEM kind;  // アイテムの種類
    public string name;     // アイテムの名前
    public int cost;        // 価格
    public Sprite image;    // 画像

    /// <summary>
    /// Over ride method. Get item name.
    /// </summary>
    /// <returns>Item name.</returns>
    public override string ToString()
    {
        return this.name;
    }
}

/// <summary>
/// Item and equip items base class.
/// </summary>
public class ItemEquipBase
{
    protected ItemData itemData;    // 基本構造体

    /// <summary>
    /// Constractor.
    /// </summary>
    /// <param name="id">Identification number.</param>
    /// <param name="kind">Item kind.</param>
    /// <param name="name">Item name.</param>
    /// <param name="cost">Item buy cost.</param>
    protected ItemEquipBase(int id, KIND_ITEM kind, string name, int cost, string imageName)
    {
        itemData.id = id;
        itemData.kind = kind;
        itemData.name = name;
        itemData.cost = cost;
        // アイテムデータの読み込み
        switch (kind)
        {
            case KIND_ITEM.EQUIP:
                itemData.image = Resources.Load<Sprite>("Asset/Sprite/MainGame/UI/Equip/" + imageName);
                break;
            case KIND_ITEM.ITEM:
                itemData.image = Resources.Load<Sprite>("Asset/Sprite/MainGame/UI/Items" + imageName);
                break;
        }
    }

    /// <summary>
    /// Constractor.
    /// </summary>
    /// <param name="itemData">Item basic struct.</param>
    protected ItemEquipBase(ref ItemData itemData)
    {
        this.itemData = itemData;
    }
}

/// <summary>
/// All equip item base class.
/// </summary>
public class EquipBase : ItemEquipBase{
    
    protected int attack;           // 攻撃力
    protected int defense;          // 防御力
    protected char equipJob;        // 装備できるジョブ

    /// <summary>
    /// Constractor.
    /// </summary>
    /// <param name="attack">Attack value.</param>
    /// <param name="defense">Defense value.</param>
    /// <param name="equipJob">Can equip job bit flag.</param>
    protected EquipBase(int id, KIND_ITEM kind, string name, string imageName, int cost, int attack, int defense, char equipJob)
        : base(id, kind, name, cost, imageName)
    {
        this.attack = attack;
        this.defense = defense;
        this.equipJob = equipJob;
    }

    /// <summary>
    /// Constractor.
    /// </summary>
    /// <param name="itemData">Item data.</param>
    /// <param name="attack">Attack value.</param>
    /// <param name="defense">Defense value.</param>
    /// <param name="equipJob">Can equip job bit flag.</param>
    protected EquipBase(ref ItemData itemData, int attack, int defense, char equipJob)
        : base(ref itemData)
    {
        this.attack = attack;       // 攻撃力
        this.defense = defense;     // 防御力
        this.equipJob = equipJob;   // 装備できるジョブ
    }

    /// <summary>
    /// Can equip this equipment.
    /// </summary>
    /// <param name="jobNumber">Job number.</param>
    /// <returns>able or diseable.</returns>
    protected bool IsEquip(int jobNumber)
    {
        return ((equipJob & (1 << jobNumber)) != 0);
    }
}

/// <summary>
/// All item's base class.
/// </summary>
public class ItemBase : ItemEquipBase
{
    protected int hpRecoveryValue;  // HPの回復量
    protected int spRecoveryValue;  // SPの回復量

    /// <summary>
    /// Constractor.
    /// </summary>
    /// <param name="itemData">Item data struct.</param>
    /// <param name="hpRecoveryValue">Hp recovery value.</param>
    /// <param name="spRecoveryValue">Sp recovery value.</param>
    public ItemBase(ref ItemData itemData, int hpRecoveryValue, int spRecoveryValue)
        : base(ref itemData)
    {
        this.itemData = itemData;       // アイテムの構造体
        this.hpRecoveryValue = hpRecoveryValue;     // HPの回復量
        this.spRecoveryValue = spRecoveryValue;     // SPの回復量
    }

    /// <summary>
    /// Constractor.
    /// </summary>
    /// <param name="id">Identification number</param>
    /// <param name="hpRecoveryValue">Hp recovery value.</param>
    /// <param name="spRecoveryValue">Sp recovery value.</param>
    public ItemBase(int id, KIND_ITEM kind, string name, string imageName, int cost, int hpRecoveryValue, int spRecoveryValue)
        : base(id, kind, name, cost, imageName)
    {
        this.itemData.id = id;      // アイテムの識別番号
        this.hpRecoveryValue = hpRecoveryValue; // HPの回復量
        this.spRecoveryValue = spRecoveryValue; // SPの回復量
    }

    /// <summary>
    /// Return Hp + hpRecovery value.
    /// </summary>
    /// <param name="Hp">hit point.</param>
    /// <returns>Add hp and hp recovery value.</returns>
    public int RecoveryHP(int Hp)
    {
        // 足した値を返す
        return Hp + hpRecoveryValue;
    }

    /// <summary>
    /// Recovery hp.
    /// </summary>
    /// <param name="Hp">This hp is refarence of target hp.</param>
    public void RecoveryHP(ref int Hp)
    {
        // HPを回復させる
        Hp += hpRecoveryValue;
    }

    /// <summary>
    /// Recovery hp.
    /// </summary>
    /// <param name="Hp">This is refarence of target hp.</param>
    /// <param name="itemNum">This is refarence of having item num.</param>
    public void RecoveryHP(ref int Hp, ref int itemNum)
    {
        // アイテムがある場合
        if (itemNum > 0)
        {
            // アイテムの個数を引く
            itemNum--;
            // HPを回復する
            Hp += hpRecoveryValue;
        }
    }

    /// <summary>
    /// Return Sp + spRecovery value.
    /// </summary>
    /// <param name="Sp">sp</param>
    /// <returns>Add sp and sp recovery value.</returns>
    public int RecoverySP(int Sp)
    {
        // 足した値を返す
        return Sp + spRecoveryValue;
    }

    /// <summary>
    /// Recovery sp.
    /// </summary>
    /// <param name="Sp">Refarence of target sp.</param>
    public void RecoverySP(ref int Sp)
    {
        // SPを回復させる
        Sp += spRecoveryValue;
    }

    /// <summary>
    /// Recovery sp.
    /// </summary>
    /// <param name="Sp">Refarence of target sp.</param>
    /// <param name="itemNum">Refarence of target item num.</param>
    public void RecoverySP(ref int Sp, ref int itemNum)
    {
        // アイテムがある場合
        if (itemNum > 0)
        {
            // アイテムの個数を引く
            itemNum--;
            // SPを回復させる
            Sp += spRecoveryValue;
        }
    }

    /// <summary>
    /// Recovery hp and sp.
    /// </summary>
    /// <param name="hp">Refarence of target hp.</param>
    /// <param name="sp">Refarence of target sp.</param>
    public void RecoveryHPandSP(ref int hp, ref int sp)
    {
        // HPを回復する
        hp += hpRecoveryValue;
        // SPを回復する
        sp += spRecoveryValue;
    }

    /// <summary>
    /// Recovery hp and sp.
    /// </summary>
    /// <param name="hp">Refarence of target hp.</param>
    /// <param name="sp">Refarence of target sp.</param>
    /// <param name="itemNum">Refarence of target item num.</param>
    public void RecoveryHPandSP(ref int hp, ref int sp, ref int itemNum)
    {
        // アイテムがある場合
        if (itemNum > 0)
        {
            // アイテムの個数を引く
            itemNum--;
            // HPを回復する
            hp += hpRecoveryValue;
            // SPを回復する
            sp += spRecoveryValue;
        }
    }
}

/// <summary>
/// All item data.
/// </summary>
public static class Items
{
    public static System.Collections.Generic.Dictionary<int, ItemBase> items { get { return Items.items; } private set { Items.items = value; } }
    static LoadCSV itemData = new LoadCSV("Items.csv");     // アイテムのデータがある部分

    /// <summary>
    /// Default constractor.
    /// </summary>
    static Items()
    {
        items = new System.Collections.Generic.Dictionary<int, ItemBase>();    // アイテムの配列

        // データテーブルの数だけ繰り返す
        for (int i = 0; i < itemData.dataTable.Count; i++)
        {
            // アイテムを入れ込む
            items.Add(int.Parse((string)itemData.dataTable[i][0]), new ItemBase(
                id: int.Parse((string)itemData.dataTable[i][0]),
                kind: KIND_ITEM.ITEM,
                name: (string)itemData.dataTable[i][1],
                imageName: (string)itemData.dataTable[i][2],
                cost: int.Parse((string)itemData.dataTable[i][3]),
                hpRecoveryValue: int.Parse((string)itemData.dataTable[i][4]),
                spRecoveryValue: int.Parse((string)itemData.dataTable[i][5])
                )
            );
        }
    }
}