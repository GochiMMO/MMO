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
}
