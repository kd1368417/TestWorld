using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	[SerializeField]
	private PhaseData phaseData;

	[SerializeField]
	private UIData uiData;

	[SerializeField]
	private string playerAddress;

	public string PlayerAddress => playerAddress;
	public PhaseData CurrentPhaseData => phaseData;

    // GameManager.cs の中に追加
    private async void Start()
    {
        await TransitionManager.Instance.UiUse(uiData);
    }

    public async void StartGame(string sceneName)
	{
        // 1. シーンの読み込みを開始、完了まで待つ
        await SceneManager.LoadSceneAsync(sceneName);

        Debug.Log("ステージの生成を開始");

		// シーン遷移したため、ここでステージの生成を開始
		if (TransitionManager.Instance != null && phaseData != null)
			await TransitionManager.Instance.TransitionTo(phaseData);
		else
			Debug.LogError("TransitionManager がないか、PhaseData が設定されていません。");
	}
}