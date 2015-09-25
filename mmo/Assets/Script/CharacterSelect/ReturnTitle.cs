using UnityEngine;
using System.Collections;

public class ReturnTitle : MonoBehaviour {
    [SerializeField]
    GameObject window;
    [SerializeField]
    GameObject prefab;
    /// <summary>
    /// Go to title scene and disconnect on photon network.
    /// </summary>
    public void GoTitleAndDisconnectPhotonNetwork()
    {
        PhotonNetwork.Disconnect();
        Application.LoadLevel("SelectServer");
    }
}
