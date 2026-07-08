using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	// シーンビューでのみ実行される描画処理
	private void OnDrawGizmos()
	{
		// 元のマトリックスを保存
		Matrix4x4 originalMatrix = Gizmos.matrix;

		// ギズモのマトリックスをオブジェクトのローカルマトリックスに設定
		Gizmos.matrix = transform.localToWorldMatrix;

		// タグによって色を変える
		if (CompareTag("Player"))
			Gizmos.color = Color.green; // プレイヤーは緑
		else
			Gizmos.color = Color.red; // 敵は赤

		// ローカル座標で立方体（Cube）を描画する
		Gizmos.DrawCube(Vector3.zero, Vector3.one * 0.5f);

		// ローカルの前方向（Z軸方向）に線を引く
		Gizmos.DrawLine(Vector3.zero, Vector3.forward * 1.0f);

		// マトリックスを元に戻す
		Gizmos.matrix = originalMatrix;
	}
}
