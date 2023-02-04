#region

using UnityEngine;

#endregion

namespace Game.Scripts
{
    public class TeleportEffect : MonoBehaviour
    {
    #region Private Variables

        [SerializeField]
        private AnimationClip clip;

    #endregion

    #region Unity events

        private void Start()
        {
            var clipLength = clip.length + 0.1f;
            Destroy(gameObject , clipLength);
        }

    #endregion
    }
}