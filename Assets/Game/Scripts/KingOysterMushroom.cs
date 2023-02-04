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

        [SerializeField]
        private GameObject enoki;

        [SerializeField]
        private GameObject lingzi;

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
                var mushroomPosition = mushroomController.GetPos();
                var skillPrefab      = Random.Range(0 , 2) == 0 ? enoki : lingzi;
                var lingziPosition   = lingzi.transform.position;
                lingziPosition.x = Random.Range(0 , 2) == 0 ? 9 : -9;
                var enokiPosition = mushroomPosition;
                enokiPosition.y -= 1.2f;
                var skillPosition = skillPrefab == lingzi ? lingziPosition : enokiPosition;
                Instantiate(skillPrefab , skillPosition , Quaternion.identity);
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