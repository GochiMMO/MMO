using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DrawDamage : MonoBehaviour {
    Camera targetCamera = null;
    Transform cameraTransform;
    //Transform _transform;

    Mesh mesh;      //メッシュフィルターをゲットして入れる先
    Vector3[] ver;  //頂点
    int[] tri;      //面
    Vector2[] uv;   //UV情報

    float startTime = 0;        //開始時間を記録
    float displayTime = 3.0f;   //表示時間、Destroyするまでの時間

    void Awake()
    {
        mesh = gameObject.GetComponent<MeshFilter>().mesh;
    }

    void Start()
    {
        if(targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        cameraTransform = targetCamera.transform;
        //_transform = transform;
        startTime = Time.time;
    }

    void Update()
    {
        //カメラと同じ向きに　※サンプル、カメラの仕様による
        transform.rotation = cameraTransform.rotation;
        transform.Translate(Vector3.up * 0.02f);     //徐々に上昇
        if (startTime + displayTime < Time.time)
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// 表示する数値の設定 Instantiate後実行
    /// </summary>
    /// <param name="value">表示する値</param>
    public void SetValue(int value)
    {
        CreateMesh(value);

        mesh.vertices = ver;
        mesh.triangles = tri;
        mesh.uv = uv;
    }

    /// <summary>
    /// メッシュの作成
    /// </summary>
    /// <param name="value">表示する数値</param>
    void CreateMesh(int value)
    {
        float numberWidth = 0.6f;  //これが1文字の幅
        float numberHeight = 0.5f; //これの2倍が1文字の高さ
        float margin = 0.0f;      //余白　隣り合う数同士の距離を開けたければ正、詰めたければ負
        float uvW = 4f;     //テクスチャの横の分割数
        float uvH = 4f;     //テクスチャの縦の分割数

        string vStr = value.ToString();     //文字列に変えて
        int size = vStr.Length;             //桁数を出す

        ver = new Vector3[size * 4];
        tri = new int[size * 6];
        uv = new Vector2[size * 4];

        float start = numberWidth / 2f * -size; //頂点の開始位置
        float shift = 0f;

        for (int i = 0; i < size; i++)
        {
            //頂点の座標情報
            shift = margin*(i-(size-1)/2f);
            ver[i * 4] = new Vector3(start + numberWidth * i + shift , numberHeight, 0);
            ver[i * 4 + 1] = new Vector3(start + numberWidth * i + shift, -numberHeight, 0);
            ver[i * 4 + 2] = new Vector3(start + numberWidth * (i + 1) + shift, numberHeight, 0);
            ver[i * 4 + 3] = new Vector3(start + numberWidth * (i + 1) + shift, -numberHeight, 0);

            //頂点が構成する三角面の情報
            tri[i * 6] = i * 4 + 1;
            tri[i * 6 + 1] = i * 4;
            tri[i * 6 + 2] = i * 4 + 2;
            tri[i * 6 + 3] = i * 4 + 1;
            tri[i * 6 + 4] = i * 4 + 2;
            tri[i * 6 + 5] = i * 4 + 3;

            //UV情報
            int t = int.Parse(vStr.Substring(i, 1));
            uv[i * 4] = new Vector2(0 + 1f / uvW * (t % uvW), 1f - 1f / uvH * Mathf.Floor(t / uvH));
            uv[i * 4 + 1] = new Vector2(0 + 1f / uvW * (t % uvW), 1f - 1f / uvH * (Mathf.Floor(t / uvH) + 1));
            uv[i * 4 + 2] = new Vector2(0 + 1f / uvW * ((t % uvW) + 1), 1f - 1f / uvH * Mathf.Floor(t / uvH));
            uv[i * 4 + 3] = new Vector2(0 + 1f / uvW * ((t % uvW) + 1), 1f - 1f / uvH * (Mathf.Floor(t / uvH) + 1));
        }
    }
}