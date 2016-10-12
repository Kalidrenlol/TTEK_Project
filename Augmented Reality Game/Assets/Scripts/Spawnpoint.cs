using UnityEngine;
using System.Collections;

public class Spawnpoint : MonoBehaviour {

	[SerializeField] private int number;

	private ParticleSystem particle;

	void Start() {
		particle = GetComponent<ParticleSystem>();
	}

	void StartParticle() {
		particle.Play();

	}

	void StopParticle() {
		particle.Stop();
	}

	public int GetNumber() {
		return number;
	}
}
