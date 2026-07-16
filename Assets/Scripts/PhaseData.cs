using UnityEngine;

[CreateAssetMenu(menuName = "Game/Phase Data")]
public class PhaseData : ScriptableObject
{
    public string phasePrefabAddress;

    public string[] enemyAddresses;
}