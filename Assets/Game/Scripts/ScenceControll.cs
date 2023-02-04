using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenceControll : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button quitBtn;
    [SerializeField] private Button startBtn;
    void Start()
    {
        if (quitBtn != null) quitBtn.onClick.AddListener(Quit);
    }

    void Quit()
    {
        Application.Quit();
    }
    
    // Update is called once per frame
    public void port(string scenceName)
    {
        SceneManager.LoadSceneAsync(scenceName);
    }
    
}
