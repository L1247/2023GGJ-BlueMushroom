#region

using UnityEngine;

#endregion

namespace Game.Scripts
{
    public class KingOysterMushroom : MonoBehaviour
    {
    #region Private Variables

        private MushroomController mushroomController;

        private double lastAttackedTime;

        private float attackFrequency;

        [SerializeField]
        [Min(0.1f)]
        private float attackFrequencyMin;

        [SerializeField]
        [Min(0.1f)]
        private float attackFrequencyMax;

    #endregion

    #region Unity events

        private void Start()
        {
            mushroomController = FindObjectOfType<MushroomController>();
            SetAttackFrequency();
        }

        private void Update()
        {
            var doAttack = Time.time >= lastAttackedTime + attackFrequency;
            if (doAttack)
            {
                lastAttackedTime = Time.time;
                SetAttackFrequency();
            }
        }

    #endregion

    #region Private Methods

        private void SetAttackFrequency()
        {
            attackFrequency = Random.Range(attackFrequencyMin , attackFrequencyMax);
        }

    #endregion
    }
}