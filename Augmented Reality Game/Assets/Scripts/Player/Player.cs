using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;

public class Player : NetworkBehaviour {

	private bool _isDead = false;
	public bool isDead {
		get {return _isDead;}
		protected set {_isDead = value;}
	}
    public GameObject explosivePrefab;
    public GameObject explosiveMinePrefab;
    
	[SyncVar] public  string username;
	[SyncVar] public  string playerID = "Loading...";
	[SyncVar] public  int score = 0;
	[SyncVar] private int currentHealth;
	[SyncVar] public  float mana;

	[SerializeField] private int maxHealth = 100;
	[SerializeField] private Behaviour[] disableOnDeath;
	[SerializeField] private GameObject spawnParticle;
    [SerializeField] public  GameObject smokeParticle;
	[SerializeField] public  GameObject gameManager;
	[SerializeField] private GameObject hitCollider;

    public string currentPU = "None";

	Animator playerAnimator;
	public  Renderer rend;
	public  Color color;
	private int playerIndex;
	private Vector3 spawnpointPos;
	private Quaternion spawnpointRot;
	private bool[] wasEnabled;

	public string pushedByPlayer;
	public bool isAttacking;

	// Mana
	public float tempMana;
	public float savedMana;
	public bool isOnWonderland;


    [Header("Sound")]
    public GameObject asThrowExplosive;
    GameObject throwSound;
    AudioSource throwAudioSource;

    public GameObject asPunch;
    GameObject punchSound;
    AudioSource punchAudioSource;

    public GameObject asSpeed;
    GameObject speedSound;
    AudioSource speedAudioSource;

    public GameObject asInvis;
    GameObject invisSound;
    AudioSource invisAudioSource;

    public GameObject asTick;
    GameObject tickSound;
    AudioSource tickAudioSource;

    Transform sound_holder;

    Button spritePU;

    [Header("Images")]
    public Sprite spr_invis;
    public Sprite spr_speed;
    public Sprite spr_mine;
    public Sprite spr_grenade;
    public Sprite[] sprite_array;

    public GameObject powerUpGraphic;

	void Start() {

        sound_holder = GameObject.FindGameObjectWithTag("SoundHolder").transform;

        //sound
        throwSound = Instantiate(asThrowExplosive) as GameObject;
        throwAudioSource = throwSound.GetComponent<AudioSource>();
        throwSound.transform.parent = sound_holder;

        punchSound = Instantiate(asPunch) as GameObject;
        punchAudioSource = punchSound.GetComponent<AudioSource>();
        punchAudioSource.transform.parent = sound_holder;

        speedSound = Instantiate(asSpeed) as GameObject;
        speedAudioSource = speedSound.GetComponent<AudioSource>();
        speedSound.transform.parent = sound_holder;

        invisSound = Instantiate(asInvis) as GameObject;
        invisAudioSource = invisSound.GetComponent<AudioSource>();
        invisSound.transform.parent = sound_holder;

        tickSound = Instantiate(asTick) as GameObject;
        tickAudioSource = tickSound.GetComponent<AudioSource>();
        tickSound.transform.parent = sound_holder;
        //sound end

		spawnpointPos = transform.position;
		spawnpointRot = transform.rotation;

		SetColor();
		playerAnimator = transform.FindDeepChild("Character").GetComponent<Animator> ();

		if (isLocalPlayer) {
            rend = GetComponent<Renderer>();
			StartParticle();
			SetPlayerIndex();
		}

        
	}

