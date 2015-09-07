using System;

//プレイヤーのデータ（保存する）
[Serializable]
public struct PlayerData {
    public int Lv;  //レベル
    public int job;    //職業
    public int HP;  //HP
    public int SP;  //SP
    public int logoutScene;  //ログアウトしたシーン
    //座標
    public float x;
    public float y;
    public float z;
    
}
