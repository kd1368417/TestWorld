using System.Collections.Generic;
using UnityEngine;

// スポーンポイントを記憶、視覚的に見やすくするスクリプト

public class SpawnPoint : MonoBehaviour
{
    public static List<SpawnPoint> AllSpawnPoints { get; private set; } = new();

    private void Awake()
    {
        AllSpawnPoints.Add(this);
    }

    private void OnDestroy()
    {
        AllSpawnPoints.Remove(this);
    }

    private void OnDrawGizmos()
	{
		Matrix4x4 originalMatrix = Gizmos.matrix;

		Gizmos.matrix = transform.localToWorldMatrix;

		if (CompareTag("Player"))Gizmos.color = Color.green;
		else Gizmos.color = Color.red;

		Gizmos.DrawCube(Vector3.zero, Vector3.one * 0.5f);

		Gizmos.DrawLine(Vector3.zero, Vector3.forward * 1.0f);

		Gizmos.matrix = originalMatrix;
	}
}