	void Update() {
		if (!isLocalPlayer) {
			return;
		}

		if (GetComponent<GameController>().gameStarted) {
			UpdateMana();
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
		if (isLocalPlayer) {
			GetComponent<PlayerSetup>().playerUIInstance.GetComponent<PlayerUI>().btnPowerUp.image.overrideSprite = sprite_array[4];//
		}
	}
		
	public void TakeDamage(int _amount) {
		if (isDead) {
			return;
		}
		if (!isServer) {
			return;
		}

		currentHealth -= _amount;

		if (currentHealth <= 0) {
			Die();
		}

	}

	[Client]
	public void Die() {
		isDead = true;
		for (int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath[i].enabled = false;
		}

		Collider _col = GetComponent<Collider>();
		if (_col != null) {
			_col.enabled = false;
		}

		GetComponent<Rigidbody>().drag = 5;

		StartCoroutine(Respawn());

		if (!isLocalPlayer) {
			return;
		}

		if (pushedByPlayer != null) {
			CmdSetScore(pushedByPlayer, 1, "Kill");
		} else {
			CmdSetScore(playerID, -1, "Suicide");
		}

	}

	public void SetDefaults() {
		isDead = false;

		currentHealth = maxHealth;
        GetComponent<Rigidbody>().drag = 0;
		isOnWonderland = false;
		isAttacking = false;
		pushedByPlayer = null;

		for(int i = 0; i < disableOnDeath.Length; i++) {
			disableOnDeath[i].enabled = wasEnabled[i];
		}

		Collider _col =  GetComponent<Collider>();
		if (_col != null) {
			_col.enabled = true;
		}
	}

	[Command]
	public void CmdSetScore(string _playerID, int _score, string _reason) {
		Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcSetScore(_score, _reason);
	}

	[ClientRpc]
	public void RpcSetScore(int _score, string _reason) {

		score += _score;
		GetComponent<GameController>().IsGameFinish();

		if (!isLocalPlayer) {
			return;
		}


		int _rand;
		switch(_reason) {
		case "Kill":
			Debug.Log("Killed someone");
			_rand = Random.Range(0,3);
			switch (_rand) {
				case 0:
					GetComponent<PlayerSetup>().playerUIInstance.GetComponent<PlayerUI>().ShowFeedbackText("Nice One");
					break;
				case 1:
					GetComponent<PlayerSetup>().playerUIInstance.GetComponent<PlayerUI>().ShowFeedbackText("Kill");
					break;
				case 2:
					GetComponent<PlayerSetup>().playerUIInstance.GetComponent<PlayerUI>().ShowFeedbackText("Perfection");
					break;
				}
			break;
		case "Suicide":
			Debug.Log("Suicide");
			_rand = Random.Range(0,3);
			switch (_rand) {
				case 0:
					GetComponent<PlayerSetup>().playerUIInstance.GetComponent<PlayerUI>().ShowFeedbackText("Suicide");
					break;
				case 1:
					GetComponent<PlayerSetup>().playerUIInstance.GetComponent<PlayerUI>().ShowFeedbackText("Stupidity");
					break;
				case 2:
					GetComponent<PlayerSetup>().playerUIInstance.GetComponent<PlayerUI>().ShowFeedbackText("Oh No");
					break;
			}
			break;
		default:
			Debug.Log("Årsag til død blev ikke genkendt");
			break;
		}
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

		if (!GetComponent<GameController>().gameEnded) {
			SetDefaults();
			GoToSpawnpoint();
			StartParticle();
			ResetMana();
		}
	}
		
	public void HitWater() {
		CmdHitWater(playerID);
		Debug.Log("HitWater");
	}

	[Command]
	void CmdHitWater(string _playerID) {	
		Player _player = GameManager.GetPlayer(_playerID);
		_player.TakeDamage(100);
	}

	#region PUSHING

    [Command]
    public void CmdPushNew(GameObject _pusher)
    {
        Debug.Log(_pusher);
        float hitForce = 4000f;
        float hitRadius = 3f;
        Vector3 pos = _pusher.transform.position;
        Rigidbody thisRb = _pusher.GetComponent<Rigidbody>();
        Collider[] colliders = Physics.OverlapSphere(pos, hitRadius);
        Camera.main.transform.GetComponent<ScreenShake>().InitScreenShake(0.1f, 0.1f);
        foreach (Collider hit in colliders)
        {
           
            Rigidbody rbHit = hit.GetComponent<Rigidbody>();
            if (rbHit != null && rbHit != thisRb)
            {
                //force, position, radius
                rbHit.AddExplosionForce(hitForce, transform.position, hitRadius);
            }

        }
    }






    public void PushSound()
    {
        punchAudioSource.Play();
    }

	[Client]
	public void PushOpponent(Collider coll) {
        
		if (!isLocalPlayer) {
			return;
		}
		if (isAttacking) {
          /*  Debug.Log("Push Collision isattacking");
			Vector3 dir = (transform.position - coll.transform.position).normalized;
			Vector3 _force = -dir * GameManager.instance.powerUps.punchForce;
            if (Network.isServer)
            {
                RpcPushOpponent(coll.gameObject.name, gameObject.name, _force);
                Debug.Log("Call isServer");
            }
            if (Network.isClient)
            {
                CmdPushOpponent(coll.gameObject.name, gameObject.name, _force);
                Debug.Log("Call isClient");
            }
            else
            {
                RpcPushOpponent(coll.gameObject.name, gameObject.name, _force);
                Debug.Log("Call is!client");
            }*/
		}
	}

	[Client]
	public void PushOpponent(Collision coll) {
        
		if (!isLocalPlayer) {
            return;
            
		}

		if (isAttacking) {
           /* Debug.Log("Push Collider isAttacking");
			Vector3 dir = (transform.position - coll.transform.position).normalized;
			Vector3 _force = -dir * GameManager.instance.powerUps.punchForce;
            if (Network.isServer)
            {
                RpcPushOpponent(coll.gameObject.name, gameObject.name, _force);
                Debug.Log("Call isServer");
            }
            if (Network.isClient)
            {
                CmdPushOpponent(coll.gameObject.name, gameObject.name, _force);
                Debug.Log("Call isClient");
            }
            else
            {
                RpcPushOpponent(coll.gameObject.name, gameObject.name, _force);
                Debug.Log("Call is!client");
            }*/
		}
	}

	[Command]
	void CmdPushOpponent(string _playerPushed, string _pushingPlayer, Vector3 _force) {
		Player _player = GameManager.GetPlayer(_playerPushed);
		_player.PushedOpponent(_pushingPlayer, _force);

	}

    [ClientRpc]
    void RpcPushOpponent(string _playerPushed, string _pushingPlayer, Vector3 _force)
    {
        Player _player = GameManager.GetPlayer(_playerPushed);
        _player.PushedOpponent(_pushingPlayer, _force);

    }
		
	public void PushedOpponent(string _pushingPlayer, Vector3 _force) {
        Debug.Log(_pushingPlayer);
        Debug.Log(_force);
        Debug.Log(this);
		pushedByPlayer = _pushingPlayer;
		StartCoroutine(ResetPushedByPlayer());
		GetComponent<Rigidbody>().AddForce(_force);
	}

	private IEnumerator ResetPushedByPlayer() {
		yield return new WaitForSeconds(GameManager.instance.matchSettings.timeForPushToKill);

		pushedByPlayer = null;
	}

	#endregion

	#region MANA

	void SetMana(float _mana) {
		mana += _mana;
	}

	public void TempMana(float _mana) {
		if (!isLocalPlayer) {
			return;
		}
		float totalTemp = mana + tempMana + savedMana;
		if (totalTemp < GameManager.instance.matchSettings.maxMana) {
			CmdTempMana(playerID, _mana);
		}
	}

	[Command]
	public void CmdTempMana(string _playerID, float _mana) {
		Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcTempMana(_playerID, _mana);
	}

	[ClientRpc]
	public void RpcTempMana(string _playerID, float _mana) {
		float totalTemp = mana + tempMana + savedMana;
		if (playerID == _playerID) {
			if (totalTemp < GameManager.instance.matchSettings.maxMana) {
				tempMana += _mana;
			}
		}
	}

	[Command]
	public void CmdSaveMana(string _playerID, float _mana) {
		Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcSaveMana(_mana);
		Debug.Log("Cmd");
	}

	[ClientRpc]
	public void RpcSaveMana(float _mana) {
		Debug.Log("Rpc");
		savedMana += _mana;
		tempMana = 0;
	}
		
	void UpdateMana() {
		
		// Save mana hvis ikke på wonderland //
		if (tempMana > 0 && !isOnWonderland) {
			tempMana = Mathf.Floor(tempMana);
			CmdSaveMana(playerID, tempMana);
			Debug.Log("Called");
		} else if (tempMana < 0){
			tempMana = 0;
		}

		// Begynd at få saved mana //
		if (!isOnWonderland) {
			if (savedMana > 0) {
				float _setMana = GameManager.instance.matchSettings.restoreManaPrSecond * Time.deltaTime;
				SetMana(_setMana);
				savedMana -= _setMana;
			} else if (savedMana < 0) {
				mana = Mathf.Floor(mana);
				savedMana = 0;
			}
		}
	}
		

	void ResetMana() {
		tempMana = 0;
		savedMana = 0;
		//mana = 0;
	}

	#endregion

	#region POWER UP

    public void RotateGraphics(float _time, int _pu)
    {
        if (isLocalPlayer)
        {
            Debug.Log("Start change");
            StartCoroutine(ChangeGraphics(_time, _pu));
        }
    }

    public IEnumerator ChangeGraphics(float _time, int _pu)
    {
        spritePU = GetComponent<PlayerSetup>().playerUIInstance.GetComponent<PlayerUI>().btnPowerUp;
        
        spritePU.image.overrideSprite = sprite_array[0];
        tickAudioSource.Play();
        yield return new WaitForSeconds(_time);

        spritePU.image.overrideSprite = sprite_array[1]; 
        tickAudioSource.Play();
        yield return new WaitForSeconds(_time);

        spritePU.image.overrideSprite = sprite_array[2];
        tickAudioSource.Play();
        yield return new WaitForSeconds(_time);

        spritePU.image.overrideSprite = sprite_array[3];
        tickAudioSource.Play();
        yield return new WaitForSeconds(_time);

        spritePU.image.overrideSprite = sprite_array[0];
        tickAudioSource.Play();
        yield return new WaitForSeconds(_time);

        spritePU.image.overrideSprite = sprite_array[1];
        tickAudioSource.Play();
        yield return new WaitForSeconds(_time);

        spritePU.image.overrideSprite = sprite_array[2];
        tickAudioSource.Play();
        yield return new WaitForSeconds(_time);

        spritePU.image.overrideSprite = sprite_array[3];
        tickAudioSource.Play();
        yield return new WaitForSeconds(_time);
        tickAudioSource.Play();
        spritePU.image.overrideSprite = sprite_array[_pu];
    }
    
	public void CollectPowerup()
	{

		int puType = Mathf.RoundToInt(Random.Range(0, 4));
        RotateGraphics(0.1f, puType);

        //GetComponent<PlayerSetup>().playerUIInstance.GetComponent<PlayerUI>().btnPush.image.overrideSprite = sprite_array[puType];
		Debug.Log("Powerup collected, type: " + puType);
		//Hvis flere, tjek type, udfra puType
		switch (puType)
		{
		case 0:
			currentPU = "0 - Invisibility";
			break;
		case 1:
			currentPU = "1 - Speed";
			break;
        case 2:
            currentPU = "2 - Explosive";
            break;
        case 3:
            currentPU = "3 - Mine";
            break;
		default:
			currentPU = "0 - Invisibility";
			break;
		}

	}

    public void PU_MakeInvisible()
    {
        invisAudioSource.Play();
        Instantiate(smokeParticle, transform.position, Quaternion.identity);
        Renderer[] rs = transform.GetChild(0).GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
        {
            r.enabled = false;
            Invoke("PU_MakeVisible", 5); //5 is var for time in seconds before invoking
        }
    }

	public void PU_MakeVisible()
	{
		Debug.Log("Make visible");
		Renderer[] rs = transform.GetChild(0).GetComponentsInChildren<Renderer>();
		foreach (Renderer r in rs)
		{
			r.enabled = true;
		}
	}

	[Command]
	void CmdPU_MakeInvisible() {
		
	}

    public void PU_HeightenSpeed()
    {
        GetComponent<PlayerMotor>().speed = 20;
        speedAudioSource.Play();
        Invoke("PU_ResetSpeed", 5);
    }

    public void PU_ResetSpeed()
	{
        GetComponent<PlayerMotor>().speed = 10; // Evt sæt dynamisk, så det kan ændres senere hen
	}

    public void PU_ThrowExplosive()
    {
        /*Transform tp = transform.Find("Graphics");
        throwAudioSource.Play();
        Debug.Log(tp);
        Vector3 vec = new Vector3(0, 1.3f, 0);
        var explosive = Instantiate(explosivePrefab, tp.position+vec, tp.rotation) as GameObject;
        explosive.GetComponent<Rigidbody>().AddRelativeForce(explosive.transform.forward * 1000);
        //explosive.rigidbody.AddForce(transform.forward * 2000);

		NetworkServer.Spawn(explosive);*/
		//CmdSpawnTest(gameObject);
        CmdSpawnGrenade(gameObject);
    }

    public void PU_PlaceMine()
    {
        /*throwAudioSource.Play();
        Vector3 offset = new Vector3(0, 5, 0);
        var explosive = Instantiate(explosiveMinePrefab, transform.position + offset, Quaternion.identity) as GameObject;*/
        CmdSpawnMine(gameObject);
    }

	public void ActivatePowerup()
	{
		if (currentPU != "None")
		{
			Debug.Log("Using powerup: "+currentPU);

			switch (currentPU)
			{
			case "0 - Invisibility":
                PU_MakeInvisible();
				break;
			case "1 - Speed":
                PU_HeightenSpeed();
				break;
            case "2 - Explosive":
                PU_ThrowExplosive();
                break;
            case "3 - Mine":
                PU_PlaceMine();
                break;
			default:
				break;
			}


			currentPU = "None";
            spritePU.image.overrideSprite = sprite_array[4];//

		}
		else
		{
			Debug.Log("No powerup available");
		}
	}

	#endregion

	#region Spawn

    [Command]
    public void CmdSpawnGrenade(GameObject _go)
    {
        Transform tp = _go.transform.Find("Graphics");
        throwAudioSource.Play();
        Vector3 vec = new Vector3(0, 1.3f, 0);

        var explosive = Instantiate(explosivePrefab, tp.position + vec, tp.rotation) as GameObject;
        explosive.GetComponent<Rigidbody>().AddRelativeForce(explosive.transform.forward * 1000);
        NetworkServer.Spawn(explosive);
    }
    
    [Command]
    public void CmdSpawnMine(GameObject _go)
    {
        throwAudioSource.Play();
        Vector3 offset = new Vector3(0, 5, 0);
        var explosive = Instantiate(explosiveMinePrefab, _go.transform.position + offset, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(explosive);
    }

	#endregion

}
