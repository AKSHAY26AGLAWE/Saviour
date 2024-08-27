using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform bike; // The bike that the camera will follow

    private Vector3 offset; // The initial offset between the camera and the bike

    void Start()
    {
        // Calculate and store the offset at the start
        offset = transform.position - bike.position;
    }

    void LateUpdate()
    {
        // Maintain the camera's position relative to the bike's position, keeping the initial offset
        transform.position = bike.position + offset;
        
        // Optionally, if you don't want the camera to rotate at all, ensure that its rotation remains constant
        transform.rotation = Quaternion.identity;
    }
}
