using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenceReload : MonoBehaviour
{
    // Start is called before the first frame update
    public void ReloadScence()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex , LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
