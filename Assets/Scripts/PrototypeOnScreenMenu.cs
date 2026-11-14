using UnityEngine;

public class PrototypeOnScreenMenu : MonoBehaviour
{
    public NetworkMenu menu;
    private void OnGUI()
    {
        if (menu == null) return;
        if (GUI.Button(new Rect(20, 20, 130, 55), "HOST")) menu.StartHost();
        if (GUI.Button(new Rect(160, 20, 130, 55), "CLIENT")) menu.StartClient();
    }
}
