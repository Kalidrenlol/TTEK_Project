using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;

public class Player : NetworkBehaviour {

	private bool _isDead = false;
	public bool isDead {
		get {return _isDead;}
		protected set {_isDead = value;}
	}

	[SyncVar] public string username;
	[SyncVar] public string playerID = "Loading...";
	[SyncVar] public int score = 0;
	[SyncVar] private int currentHealth;
	[SyncVar] private float mana;

	[SerializeField] private int maxHealth = 100;
	[SerializeField] private Behaviour[] disableOnDeath;
	[SerializeField] private GameObject spawnParticle;
	[SerializeField] public GameObject gameManager;

	Animator playerAnimator;
	public Color color;
	private int playerIndex;
	private Vector3 spawnpointPos;
	private Quaternion spawnpointRot;
	private bool[] wasEnabled;
	private float tempMana;
	public bool isOnWonderland;

	void Start() {
		spawnpointPos = transform.position;
		spawnpointRot = transform.rotation;

		SetColor();
		playerAnimator = transform.FindDeepChild("Character").GetComponent<Animator> ();

		if (isLocalPlayer) {
			StartParticle();
			SetPlayerIndex();
			username = System.Environment.UserName;
		}
	}

	void Update() {
		if (GetComponent<GameController>().gameStarted) {
			if (tempMana > 0 && !isOnWonderland) {
				tempMana = Mathf.Floor(tempMana);
				SaveMana();
			} else if (tempMana < 0){
				tempMana = 0;
			}




		}
	}

	void SetColor() {
		color = gameManager.GetComponent<GameManager>().GetPlayerColor(playerIndex);
	}

	public void SetPlayerIndex() {
		playerIndex = GameManager.GetPlayers().Count() - 1;
	}

	public void Setup() {
		wasEnabled = new bool[disableOnDeath.Length];
		for(int i = 0; i < wasEnabled.Length; i++) {
			wasEnabled[i] = disableOnDeath[i].enabled;
		}
		SetDefaults();
		Debug.Log("Setup called from "+gameObject.name +": "+ wasEnabled[0]);
	}

	[ClientRpc]
	public void RpcTakeDamage(int _amount) {
		if (isDead) {
			return;
		}

		currentHealth -= _amount;

		if (currentHealth <= 0) {
			Die();
            GetComponent<Rigidbody>().drag = 30;
            Debug.Log("drag is low");
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
        GetComponent<Rigidbody>().drag = 0;
		isOnWonderland = false;

		for(int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath[i].enabled = wasEnabled[i];
		}

		Collider _col =  GetComponent<Collider>();
		if (_col != null) {
			_col.enabled = true;
		}
	}

	public void SetScore(int _score) {
		score += _score;
	}

	void GoToSpawnpoint() {
		transform.position = spawnpointPos;
		transform.rotation = spawnpointRot;
	}

	void StartParticle() {
		ParticleSystem[] _particles;
		_particles = spawnParticle.GetComponentsInChildren<ParticleSystem>();

		foreach (ParticleSystem _particle in _particles) {
			_particle.Play();
		}
	}
	
	private IEnumerator Respawn() {
		yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);

		GoToSpawnpoint();
		StartParticle();
		SetDefaults();
	}
		
	[Client]
	public void HitWater() {
		if (!isLocalPlayer) {
			return;
		}
		CmdHitWater(playerID);
	}

	[Command]
	void CmdHitWater(string _playerID) {
		Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcTakeDamage(100);
	}

	[Client]
	public void PushOpponent(Collision collider) {
		if (!isLocalPlayer) {
			return;
		}

		if (playerAnimator.GetBool ("HasAttacked") == true) {
			Vector3 dir = (transform.position - collider.transform.position).normalized;
			Vector3 _force = -dir * 20000f;
			CmdPushOpponent(collider.gameObject.name, _force);
		}
	}

	[Command]
	void CmdPushOpponent(string _playerID, Vector3 _force) {
		Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcPushOpponent(_force);

	}

	[ClientRpc]
	public void RpcPushOpponent(Vector3 _force) {
		GetComponent<PlayerController>().BePushed(_force);
	}


	/************************
	 *        MANA          *
	 ************************/

	void SetMana(float _mana) {
		mana += _mana;
	}

	[Client]
	public void TempMana(float _mana) {
		CmdTempMana(playerID, _mana);
	}

	[Command]
	public void CmdTempMana(string _playerID, float _mana) {
		Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcTempMana(_mana);
	}

	[ClientRpc]
	public void RpcTempMana(float _mana) {
		float totalTemp = mana + tempMana;
		if (totalTemp < GameManager.instance.matchSettings.maxMana) {			
			tempMana += _mana;
		}
	}

	[Client]
	void SaveMana() {
		CmdSaveMana(playerID, GameManager.instance.matchSettings.restoreManaPrSecond * Time.deltaTime);
	}

	[Command]
	public void CmdSaveMana(string _playerID, float _mana) {
		Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcSaveMana(_mana);
	}

	[ClientRpc]
	public void RpcSaveMana(float _mana) {
		if (mana < GameManager.instance.matchSettings.maxMana && tempMana > 0) {
			SetMana(_mana);
			tempMana -= _mana;
		}
	}

}
