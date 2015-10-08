using UnityEngine;
using System.Collections;
using UnityEngine.UI;  ////ここを追加////

public class SKillTable : MonoBehaviour {

    [SerializeField,Tooltip("スキルレベル")]
    int skillLv;
    [SerializeField, Tooltip("スキル名")]
    char skillName;
    [SerializeField, Tooltip("スキル効果")]
    char skillEffect;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
