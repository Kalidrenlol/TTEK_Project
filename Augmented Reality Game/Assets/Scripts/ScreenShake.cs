using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {


    public Transform camTransform;
    public GameObject player;
    public Vector3 offset;

    public float timeDecay = 0.0f;
    public float magnitude = 1.0f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;


    void Start()
    {
        
        offset = transform.position - transform.parent.position;//player.transform.position;


    }

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    

	// Update is called once per frame
	void LateUpdate () {

        if (timeDecay > 0.1f)
        {
            //camTransform.localPosition = originalPos + offset + Random.insideUnitSphere * magnitude;
            transform.position = transform.parent.position + offset + Random.insideUnitSphere * magnitude;
            
            timeDecay -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            timeDecay = 0f;
            //camTransform.localPosition = originalPos + offset;
            transform.position = transform.parent.position + offset;
        
        }
	}

    public void InitScreenShake(float _magnitudeFloat, float _timeFloat)
    {
        //originalPos = camTransform.localPosition;
        timeDecay = _timeFloat;
        magnitude = _magnitudeFloat;
    }
}
