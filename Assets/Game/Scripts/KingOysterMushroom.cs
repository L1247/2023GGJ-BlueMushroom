#region

using DG.Tweening;
using GameJamUtility.Core.AudioManager;
using UnityEngine;

#endregion

namespace Game.Scripts
{
    public class KingOysterMushroom : MonoBehaviour
    {
    #region Private Variables

        private const float teleportFrequencyMin = 3f;
        private const float teleportFrequencyMax = 6f;

        private MushroomController mushroomController;

        private double lastAttackedTime;
        private double lastTeleportTime;

        private float attackFrequency;
        private float teleportFrequency;

        private Transform teleportPointParent;
        private Vector3   teleportPosition;

        private bool effectSpawned;
        private int  lastTeleportIndex;

        private int currentHealth;

        private bool isDead;

        [SerializeField]
        private SpriteRenderer visual;

        [SerializeField]
        private Sprite death;

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

        [SerializeField]
        private GameObject teleportEffect;

        [SerializeField]
        [Min(1)]
        private int healthAmount;

        [SerializeField]
        private HealthBar healthBar;

    #endregion

    #region Unity events

        private void Start()
        {
            currentHealth       = healthAmount;
            teleportPointParent = GameObject.Find("TeleportPoints").transform;
            lastTeleportTime    = Time.time + teleportFrequencyMin;
            mushroomController  = FindObjectOfType<MushroomController>();
            SetNextAttackFrequency();
            spriteRenderer.sprite = normalState;
        }

        private void Update()
        {
            if (isDead) return;
            FacingMushroom();
            if (DoAttack()) Attack();
            if (DoSpawnTeleportEffect()) SpawnTeleportEffect();
            if (DoTeleport()) Teleport();
        }

    #endregion

    #region Public Methods

        [ContextMenu("TakeDamage")]
        public bool TakeDamage()
        {
            if (isDead) return true;
            currentHealth -= 1;
            healthBar.SetFillAmount(currentHealth);
            AudioManager.Instance.PlayAudio("BossHurt");
            if (currentHealth <= 0) Die();
            return currentHealth <= 0;
        }

    #endregion

    #region Private Methods

        private void Attack()
        {
            lastAttackedTime = Time.time;
            SetNextAttackFrequency();
            SpawnSkill();
        }

        [ContextMenu("Die")]
        private void Die()
        {
            isDead                                = true;
            GetComponent<BoxCollider2D>().enabled = false;
            HandleDieFacing();
            visual.sprite = death;
            var animator = visual.GetComponent<Animator>();
            animator.enabled = true;
            SlowTimeEffect();
        }

        private bool DoAttack()
        {
            var doAttack = Time.time >= lastAttackedTime + attackFrequency;
            return doAttack;
        }

        private bool DoSpawnTeleportEffect()
        {
            return Time.time >= lastTeleportTime + teleportFrequency - 1;
        }

        private bool DoTeleport()
        {
            var doTeleport = Time.time >= lastTeleportTime + teleportFrequency;
            return doTeleport;
        }

        private void FacingMushroom()
        {
            var mushroomX = mushroomController.GetPos().x;
            var mineX     = transform.position.x;
            spriteRenderer.flipX = mineX <= mushroomX;
        }

        private Transform GetRandomTeleportPoint()
        {
            var count         = teleportPointParent.childCount;
            var teleportIndex = Random.Range(0 , count);
            if (lastTeleportIndex == teleportIndex)
            {
                if (teleportIndex >= count - 1) teleportIndex =  0;
                else teleportIndex                            += 1;
            }

            var randomTeleportPoint = teleportPointParent.GetChild(teleportIndex);
            lastTeleportIndex = teleportIndex;
            return randomTeleportPoint;
        }

        private void HandleDieFacing()
        {
            if (IsFacingRight())
            {
                var position = transform.Find("Position");
                position.localRotation = Quaternion.Euler(0 , 180 , 0);
                visual.flipX           = !visual.flipX;
            }
        }

        private bool IsFacingRight()
        {
            return spriteRenderer.flipX;
        }

        private void ResetSprite()
        {
            if (isDead) return;

            spriteRenderer.sprite = normalState;
        }

        private void SetNextAttackFrequency()
        {
            attackFrequency = Random.Range(attackFrequencyMin , attackFrequencyMax);
        }

        private static void SlowTimeEffect()
        {
            float timeScale = 0;
            DOTween.To(() => timeScale , x => timeScale = x , 1 , 1.6f)
                   .SetEase(Ease.OutQuad)
                   .OnUpdate(() => Time.timeScale = timeScale)
                   .SetUpdate(UpdateType.Fixed);
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

        private void SpawnTeleportEffect()
        {
            if (effectSpawned) return;
            effectSpawned      =  true;
            teleportPosition   =  GetRandomTeleportPoint().position;
            teleportPosition.y += 2.3f;
            var effectPosition = teleportPosition;
            effectPosition.y -= 0.77f;
            Instantiate(teleportEffect , effectPosition , Quaternion.identity);
        }

        private void Teleport()
        {
            lastTeleportTime   = Time.time;
            teleportFrequency  = Random.Range(teleportFrequencyMin , teleportFrequencyMax);
            transform.position = teleportPosition;
            effectSpawned      = false;
        }

    #endregion
    }
}