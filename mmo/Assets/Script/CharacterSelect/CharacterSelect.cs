using UnityEngine;
using System.Collections;

public class CharacterSelect : MonoBehaviour {
    [SerializeField]
    GameObject ynWindow;
    // Use this for initialization
    void Start () {
    
    }

    public void EnabledYNWindow()
    {
        Instantiate(ynWindow, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update () {
    
    }
}