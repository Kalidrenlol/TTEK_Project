using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Explosive : NetworkBehaviour {

    public float lifeTime = 3f;
    public float explosiveForce = 5000f;
    public float explosiveRadius = 5f;
    Rigidbody rb;
    public GameObject explosiveParticle;
    public NetworkTransform netTrans;


	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            Explode();
        }
	}

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;

        if (stream.isWriting)
        {
            syncPosition = netTrans.transform.position;
            stream.Serialize(ref syncPosition);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            netTrans.transform.position = syncPosition;
        }
    }
		
    void Explode()
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
        Destroy(gameObject);
    }
}
