#region

using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Game.Scripts
{
    public class HealthBar : MonoBehaviour
    {
    #region Private Variables

        [SerializeField]
        private Image healthBarImage;

    #endregion

    #region Public Methods

        public void SetFillAmount(float fillAmount)
        {
            healthBarImage.fillAmount = fillAmount;
        }

    #endregion
    }
}