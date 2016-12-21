using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Explosive : NetworkBehaviour {

    [SyncVar]
    public float lifeTime = 3f;
    public float explosiveForce = 5000f;
    public float explosiveRadius = 5f;
    Rigidbody rb;
    public GameObject explosiveParticle;
    bool live = true;

	void Start () {
        rb = this.GetComponent<Rigidbody>();
	}
	
	void Update () {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0 && live == true)
        {
            RpcExplode();
            live = false;
        }
	}

    
    void RpcExplode()
    {
        Debug.Log("explosion" + transform.position);

        Vector3 pos = rb.transform.position;
        Collider[] colliders = Physics.OverlapSphere(pos, explosiveRadius);
        Camera.main.transform.GetComponent<ScreenShake>().InitScreenShake(1f, 0.4f);
        foreach (Collider hit in colliders)
        {
            Rigidbody rbHit = hit.GetComponent<Rigidbody>();
            if (rbHit != null)
            {
                rbHit.AddExplosionForce(explosiveForce, transform.position, explosiveRadius);
            }
        }

        GameObject ep = Instantiate(explosiveParticle, transform.position, Quaternion.identity) as GameObject;
        Destroy(ep, 2f);
        GetComponent<Renderer>().enabled = false;
        Destroy(gameObject,2f);
    }
}
