using UnityEngine;
using System.Collections;

public class Methods : MonoBehaviour{
    /// <summary>
    /// Delete gameobject of attached this script.
    /// </summary>
    public void DestroyObject()
    {
        GameObject.Destroy(this.gameObject);
    }


}
