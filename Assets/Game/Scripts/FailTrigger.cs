using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
        private BoxCollider2D mapBounds;
        [SerializeField]
        private Transform followTransform;
         private float  deathLine;
    void Start()
    {
        deathLine=mapBounds.bounds.min.y;
        
    }

    // Update is called once per frame
    void fallTrigger(){
        if(followTransform.position.y<deathLine){
            
        }
    }
    void Update()
    {
        fallTrigger();
    }
}
