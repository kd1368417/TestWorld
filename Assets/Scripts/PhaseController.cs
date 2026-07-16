using UnityEngine;

// アセットプレハブを管理するスクリプト

public class PhaseController : MonoBehaviour
{
	private AutoReleaseObjectParent releaseParent;

	public async Awaitable Setup(PhaseData phaseData)
	{
		releaseParent = gameObject.AddComponent<AutoReleaseObjectParent>();

		string playerAddr = GameManager.Instance.PlayerAddress;
		releaseParent.Initialize(phaseData.phasePrefabAddress, playerAddr, phaseData.enemyAddresses);

		SpawnController.Spawn(gameObject, phaseData);
	}

	public void RegisterDynamicAsset(string address)
	{
		if (releaseParent != null)
		{
			releaseParent.RegisterDynamicAddress(address);
		}
	}
}