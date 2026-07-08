using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetManager : Singleton<AssetManager>
{
	private Dictionary<string, Object> cache = new();
	private Dictionary<string, AsyncOperationHandle> handles = new();
	private Dictionary<string, int> instanceCounts = new();

	public int LoadedCount => cache.Count;

	protected override void Release()
	{
		Debug.Log($"AssetManager Destroy : {GetInstanceID()}");
	}

	public async Awaitable<T> Load<T>(string address) where T : Object
	{
		// 1. すでにキャッシュにあればカウントを増やして返す
		if (cache.TryGetValue(address, out Object cached))
		{
			instanceCounts[address] = instanceCounts.GetValueOrDefault(address, 0) + 1;
			return cached as T;
		}

		// 2. 新規ロード
		AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
		await handle.Task;

		if (handle.Status != AsyncOperationStatus.Succeeded)
		{
			Debug.LogError($"Load Failed : {address}");
			return null;
		}

		// 3. キャッシュとハンドルを登録し、カウントを +1 する
		cache[address] = handle.Result;
		handles[address] = handle;
		instanceCounts[address] = instanceCounts.GetValueOrDefault(address, 0) + 1;

		return handle.Result;
	}

	public void UnregisterInstance(string address)
	{
		if (string.IsNullOrEmpty(address) || !instanceCounts.ContainsKey(address))
			return;

		instanceCounts[address]--;

		// カウントが0以下になったら完全にメモリから解放
		if (instanceCounts[address] <= 0)
		{
			instanceCounts.Remove(address);
			ReleaseAsset(address);
			Debug.Log($"Auto Unload : {address}");
		}
	}

	public T Get<T>(string address) where T : Object
	{
		if (cache.TryGetValue(address, out Object asset))
			return asset as T;

		Debug.LogWarning($"Asset Not Loaded : {address}");
		return null;
	}

	private void ReleaseAsset(string address)
	{
		if (!handles.TryGetValue(address, out AsyncOperationHandle handle))
			return;

		Addressables.Release(handle);
		handles.Remove(address);
		cache.Remove(address);
	}
}