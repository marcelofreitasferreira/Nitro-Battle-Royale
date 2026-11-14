using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    public int maxHealth = 100;
    public NetworkVariable<int> health = new NetworkVariable<int>(100,
        NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public override void OnNetworkSpawn()
    {
        if (IsServer) health.Value = maxHealth;
    }

    public void ApplyDamage(int damage)
    {
        if (!IsServer || damage <= 0 || health.Value <= 0) return;
        health.Value = Mathf.Max(0, health.Value - damage);
    }
}
