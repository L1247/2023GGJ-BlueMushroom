#region

using System.Collections;
using GameJamUtility.Core.AudioManager;
using UnityEngine;

#endregion

namespace Game.Scripts
{
    public class Enoki : MonoBehaviour
    {
    #region Private Variables

        [SerializeField]
        private AnimationClip clip;

    #endregion

    #region Unity events

        private void Start()
        {
            
            AudioManager.Instance.PlayAudio("JinjenAppear");
            
            Destroy(gameObject , clip.length + 0.2f);
        }
    #endregion
        
    }
}