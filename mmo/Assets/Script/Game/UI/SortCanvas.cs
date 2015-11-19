using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class SortCanvas : MonoBehaviour, IPointerDownHandler, IComparer<SortCanvas> {
    /// <summary>
    /// ソート対象のキャンバスのスクリプトのリスト
    /// </summary>
    static List<SortCanvas> sortCanvass = new List<SortCanvas>();
    /// <summary>
    /// ソート対象のキャンバス
    /// </summary>
    [SerializeField, Tooltip("ソートするキャンバス")]
    public Canvas canvas;
    
    /// <summary>
    /// 生成された時に自動で呼ばれる関数
    /// </summary>
    void Awake()
    {
        sortCanvass.Add(this);
        this.canvas.sortingOrder = FindHighestSortCanvas() + 1;
    }

    public override int GetHashCode()
    {
        return this.canvas.sortingOrder;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    /// <returns></returns>
    public int Compare(SortCanvas obj1, SortCanvas obj2)
    {
        if (obj1 == null)
        {
            return 1;
        }
        else if (obj2 == null)
        {
            return -1;
        }
        else
        {
            return obj1.canvas.sortingOrder < obj2.canvas.sortingOrder ? 1 : -1;
        }
    }

    /// <summary>
    /// 等価かどうか調べる関数
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        return this == (SortCanvas)obj;
    }

    /// <summary>
    /// 削除されたときに自動的に呼ばれる関数
    /// </summary>
    void OnDestroy()
    {
        // 自身を削除する
        sortCanvass.Remove(this);
    }

    /// <summary>
    /// ボタンが押された瞬間
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerDown(PointerEventData data)
    {
        // 最大のソート番号を取得する
        int highestSortNumber = FindHighestSortCanvas();
        // ソートを変更する
        this.canvas.sortingOrder = highestSortNumber + 1;
        // ソート番号が30000を超えていたら
        if (highestSortNumber > 30000)
        {
            // ソート番号を調整する
            AdjustSorting();
        }
    }

    /// <summary>
    /// ソート番号を調整する
    /// </summary>
    public void AdjustSorting()
    {
        // ソートする
        sortCanvass.Sort();
        // 設定するソート番号
        int count = 0;
        // ソートした中身のソート番号を変更する
        foreach (SortCanvas sortCanvas in sortCanvass)
        {
            // ソートされた中身のソート番号を変更する
            sortCanvas.canvas.sortingOrder = count++;
        }
    }

    /// <summary>
    /// 登録されたUIから最大のソート番号を取得する
    /// </summary>
    /// <returns></returns>
    static int FindHighestSortCanvas()
    {
        // 最大のソート番号を格納する変数
        int highestSortValue = -99999;
        // ソート対象のキャンバスだけ繰り返す
        foreach (SortCanvas sortCanvas in sortCanvass)
        {
            // 変数として登録した最大のソート番号を超えていたら
            if (highestSortValue < sortCanvas.canvas.sortingOrder)
            {
                // ソート番号を変更する
                highestSortValue = sortCanvas.canvas.sortingOrder;
            }
        }
        // 最大のソート番号を返却する
        return highestSortValue;
    }
}
