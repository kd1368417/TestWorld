using UnityEngine;
using UnityEngine.SceneManagement;

//namespace UI.Test
//{
    // 命令を受けて、実際にシーンを移行する仕事を執行するスクリプト
    public class TestSceneChanger : MonoBehaviour
    {
        // コントローラーなどから呼び出される、実際のシーン遷移メソッド
        public void ChangeScene(string sceneName)
        {
            Debug.Log($"[Changer] 了解しました。実際に {sceneName} にシーンを移行します。");

            // 実際の仕事（シーン読み込み）を実行
           GameManager.Instance.StartGame(sceneName);
        }
    }
//}