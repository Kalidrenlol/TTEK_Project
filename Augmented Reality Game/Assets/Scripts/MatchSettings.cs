using UnityEngine;

[System.Serializable]
public class MatchSettings {
	// Match
	[Header("Match")]
	public float respawnTime = 3f;
	public float spawnNextWeapon = 5f;
	public int killToWin = 1;
	public float timeForPushToKill = 10f;

	// Mana
	[Header("Mana")]
	public float maxMana = 10f;
	public float getManaPrSecond = 1f;
	public float restoreManaPrSecond = 1f;

}