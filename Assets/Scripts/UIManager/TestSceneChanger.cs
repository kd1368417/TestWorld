using UnityEngine;

// 仮の実際にシーン移行するスクリプト

public class TestSceneChanger : MonoBehaviour
{
	public void ChangeScene(string sceneName)
	{
		Debug.Log($"[Changer] 了解しました。実際に {sceneName} にシーンを移行します。");

		GameManager.Instance.StartGame(sceneName);
	}
}
