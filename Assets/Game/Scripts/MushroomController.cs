#region

using System.Collections.Generic;
using GameJamUtility.Core.AudioManager;
using UnityEngine;
using UnityEngine.UI;

#endregion

namespace Game.Scripts
{
    public class MushroomController : MonoBehaviour
    {
    #region Private Variables

        private const int dashPower = 10;

        private const float groundCheckDistance = 0.2f;
        private const float playSoundFrequency  = 0.3f;

        private int horizontalAxis;

        private Vector2 direction;

        private bool onGround;

        private int currentHealthAmount;

        private bool isDead;

        private bool inDash;

        private float defaultGravityScale;

        private Vector2 defaultColliderOffset;
        private Vector2 defaultColliderSize;

        private float currentMoveSoundTimer;

        private bool invincible;

        private float currentDashAmount;

        private readonly float maxDashAmount = 3f;

        [SerializeField]
        [Min(0)]
        private float moveSpeed;

        [SerializeField]
        private GameObject Reload;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private Rigidbody2D rb;

        [SerializeField]
        private bool movable;

        [SerializeField]
        private int jumpForce = 15;

        [SerializeField]
        private float flashDuration = 0.3f;

        [SerializeField]
        private int healthAmount = 6;

        [SerializeField]
        private HealthBar healthBar;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private List<Transform> groundSensors;

        [SerializeField]
        private LayerMask groundCheckLayerMask;

        [SerializeField]
        private BoxCollider2D boxCollider2D;

        [SerializeField]
        private Image dashImage;

    #endregion

    #region Unity events

        private void Start()
        {
            currentDashAmount     = 3;
            defaultColliderOffset = boxCollider2D.offset;
            defaultColliderSize   = boxCollider2D.size;
            defaultGravityScale   = rb.gravityScale;
            currentHealthAmount   = healthAmount;
            if (movable) EnableController();
            else spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }

        private void Update()
        {
            CountDashEnergy();
            if (movable == false) return;
            HandleDash();
            if (inDash) return;
            CheckOnGround();
            PlayAnimationWithOnGroundState();
            HandleMove();
            HandleJump();
            HandleMoveSound();
        }

    #endregion

    #region Public Methods

        public void EnableController()
        {
            movable = true;
            DisableMask();
        }

        public Vector3 GetPos()
        {
            return transform.position;
        }

        [ContextMenu("TakeDamage")]
        public void TakeDamage()
        {
            if (isDead || inDash || invincible) return;
            spriteRenderer.color = Color.red;
            Invoke(nameof(ResetColor) , flashDuration);
            currentHealthAmount -= 1;
            healthBar.SetFillAmount(currentHealthAmount);
            AudioManager.Instance.PlayAudio("HeroHurt");
            if (currentHealthAmount == 0) Die();
        }

    #endregion

    #region Private Methods

        private bool CanDash()
        {
            var canDash = currentDashAmount >= maxDashAmount;
            return canDash;
        }

        private void CheckOnGround()
        {
            foreach (var groundSensor in groundSensors)
            {
                var groundSensorPosition = groundSensor.position;
                var groundSensorForward = -groundSensor.up;
                var hit = Physics2D.Raycast(groundSensorPosition , groundSensorForward , groundCheckDistance , groundCheckLayerMask);
                onGround = hit.collider is not null;
                Debug.DrawRay(groundSensorPosition , groundSensorForward * groundCheckDistance , onGround ? Color.green : Color.red);
                if (onGround) break;
            }
        }

        private void CountDashEnergy()
        {
            if (CanDash()) return;
            currentDashAmount += Time.deltaTime;
            var barPercent = currentDashAmount / maxDashAmount;
            dashImage.fillAmount = barPercent;
        }

        private void DealDamageForBoss(KingOysterMushroom kingOysterMushroom)
        {
            if (inDash == false) return;
            var bossIsDead = kingOysterMushroom.TakeDamage();
            if (bossIsDead)
            {
                invincible = true;
                movable    = false;
            }

            Invoke(nameof(ResetAnimation) , 2f);
        }

