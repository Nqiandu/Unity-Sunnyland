using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterHouse : MonoBehaviour
{

    private void Start()
    {
        PlayerController player = new PlayerController();
        print(player);
    }
    // Update is called once per frame
    void Update()
    {
        PlayerController player = new PlayerController();
        
        //Input.anyKey
        if (Input.anyKey) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
