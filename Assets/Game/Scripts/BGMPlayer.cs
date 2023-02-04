using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource bgmSource;
    
    void Start()
    {
        
    }

    void BGMSet(AudioClip bgm)
    {
        if (bgm != null)
        {
             bgmSource.clip = bgm;
             bgmSource.loop = true;
             bgmSource.Play();
        }
       
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
