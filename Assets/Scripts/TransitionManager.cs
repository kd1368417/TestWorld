using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 仮マネージャー

public class TransitionManager : Singleton<TransitionManager>
{
	private PhaseFlow phaseFlow = new();
	private UI ui = new();

	// アセットプレハブを使うときに呼ぶ
	public async Awaitable TransitionTo(PhaseData phaseData)
	{
		await phaseFlow.ChangePhase(phaseData);
	}

	// UIプレハブを使うときに呼ぶ
	public async Awaitable UiUse(UIData uiData)
	{
		await ui.Use(uiData);
	}

	private class PhaseFlow
	{
		private GameObject currentPhase;

		public async Awaitable ChangePhase(PhaseData phaseData)
		{
			if (GameManager.Instance != null)
			{
				string playerAddr = GameManager.Instance.PlayerAddress;
				if (!string.IsNullOrEmpty(playerAddr))
				{
					await AssetManager.Instance.Load<GameObject>(playerAddr);
				}
			}
			else
			{
				Debug.LogError("GameManager.Instance が見つかりません。");
			}

			foreach (string address in phaseData.enemyAddresses)
			{
				await AssetManager.Instance.Load<GameObject>(address);
			}

			if (currentPhase != null)
			{
				Destroy(currentPhase);
			}

			GameObject phasePrefab = await AssetManager.Instance.Load<GameObject>(phaseData.phasePrefabAddress);
			currentPhase = Instantiate(phasePrefab);

			if (currentPhase.TryGetComponent<PhaseController>(out var phaseController))
			{
				await phaseController.Setup(phaseData);
			}
			else
			{
				Debug.LogError($"{phasePrefab.name} に PhaseController がアタッチされていません。");
			}
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