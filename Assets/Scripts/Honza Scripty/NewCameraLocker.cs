using UnityEngine;
using Cinemachine;

public class NewCameraLocker : MonoBehaviour
{
    public string targetTag = "YourSpecificTag";
    public CinemachineFreeLook freeLook;

    void Start()
    {
        CheckTagAndControlCamera();
    }

    private void Update()
    {
        CheckTagAndControlCamera();
    }

    void CheckTagAndControlCamera()
    {
        // Find all game objects with the specified tag
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(targetTag);

        // Check if at least one object with the specified tag exists
        if (objectsWithTag.Length > 0)
        {
            // At least one object with the specified tag exists, lock the camera
            LockCamera();
        }
        else
        {
            // No objects with the specified tag found, unlock the camera
            UnlockCamera();
        }
    }

    void LockCamera()
    {
        // Disable Y axis control
        freeLook.m_YAxis.m_MaxSpeed = 0;

        // Disable X axis control
        freeLook.m_XAxis.m_MaxSpeed = 0;
    }

    void UnlockCamera()
    {
        // Enable Y axis control
        freeLook.m_YAxis.m_MaxSpeed = 2f;

        // Enable X axis control
        freeLook.m_XAxis.m_MaxSpeed = 300f;
    }
}
