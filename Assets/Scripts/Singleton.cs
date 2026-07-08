using UnityEngine;

public abstract class ManagerBase : MonoBehaviour
{
	public bool IsInitialized { get; protected set; } = false;
}

public class Singleton<T> : ManagerBase where T : MonoBehaviour
{
	public static T Instance { get; private set; }

	private void Awake()
	{
		if (!Instance)
		{
			Instance = this as T;
			DontDestroyOnLoad(gameObject);
		}
		else
			Destroy(this); // 2個目が来たら自身を即座に消す
	}

	private void Start()
	{
		Initialize();
		Debug.Log($"[ {typeof(T)} ] 初期化完了");
		IsInitialized = true;
	}

	protected virtual void Initialize() {}
	protected virtual void Release() {}

	private void OnDestroy()
	{
		// 自身が正当なインスタンスだった場合のみクリア
		if (Instance == this as T)
		{
			Release();
			IsInitialized = false;
			Instance = null;
		}
	}
}
