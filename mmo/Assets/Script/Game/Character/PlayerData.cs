using System;

//プレイヤーのデータ（保存する）
[Serializable]
public struct PlayerData {
    public int Lv;  //レベル
    public int job;    //職業
    public string name; //名前
    public int HP;  //HP
    public int SP;  //SP
    public int attack;      //攻撃力
    public int defense;     //防御力
    public int magicAttack; //魔法攻撃
    public int magicDefence;    //魔法防御

    public int str;
    public int vit;
    public int mnd;
    public int intelligence;

    public int logoutScene;  //ログアウトしたシーン
    //座標
    public float x;
    public float y;
    public float z;
    

}
