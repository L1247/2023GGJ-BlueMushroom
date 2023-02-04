namespace Game.Scripts
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class HealthSetting : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] private GameObject healthBar;
        [SerializeField] private List<Sprite> healthImg;
        private int currentHealth;
        private int healthMax;

        void subHealth()
        {
            currentHealth--;

        }

        void Start()
        {
            healthMax = healthBar.transform.childCount * 2;
            if(currentHealth==0){
            currentHealth = healthMax;}
            for (int i = 0; i < healthBar.transform.childCount; i++)
            {
                healthBar.transform.GetChild(i).GetComponent<Image>().sprite = healthImg[2];
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}