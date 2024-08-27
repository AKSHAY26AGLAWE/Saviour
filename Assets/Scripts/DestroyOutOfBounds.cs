using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOutOfBounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < -5)
        {
            Destroy(gameObject);
        }
    }
    //public void PlayGame()
    //{
       // SceneManager.LoadScene("Lvl_1");
   // }
}
