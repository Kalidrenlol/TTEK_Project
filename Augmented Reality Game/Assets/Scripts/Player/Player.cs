using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour {

	private bool _isDead = false;
	public bool isDead {
		get {return _isDead;}
		protected set {_isDead = value;}
	}

	[SerializeField] private int maxHealth = 100;

	[SyncVar] private int currentHealth;
	[SerializeField] private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;

	public void Setup() {
		wasEnabled = new bool[disableOnDeath.Length];
		for(int i = 0; i < wasEnabled.Length; i++) {
			wasEnabled[i] = disableOnDeath[i].enabled;
		}
		SetDefaults();
	}
		
	[ClientRpc]
	public void RpcTakeDamage(int _amount) {
		if (isDead) {
			return;
		}

		currentHealth -= _amount;

		if (currentHealth <= 0) {
			Die();
		}
	}

	private void Die() {
		isDead = true;

		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath[i].enabled = false;
		}

		Collider _col = GetComponent<Collider>();
		if (_col != null) {
			_col.enabled = false;
		}

		StartCoroutine(Respawn());

	}

	public void SetDefaults() {
		isDead = false;

		currentHealth = maxHealth;

		for(int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath[i].enabled = wasEnabled[i];
		}

		Collider _col =  GetComponent<Collider>();
		if (_col != null) {
			_col.enabled = true;
		}
	}

	private IEnumerator Respawn() {
		yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

		SetDefaults();
		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;

		Debug.Log(transform.name + " respawns.");
	}
}
