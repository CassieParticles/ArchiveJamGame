using UnityEngine;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("MainGame");    
        }
    }
}
