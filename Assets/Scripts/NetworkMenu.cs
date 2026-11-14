using Unity.Netcode;
using UnityEngine;

public class NetworkMenu : MonoBehaviour
{
    public void StartHost()
    {
        if (NetworkManager.Singleton != null && !NetworkManager.Singleton.IsListening)
            NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        if (NetworkManager.Singleton != null && !NetworkManager.Singleton.IsListening)
            NetworkManager.Singleton.StartClient();
    }
}
