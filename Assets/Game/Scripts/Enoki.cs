#region

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
            Destroy(gameObject , clip.length + 0.2f);
        }

    #endregion
    }
}