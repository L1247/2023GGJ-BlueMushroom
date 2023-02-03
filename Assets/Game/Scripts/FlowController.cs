#region

using UnityEngine;

#endregion

namespace Game.Scripts
{
    public class FlowController : MonoBehaviour
    {
    #region Private Variables

        [SerializeField]
        private AnimationClip fallClip;

        [SerializeField]
        private AnimationClip scaleClip;

        [SerializeField]
        private AnimationClip maskMushroomScaleClip;

        [SerializeField]
        private Animator maskMushroomAnimator;

        [SerializeField]
        private MushroomController mushroomController;

    #endregion

    #region Unity events

        private void Start()
        {
            Invoke(nameof(StartShowingMushrooms) , fallClip.length + scaleClip.length);
        }

    #endregion

    #region Private Methods

        private void EnableMushroomController()
        {
            mushroomController.EnableController();
        }

        private void StartShowingMushrooms()
        {
            maskMushroomAnimator.Play("ScaleMask");
            Invoke(nameof(EnableMushroomController) , maskMushroomScaleClip.length);
        }

    #endregion
    }
}