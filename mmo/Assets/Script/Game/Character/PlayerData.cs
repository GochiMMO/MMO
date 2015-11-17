using System;

//プレイヤーのデータ（保存する）
[Serializable]
public class PlayerData {
    public int Lv;  //レベル
    public int job;    //職業
    public int characterNumber; //キャラクターの番号
    public string name; //名前
    public int HP;      // HP
    public int MaxHP;   // MAXHP
    public int SP;      // SP
    public int MaxSP;   // MAXSP
    public int attack;      //攻撃力
    public int defense;     //防御力
    public int magicAttack; //魔法攻撃
    public int magicDefence;    //魔法防御

    public int str;
    public int vit;
    public int mnd;
    public int intelligence;

    public int skillPoint;
    public int statusPoint;

    public int logoutScene;  //ログアウトしたシーン
    //座標
    public float x;
    public float y;
    public float z;

    // スキルレベル
    public int[] skillLevel = new int[20];

    public PlayerData() { }

    /// <summary>
    /// 自分と同じ内容のオブジェクトを作成し、返却する関数
    /// </summary>
    /// <param name="obj"></param>
    public PlayerData Clone()
    {
        PlayerData obj = new PlayerData();
        obj.attack = this.attack;
        obj.characterNumber = this.characterNumber;
        obj.defense = this.defense;
        obj.HP = this.HP;
        obj.intelligence = this.intelligence;
        obj.job = this.job;
        obj.logoutScene = logoutScene;
        obj.Lv = this.Lv;
        obj.magicAttack = this.magicAttack;
        obj.magicDefence = this.magicDefence;
        obj.MaxHP = this.MaxHP;
        obj.MaxSP = this.MaxSP;
        obj.mnd = this.mnd;
        obj.name = (string)this.name.Clone();
        obj.skillLevel = new int[20];
        for (int i = 0; i < 20; i++)
        {
            obj.skillLevel[i] = this.skillLevel[i];
        }
        obj.skillPoint = this.skillPoint;
        obj.SP = this.SP;
        obj.statusPoint = this.statusPoint;
        obj.str = this.str;
        obj.vit = this.vit;
        obj.x = this.x;
        obj.y = this.y;
        obj.z = this.z;
        return obj;
    }
}
