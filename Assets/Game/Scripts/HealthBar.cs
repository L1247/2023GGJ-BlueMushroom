#region

using TMPro;
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

        [SerializeField]
        private TMP_Text healthPercentText;

    #endregion

    #region Public Methods

        public void SetFillAmount(float fillAmount)
        {
            healthBarImage.fillAmount = fillAmount;
            var percent = $"{fillAmount * 100:0}%";
            healthPercentText.text = percent;
        }

    #endregion
    }
}