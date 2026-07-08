using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
	// 現在開いている全画面UI（ウィンドウ）（例：タイトル画面、ショップ画面など）
	private GameObject currentUIWindow;

	// 重ねて表示されているポップアップのスタック
	private Stack<GameObject> popupStack = new();

	// 【全画面切り替え】（例：タイトル画面 ➔ ショップ画面など）
	// 開いているすべてのポップアップと古い全画面UIを破棄し、新しい画面に切り替える
	public async Awaitable OpenUI(UIData uiData)
	{
		// 1. 開いているポップアップをすべて閉じる（破棄時にAutoReleaseが走る）
		while (popupStack.Count > 0)
			CloseTopPopup();

		// 2. 新しいUIのPrefabをAssetManagerからロード
		GameObject uiPrefab = await AssetManager.Instance.Load<GameObject>(uiData.uiPrefabAddress);
		if (!uiPrefab)
		{
			Debug.LogError($"プレハブセットされてねーよ");
            return;
        }

		// 3. 古い全画面UIを破棄（破棄時にAutoReleaseが走りアセットが自動解放される）
		if (currentUIWindow != null)
			Destroy(currentUIWindow);

		// 4. 新しいUIを生成
		currentUIWindow = Instantiate(uiPrefab);

		// 5. ルートにあるControllerを初期化
		if (currentUIWindow.TryGetComponent<IUIController>(out var uiController))
			uiController.Setup(uiData);
		else
			Debug.LogError($"{uiPrefab.name} に IUIController が実装されていません。");
	}

	// 【ポップアップ表示】（例：ショップ画面の上に「購入確認ダイアログ」を重ねる）
	// 元の画面は残したまま、新しいUIを生成してスタックに積む
	public async Awaitable OpenPopup(UIData uiData)
	{
		// 1. ポップアップのPrefabをロード
		GameObject popupPrefab = await AssetManager.Instance.Load<GameObject>(uiData.uiPrefabAddress);
		if (!popupPrefab)
			return;

		// 2. 生成してスタックに積む
		GameObject popupInstance = Instantiate(popupPrefab);
		popupStack.Push(popupInstance);

		// 3. Controllerを初期化
		if (popupInstance.TryGetComponent<IUIController>(out var uiController))
			uiController.Setup(uiData);
		else
			Debug.LogError($"{popupPrefab.name} に IUIController が実装されていません。");
	}

	// 【一番上のポップアップを閉じる】
	// 閉じた（Destroyした）瞬間に、そのポップアップのAutoReleaseObjectParentによってメモリが解放される
	public void CloseTopPopup()
	{
		if (popupStack.Count == 0)
			return;

		GameObject topPopup = popupStack.Pop();
		Destroy(topPopup);
	}
}
