using UnityEngine;

namespace Game.Scripts
{
    public class MushroomController : MonoBehaviour
    {
    #region Private Variables

        private int horizontalAxis;

        private Vector2 direction;

        [SerializeField]
        [Min(0)]
        private float moveSpeed;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

    #endregion

    #region Unity events

        private void Update()
        {
            Move();
        }

    #endregion

    #region Private Methods

        private int GetHorizontalAxis()
        {
            if (horizontalAxis != 0) return horizontalAxis;
            return (int)Input.GetAxisRaw("Horizontal");
        }

        private void Move()
        {
            Turn(GetHorizontalAxis());
            transform.position += (Vector3)direction * Time.deltaTime * moveSpeed;
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