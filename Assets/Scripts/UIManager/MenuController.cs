using UnityEngine;

// 仮の管理スクリプト

public class MenuController : MonoBehaviour, IUIController
{
	TestSceneChanger sceneChanger; 

	public void Setup(UIData uiData)
	{
		Debug.Log("[TestMenu] UIManagerからSetupが呼び出されました。");

		if (sceneChanger == null)
		{
			sceneChanger = gameObject.AddComponent<TestSceneChanger>();
			Debug.Log("[TestMenu] TestSceneChanger を動的に追加して自動紐付けしました！");
		}
	}

	public void ChangeScene(string targetSceneName)
	{
		if (string.IsNullOrEmpty(targetSceneName))
		{
			Debug.LogError("[TestMenu] 移行先のシーン名が空っぽです！ボタンの設定を確認してください。");
			return;
		}

		if (sceneChanger != null)
		{
			Debug.Log($"[TestMenu] ボタンから指定された {targetSceneName} への遷移を SceneChanger に振り分けます。");

			sceneChanger.ChangeScene(targetSceneName);
		}
		else
		{
			Debug.LogError("[TestMenu] 実際に仕事をする TestSceneChanger がセットされていません！");
		}
	}
}
