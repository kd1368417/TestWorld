using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : Singleton<TransitionManager>
{
	private GameObject currentStage;
	private PhaseFlow phaseFlow = new();
	private UI ui = new();

	// 現在のステージのためにプリロードしている敵のリスト（遷移時の解放用）
	private List<string> currentPreloadedEnemies = new();

	protected override void Initialize()
	{
		// 最初から「Game」シーンで開始された場合（エディタデバッグ用）のみ実行する
		if (SceneManager.GetActiveScene().name != "Game")
			return;

		// タイトルから遷移してきた場合は、すでにGameManager側から呼ばれるので
		// ここで二重に実行されないようにチェックを入れる（例: すでにcurrentPhaseがあるならスキップなど）
		// もしくは、エディタで直接Gameシーンを開いてテストする時だけ動かしたい場合はこのままでOKです
		if (GameManager.Instance != null)
		{
			PhaseData data = GameManager.Instance.CurrentPhaseData;
			if (data != null)
				_ = TransitionTo(data);
		}
	}

	public async Awaitable TransitionTo(PhaseData phaseData)
	{
		await phaseFlow.ChangePhase(phaseData);
	}

	public async Awaitable UiUse(UIData uiData)
	{
		await ui.Use(uiData);
	}

	private class PhaseFlow
	{
		private GameObject currentPhase;

		public async Awaitable ChangePhase(PhaseData phaseData)
		{
			// 1. 新しいステージに必要なアセットを先に一括ロード
			// GameManager からプレイヤーのアドレスを引っ張ってくる
			if (GameManager.Instance != null)
			{
				string playerAddr = GameManager.Instance.PlayerAddress;
				if (!string.IsNullOrEmpty(playerAddr))
					await AssetManager.Instance.Load<GameObject>(playerAddr);
			}
			else
				Debug.LogError("GameManager.Instance が見つかりません。");

			// 敵のアセットをロード（ここはそのまま）
			foreach (string address in phaseData.enemyAddresses)
				await AssetManager.Instance.Load<GameObject>(address);

			// 2. 古いステージオブジェクトを破棄
			if (currentPhase != null)
				Destroy(currentPhase);

			// 3. 新しいステージPrefabのロードと生成
			GameObject phasePrefab = await AssetManager.Instance.Load<GameObject>(phaseData.phasePrefabAddress);
			currentPhase = Instantiate(phasePrefab);

			// 4. ステージ自身（Prefab側）に初期化とスポーンの指示を出す
			if (currentPhase.TryGetComponent<PhaseController>(out var phaseController))
				await phaseController.Setup(phaseData);
			else
				Debug.LogError($"{phasePrefab.name} に PhaseController がアタッチされていません。");
		}
	}

	private class UI
	{
        public async Awaitable Use(UIData uiData)
		{
			await UIManager.Instance.OpenPopup(uiData);
            Debug.Log($"UIマネージャーに渡した");
        }
	}
}