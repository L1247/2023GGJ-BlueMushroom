#region

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Game.Scripts
{
    public class HealthBar : MonoBehaviour
    {
    #region Private Variables

        [SerializeField]
        private List<Image> healthBarImages;

    #endregion

    #region Public Methods

        public void SetFillAmount(int currentHealthAmount)
        {
            if (currentHealthAmount <= 0) return;
            Image healthBarImage = null;
            var   fillAmount     = 0f;
            switch (currentHealthAmount)
            {
                case 5 :
                    healthBarImage = GetHealthBarImage(0);
                    fillAmount     = 0.5f;
                    break;
                case 4 :
                    healthBarImage = GetHealthBarImage(0);
                    fillAmount     = 0;
                    break;
                case 3 :
                    healthBarImage = GetHealthBarImage(1);
                    fillAmount     = 0.5f;
                    break;
                case 2 :
                    healthBarImage = GetHealthBarImage(1);
                    fillAmount     = 0f;
                    break;
                case 1 :
                    healthBarImage = GetHealthBarImage(2);
                    fillAmount     = 0.5f;
                    break;
                case 0 :
                    healthBarImage = GetHealthBarImage(2);
                    fillAmount     = 0f;
                    break;
            }

            healthBarImage.fillAmount = fillAmount;
        }

    #endregion

    #region Private Methods

        private Image GetHealthBarImage(int imageIndex)
        {
            Image healthBarImage;
            healthBarImage = healthBarImages[imageIndex];
            return healthBarImage;
        }

    #endregion
    }
}