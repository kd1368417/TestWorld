using UnityEngine;

// 敵やプレイヤーの生成処理を管理するスクリプト

public static class SpawnController
{
	public static void Spawn(GameObject root, PhaseData phaseData)
	{
		var points = SpawnPoint.AllSpawnPoints.FindAll(p => p.transform.IsChildOf(root.transform));

		if (points == null || points.Count == 0) return;

		string playerAddr = GameManager.Instance.PlayerAddress;

		// プレイヤーの配置処理
		if (!string.IsNullOrEmpty(playerAddr))
		{
			SpawnPoint playerPoint = points.Find(p => p.CompareTag("Player"));
			Vector3 spawnPos = playerPoint != null ? playerPoint.transform.position : Vector3.zero;
			Quaternion spawnRot = playerPoint != null ? playerPoint.transform.rotation : Quaternion.identity;

			GameObject playerPrefab = AssetManager.Instance.Get<GameObject>(playerAddr);
			if (playerPrefab != null)
			{
				Object.Instantiate(playerPrefab, spawnPos, spawnRot, root.transform);
			}
		}

		// 敵の配置処理
		foreach (var point in points)
		{
			if (point.CompareTag("Player")) continue;
			string address = phaseData.enemyAddresses[Random.Range(0, phaseData.enemyAddresses.Length)];

			GameObject prefab = AssetManager.Instance.Get<GameObject>(address);
			if (!prefab) continue;

			Object.Instantiate(prefab, point.transform.position, point.transform.rotation, root.transform);
		}
	}
}