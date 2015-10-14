using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Load csv file class.
/// </summary>
public class LoadCSV{
    public List<ArrayList> dataTable = new List<ArrayList>();
    public string filePass;

    /// <summary>
    /// Default constractor is ban.
    /// </summary>
    private LoadCSV(){}

    /// <summary>
    /// Constractor with load csv file.
    /// </summary>
    /// <param name="fileName">Load file name.</param>
    public LoadCSV(string fileName)
    {
        
        filePass = Directory.GetCurrentDirectory() + "/savedata/" + fileName;
        LoadCSVFile(filePass);
    }

    /// <summary>
    /// Load csv file by file name. Data is stored to this class's variable "dataTable".
    /// </summary>
    /// <param name="fileName"></param>
    public void LoadCSVFile(string fileName)
    {
        StreamReader csvFile = new StreamReader(fileName, System.Text.Encoding.Default);
        ArrayList tempStr = new ArrayList();
        string lineString;

        // 終わりまで読み込む
        while (!csvFile.EndOfStream)
        {
            lineString = csvFile.ReadLine();    // 改行まで読む
            string[] data = lineString.Split(',');

            // コメント行を読み飛ばす
            if (data[0][0] == '/' && data[0][1] == '/')
            {
                continue;
            }
            // データを１個ずつ格納する
            for (int i = 0; i < data.Length; i++)
            {
                tempStr.Add(data[i]);   // データを入れ込む
            }
            // データテーブルに１行追加する
            dataTable.Add(tempStr);
        }
        csvFile.Close();    // ファイルを解放する
    }
}
