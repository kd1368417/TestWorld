using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// アセットを管理するスクリプト

public class AssetManager : Singleton<AssetManager>
{
	private Dictionary<string, Object> cache = new();					// キャッシュ用
	private Dictionary<string, AsyncOperationHandle> handles = new();	// ハンドル用
	private Dictionary<string, int> instanceCounts = new();				// カウント用

	protected override void Release()
	{
		Debug.Log($"AssetManager Destroy : {GetInstanceID()}");
	}

	public async Awaitable<T> Load<T>(string address) where T : Object
	{
		// 既にロード済みならキャッシュを返してカウントを増やす
		// ※参照カウントも増えるため慎重に呼ぶ必要がある
		if (cache.TryGetValue(address, out Object cached))
		{
            instanceCounts[address] = instanceCounts.GetValueOrDefault(address, 0) + 1;
            return cached as T;
		}

        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
		await handle.Task;

		if (handle.Status != AsyncOperationStatus.Succeeded)
		{
			Debug.LogError($"Load Failed : {address}");
			return null;
		}

		cache[address] = handle.Result;
		handles[address] = handle;
		instanceCounts[address] = 1;

		return handle.Result;
	}

	// カウントが0になったら削除する関数
	public void UnregisterInstance(string address)
	{
		if (string.IsNullOrEmpty(address) || !instanceCounts.ContainsKey(address))return;

		instanceCounts[address]--;

		if (instanceCounts[address] <= 0)
		{
			instanceCounts.Remove(address);
			ReleaseAsset(address);
			Debug.Log($"Auto Unload : {address}");
		}
	}

	// キャッシュにすでにあるものを取得したい時に使う
	public T Get<T>(string address) where T : Object
	{
		if (cache.TryGetValue(address, out Object asset)) return asset as T;

		Debug.LogWarning($"Asset Not Loaded : {address}");
		return null;
	}

	private void ReleaseAsset(string address)
	{
		if (!handles.TryGetValue(address, out AsyncOperationHandle handle))return;

		Addressables.Release(handle);
		handles.Remove(address);
		cache.Remove(address);
	}
}