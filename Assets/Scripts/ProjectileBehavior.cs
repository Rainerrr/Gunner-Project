using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] public BallisticCurve ballisticCurve;
    [SerializeField] public float speed;
    FireEndpoint fireEndpoint;
    private float sampleTime;
    public void SetBallisticCurve(BallisticCurve curve)
    {
        ballisticCurve = curve;
        fireEndpoint = ballisticCurve.GetComponentInChildren<FireEndpoint>();

    }
    // Start is called before the first frame update
    void Start()
    {
        sampleTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        sampleTime += Time.deltaTime * speed;
        transform.position = ballisticCurve.Evaluate(sampleTime);
        transform.forward = ballisticCurve.Evaluate(sampleTime + 0.001f) - transform.position;
        if (sampleTime >= 1)
        {
            Debug.Log("Kaboom");
            fireEndpoint.UnlockTrajectory();
            Destroy(gameObject);
        }

    }
}