        [ContextMenu("Die")]
        private void Die()
        {
            isDead         = true;
            movable        = false;
            rb.velocity    = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.None;
            var angularVelocity              = onGround ? 400 : 100;
            var angularVelocityWithDirection = IsFacingLeft() ? -angularVelocity : angularVelocity;
            rb.angularVelocity = angularVelocityWithDirection;
            Reload.SetActive(true);
        }

        private void DisableMask()
        {
            spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
        }

        private void DoDash()
        {
            currentDashAmount    = 0;
            boxCollider2D.offset = new Vector2(0.04f , 1f);
            boxCollider2D.size   = new Vector2(3.36f , 2.17f);
            animator.Play("Dash");
            rb.gravityScale = 1f;
            var dashDirection = IsFacingLeft() ? -dashPower : dashPower;
            var dashMovement  = dashDirection * Time.timeScale * moveSpeed;
            rb.velocity = new Vector2(dashMovement , 0);
            AudioManager.Instance.PlayAudio("HeroDash");
            Invoke(nameof(ResetFromDash) , 0.5f);
        }

        private int GetHorizontalAxis()
        {
            if (horizontalAxis != 0) return horizontalAxis;
            return (int)Input.GetAxisRaw("Horizontal");
        }

        private void HandleDash()
        {
            var notInDash   = inDash == false;
            var dashKeyDown = Input.GetKeyDown(KeyCode.G);
            var useDash     = dashKeyDown && notInDash && CanDash();
            if (useDash)
            {
                inDash = true;
                DoDash();
            }
        }

        private void HandleJump()
        {
            var spaceKeyDown = Input.GetKeyDown(KeyCode.Space);
            var canJump      = spaceKeyDown && onGround;
            if (canJump)
            {
                AudioManager.Instance.PlayAudio("HeroJump");
                rb.AddForce(new Vector2(rb.velocity.x , jumpForce) , ForceMode2D.Impulse);
            }
        }

        private void HandleMove()
        {
            var horizontal = GetHorizontalAxis();
            Turn(horizontal);

            rb.velocity = new Vector2(direction.x * moveSpeed * 300 * Time.deltaTime , rb.velocity.y);
        }

        private void HandleMoveSound()
        {
            var isInAir = onGround == false;
            if (isInAir) return;
            var isIdling = Mathf.Abs(rb.velocity.x) <= 0.1f;
            if (isIdling) return;
            var deltaTime = Time.deltaTime;

            if (currentMoveSoundTimer >= playSoundFrequency)
            {
                currentMoveSoundTimer = 0;
                PlayMoveSound();
            }

            currentMoveSoundTimer += deltaTime;
        }

        private bool IsFacingLeft()
        {
            return spriteRenderer.flipX;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.TryGetComponent<KingOysterMushroom>(out var kingOysterMushroom)) DealDamageForBoss(kingOysterMushroom);
        }

        private void PlayAnimationWithOnGroundState()
        {
            var stateName = onGround ? "Idle" : "Fall";
            animator.Play(stateName);
        }

        private void PlayMoveSound()
        {
            AudioManager.Instance.PlayAudio("HeroWalk");
        }

        private void ResetAnimation()
        {
            animator.Play("Idle");
        }

        private void ResetColor()
        {
            spriteRenderer.color = Color.white;
        }

        private void ResetFromDash()
        {
            boxCollider2D.enabled = true;
            boxCollider2D.offset  = defaultColliderOffset;
            boxCollider2D.size    = defaultColliderSize;
            rb.gravityScale       = defaultGravityScale;
            rb.velocity           = new Vector2(0 , rb.velocity.y);
            inDash                = false;
        }

        private void Turn(int horizontalAxis)
        {
            direction = horizontalAxis switch
            {
                1 => Vector2.right , -1 => Vector2.left , 0 => Vector2.zero , _ => direction
            };
            spriteRenderer.flipX = horizontalAxis switch
            {
                1 => false , -1 => true , _ => spriteRenderer.flipX
            };
        }

    #endregion
    }
}