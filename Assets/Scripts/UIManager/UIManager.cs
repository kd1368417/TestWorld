using System.Collections.Generic;
using UnityEngine;

// UIを管理するスクリプト

public class UIManager : Singleton<UIManager>
{
	private GameObject currentUIWindow;

	private Stack<GameObject> popupStack = new();

	public async Awaitable OpenUI(UIData uiData)
	{
		while (popupStack.Count > 0)
		{
			CloseTopPopup();
		}

		GameObject uiPrefab = await AssetManager.Instance.Load<GameObject>(uiData.uiPrefabAddress);
		if (!uiPrefab)
		{
			Debug.LogError($"プレハブがセットされていません。");
            return;
        }

		if (currentUIWindow != null)
		{
			Destroy(currentUIWindow);
		}

		currentUIWindow = Instantiate(uiPrefab);

		if (currentUIWindow.TryGetComponent<IUIController>(out var uiController))
		{
			uiController.Setup(uiData);
		}
		else
		{
			Debug.LogError($"{uiPrefab.name} に IUIController が実装されていません。");
		}
	}

	public async Awaitable OpenPopup(UIData uiData)
	{
		GameObject popupPrefab = await AssetManager.Instance.Load<GameObject>(uiData.uiPrefabAddress);
		if (!popupPrefab)return;

		GameObject popupInstance = Instantiate(popupPrefab);
		popupStack.Push(popupInstance);

		if (popupInstance.TryGetComponent<IUIController>(out var uiController))
		{
			uiController.Setup(uiData);
		}
		else
		{
			Debug.LogError($"{popupPrefab.name} に IUIController が実装されていません。");
		}
	}

	public void CloseTopPopup()
	{
		if (popupStack.Count == 0)return;

		GameObject topPopup = popupStack.Pop();
		Destroy(topPopup);
	}
}
