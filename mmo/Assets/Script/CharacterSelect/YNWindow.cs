using UnityEngine;
using System.Collections;

public class YNWindow : MonoBehaviour {

    // Use this for initialization
    void Start () {
    
    }

    public void Yes()
    {

    }

    public void No()
    {
        DestroyObject(this.gameObject);
    }

    // Update is called once per frame
    void Update () {
    
    }
}