using System.Collections.Generic;
using UnityEngine;

public class AutoReleaseObjectParent : MonoBehaviour
{
    private List<string> addresses = new();

    public void Initialize(string stageAddress, string playerAddress, string[] enemyAddresses)
    {
        addresses.Add(stageAddress);
        if (!string.IsNullOrEmpty(playerAddress)) addresses.Add(playerAddress);
        foreach (var addr in enemyAddresses)
        {
            if (!addresses.Contains(addr)) addresses.Add(addr);
        }
    }

    public void RegisterDynamicAddress(string address)
    {
        if (string.IsNullOrEmpty(address)) return;

        if (!addresses.Contains(address))
        {
            addresses.Add(address);
        }
    }

    private void OnDestroy()
    {
        if (AssetManager.Instance == null) return;

        // ステージ全体が消える時に、まとめてカウントを -1 する
        foreach (var address in addresses)
        {
            AssetManager.Instance.UnregisterInstance(address);
        }
    }
}