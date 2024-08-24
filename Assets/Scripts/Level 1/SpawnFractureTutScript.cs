using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnFractureTutScript : MonoBehaviour
{
    public GameObject originalObject;
    public GameObject fracturedObject;

    void Update()
    {
       if (Input.GetMouseButton (0))
       {
        Destroy (originalObject);
       }
    }
}
