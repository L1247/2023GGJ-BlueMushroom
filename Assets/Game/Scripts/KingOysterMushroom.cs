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

        [SerializeField]
        private Sprite normalState;

        [SerializeField]
        private Sprite skillState;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

    #endregion

    #region Unity events

        private void Start()
        {
            mushroomController = FindObjectOfType<MushroomController>();
            SetNextAttackFrequency();
            spriteRenderer.sprite = normalState;
        }

        private void Update()
        {
            FacingMushroom();
            if (DoAttack()) Attack();
        }

    #endregion

    #region Private Methods

        private void Attack()
        {
            lastAttackedTime = Time.time;
            SetNextAttackFrequency();
            SpawnSkill();
        }

        private bool DoAttack()
        {
            var doAttack = Time.time >= lastAttackedTime + attackFrequency;
            return doAttack;
        }

        private void FacingMushroom()
        {
            var mushroomX = mushroomController.GetPos().x;
            var mineX     = transform.position.x;
            spriteRenderer.flipX = mineX <= mushroomX;
        }

        private void ResetSprite()
        {
            spriteRenderer.sprite = normalState;
        }

        private void SetNextAttackFrequency()
        {
            attackFrequency = Random.Range(attackFrequencyMin , attackFrequencyMax);
        }

        private void SpawnSkill()
        {
            spriteRenderer.sprite = skillState;
            var mushroomPosition = mushroomController.GetPos();
            var skillPrefab      = Random.Range(0 , 2) == 0 ? enoki : lingzi;
            var lingziPosition   = lingzi.transform.position;
            lingziPosition.x = Random.Range(0 , 2) == 0 ? 9 : -9;
            var enokiPosition = mushroomPosition;
            enokiPosition.y -= 1.2f;
            var skillPosition = skillPrefab == lingzi ? lingziPosition : enokiPosition;
            Instantiate(skillPrefab , skillPosition , Quaternion.identity);
            Invoke(nameof(ResetSprite) , 0.5f);
        }

    #endregion
    }
}