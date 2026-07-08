using UnityEngine;

[CreateAssetMenu(menuName = "Game/UI Data")]
public class UIData : ScriptableObject
{
	// 生成するUIプレハブのアドレス（例: "UI_HUDScene", "UI_ShopMenu"など）
	public string uiPrefabAddress;

	// そのUIが内部で動的にロードして使用するサブアセット（画像やSEなど）のアドレス一覧（任意）
	public string[] subAssetAddresses;
}
