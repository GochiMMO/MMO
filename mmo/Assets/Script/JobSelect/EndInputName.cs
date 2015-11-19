using UnityEngine;

public class EndInputName : MonoBehaviour{
    [SerializeField, Tooltip("名前を入力するフィールド")]
    UnityEngine.UI.InputField inputField;
    [SerializeField, Tooltip("キャラクター名が重複していた時に出すウィンドウ")]
    GameObject popWindow;

    /// <summary>
    /// End of input name method
    /// </summary>
    public void EndInput()
    {
        // 入力ボックスが空ならば
        if (inputField.text == "")
        {
            // 処理が行われない
            return;
        }
        // 指定した名前のセーブデータが存在しなければ
        if (!SaveManager.isExistSaveData(inputField.text))
        {
            // 名前を登録する
            PlayerStatus.playerData.name = inputField.text;
            // 初期ステータスを設定する
            PlayerStatus.LoadFirstStatus();
            // プレイヤーのデータをセーブする
            PlayerStatus.SavePlayerData();
            // コンフィグファイルに名前を登録する
            PlayerStatus.environmentalSaveData.playerName[PlayerStatus.environmentalSaveData.saveDataNum++] = PlayerStatus.playerData.name;
            // 職業選択ウィンドウの表示フラグを折る
            SelectJob.windowVisibleFlag = false;
            // コンフィグファイルをセーブする
            PlayerStatus.SaveEnvironmentalData();
            // キャラクターセレクト画面に遷移する
            Application.LoadLevel("CharacterSelect");
        }
        else
        {
            // 名前が被ってることを通知するウィンドウを表示する
            GameObject.Instantiate(popWindow);
        }
    }
}
