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
        if (!SaveManager.isExistSaveData(inputField.text))      //セーブデータが存在しなければ（指定した名前のキャラが存在しなければ）
        {

            PlayerStates.playerData.name = inputField.text;     //名前を登録
            PlayerStates.SavePlayerData();      //セーブ

            if (PlayerStates.environmentalSaveData.oneSaveDataName == null)
            {
                PlayerStates.environmentalSaveData.oneSaveDataName = PlayerStates.playerData.name;
            }
            else if (PlayerStates.environmentalSaveData.twoSaveDataName == null)
            {
                PlayerStates.environmentalSaveData.twoSaveDataName = PlayerStates.playerData.name;
            }
            else
            {
                PlayerStates.environmentalSaveData.threeSaveDataName = PlayerStates.playerData.name;
            }

            SelectJob.windowVisibleFlag = false;        //職業選択ウィンドウの表示フラグを折る
            PlayerStates.SaveEnvironmentalData();       //環境データファイルを保存する
            Application.LoadLevel("CharacterSelect");   //キャラクターセレクト画面に遷移する
        }
        else
        {
            GameObject.Instantiate(popWindow);      //名前が被ってることを通知するウィンドウを表示する
        }
    }
}
