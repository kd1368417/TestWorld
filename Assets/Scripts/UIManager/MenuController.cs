using UnityEngine;

//namespace UI.Test
//{
    // 本プロジェクトの設計に合わせたテスト用コントローラー
    // プレハブのルートオブジェクトにアタッチして使用します
    public class MenuController : MonoBehaviour, IUIController
    {
        //[Header("References")]
        TestSceneChanger sceneChanger; // 実際にシーン遷移の仕事をするスクリプト

        // UIManagerからの初期化（今回は空欄でOK）
        public void Setup(UIData uiData)
        {
            Debug.Log("[TestMenu] UIManagerからSetupが呼び出されました。");

            // ★もしインスペクターでセットされていなければ、自分自身にその場でアタッチして自動セット
            if (sceneChanger == null)
            {
                sceneChanger = gameObject.AddComponent<TestSceneChanger>();
                Debug.Log("[TestMenu] TestSceneChanger を動的に追加して自動紐付けしました！");
            }
        }

        // ★引数（string targetSceneName）を受け取れるように変更
        // これにより、UnityのOnClick欄でシーン名を直接入力できるようになります
        public void ChangeScene(string targetSceneName)
        {
            // 引数が空っぽ、または文字が入っていない場合のチェック
            if (string.IsNullOrEmpty(targetSceneName))
            {
                Debug.LogError("[TestMenu] 移行先のシーン名が空っぽです！ボタンの設定を確認してください。");
                return;
            }

            if (sceneChanger != null)
            {
                Debug.Log($"[TestMenu] ボタンから指定された {targetSceneName} への遷移を SceneChanger に振り分けます。");

                // 実際の仕事をするスクリプトに仕事を振り分ける
                sceneChanger.ChangeScene(targetSceneName);
            }
            else
            {
                Debug.LogError("[TestMenu] 実際に仕事をする TestSceneChanger がセットされていません！");
            }
        }
    }
//}