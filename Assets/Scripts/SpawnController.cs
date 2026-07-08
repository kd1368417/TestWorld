using UnityEngine;

public static class SpawnController
{
	public static async Awaitable Spawn(GameObject root, PhaseData phaseData)
	{
		SpawnPoint[] points = root.GetComponentsInChildren<SpawnPoint>();

		// プレイヤーの配置処理
		// GameManager からアドレスを取得
		string playerAddr = GameManager.Instance.PlayerAddress;

		if (!string.IsNullOrEmpty(playerAddr))
		{
			// PlayerタグのついたSpawnPointを探す
			SpawnPoint playerPoint = System.Array.Find(points, p => p.CompareTag("Player"));
			Vector3 spawnPos = playerPoint != null ? playerPoint.transform.position : Vector3.zero;
			Quaternion spawnRot = playerPoint != null ? playerPoint.transform.rotation : Quaternion.identity;

			GameObject playerPrefab = AssetManager.Instance.Get<GameObject>(playerAddr);
			if (playerPrefab != null)
				// 生成してステージの子要素にする
				Object.Instantiate(playerPrefab, spawnPos, spawnRot, root.transform);
		}

		// 敵の配置処理
		foreach (var point in points)
		{
			if (point.CompareTag("Player")) continue;
			string address = phaseData.enemyAddresses[Random.Range(0, phaseData.enemyAddresses.Length)];

			GameObject prefab = AssetManager.Instance.Get<GameObject>(address);
			if (!prefab) continue;

			GameObject obj = Object.Instantiate(prefab, point.transform.position, Quaternion.identity, root.transform);
		}
	}
}