[System.Serializable]
public class MatchSettings {
	// Match
	public float respawnTime = 3f;
	public float spawnNextWeapon = 5f;

	public float timeForPushToKill = 10f;

	// Player
	public float pushForce = 5000f;

	// Mana
	public float maxMana = 10f;
	public float getManaPrSecond = 1f;
	public float restoreManaPrSecond = 1f;
}