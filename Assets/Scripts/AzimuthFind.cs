using UnityEngine;
using UnityEngine.UI;

public class AzimuthFind : MonoBehaviour
{

    public Transform cameraTransform; // The player's camera transform
    public Vector3 northDirection = Vector3.forward; // The reference north direction, can be set to any world direction
    [SerializeField] private AzimuthDataSO azimuthData;    // ScriptableObject to broadcast azimuth
    [SerializeField] public TargetBank targetBank;    
    public float angleDegrees;

    public  Vector3 GetDirectionFromAzimuth(float azimuthMil)
    {
        // Normalize the north vector (just in case)
        northDirection.y = 0;
        northDirection.Normalize();

        // Convert mils to degrees
        float angleDegrees = azimuthMil * (360f / 6400f);

        // Create rotation from angle around Y-axis
        Quaternion rotation = Quaternion.Euler(0, angleDegrees, 0);

        // Rotate the north vector by that angle
        return rotation * northDirection;
    }
    public float CalculateCamaraAzimuth()
    {
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0; // Ignore vertical tilt
        cameraForward.Normalize();

        Vector3 northFlat = northDirection;
        northFlat.y = 0;
        northFlat.Normalize();

        angleDegrees = Vector3.SignedAngle(northFlat, cameraForward, Vector3.up);
        if (angleDegrees < 0) angleDegrees += 360f;

        // Convert from degrees to mils (6400 mils in 360 degrees)
        float azimuthMil = angleDegrees * (6400f / 360f);
        return azimuthMil;
    }
    public float GetAzimuthToTarget(Transform targetTransform)
    {
        Vector3 toTarget = targetTransform.position - cameraTransform.position;
        toTarget.y = 0;
        toTarget.Normalize();

        northDirection.y = 0;
        northDirection.Normalize();

        float angleDegrees = Vector3.SignedAngle(northDirection, toTarget, Vector3.up);
        if (angleDegrees < 0) angleDegrees += 360f;

        float azimuthMil = angleDegrees * (6400f / 360f);
        Debug.Log($"Azimuth to enemy: {Mathf.RoundToInt(azimuthMil)} mils");
        return azimuthMil;
    }
    void Start()
    {

    }
    void Update()
    {
        if (azimuthData != null)
        {
            float az = CalculateCamaraAzimuth();
            azimuthData.UpdateAzimuth(az);
        }
    }
}
