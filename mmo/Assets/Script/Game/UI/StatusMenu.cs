using UnityEngine;
using System.Collections;

public class StatusMenu : MonoBehaviour {
    [SerializeField, Tooltip("ステータス分配のウインドウ")]
    GameObject statusWindow;
    [SerializeField, Tooltip("スキル振りのウインドウ")]
    GameObject skillWindow;

    public void InstanceStatusWindow()
    {
        GameObject.Instantiate(statusWindow);
    }

    public void InstanceSkillWindow()
    {
        GameObject.Instantiate(skillWindow);
    }

    // Use this for initialization
    void Start () {
    
    }
    
    // Update is called once per frame
    void Update () {
    
    }
}
