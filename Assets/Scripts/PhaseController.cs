using UnityEngine;

public class PhaseController : MonoBehaviour
{
	private AutoReleaseObjectParent releaseParent;

	public async Awaitable Setup(PhaseData phaseData)
	{
		// 自分自身にメモリ解放用コンポーネントをアタッチして初期化
		releaseParent = gameObject.AddComponent<AutoReleaseObjectParent>();

		string playerAddr = GameManager.Instance.PlayerAddress;
		releaseParent.Initialize(phaseData.phasePrefabAddress, playerAddr, phaseData.enemyAddresses);

		// スポーン処理を実行
		await SpawnController.Spawn(gameObject, phaseData);
	}

	public void RegisterDynamicAsset(string address)
	{
		if (releaseParent != null)
			releaseParent.RegisterDynamicAddress(address);
	}
}