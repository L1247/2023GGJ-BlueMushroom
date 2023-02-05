#region

using GameJamUtility.Core.AudioManager;
using UnityEngine;

#endregion

namespace Game.Scripts
{
    public class Lingzi : MonoBehaviour
    {
    #region Private Variables

        private Vector3 mushPosition;

        private Vector3 moveDirection;

        [SerializeField]
        private LineRenderer lineRenderer;

        [SerializeField]
        private Transform center;

        [SerializeField]
        private float moveSpeed = 5;

    #endregion

    #region Unity events

        private void Awake()
        {
            var mushroomController = FindObjectOfType<MushroomController>();
            mushPosition   =  mushroomController.GetPos();
            mushPosition.y += 1;

            RefreshLineRendererPositions();
            AudioManager.Instance.PlayAudio("LinziCharge");
            Destroy(gameObject , 3f);
        }

        private void Update()
        {
            RefreshLineRendererPositions();
            MoveTowardToDestination();
        }

    #endregion

    #region Private Methods

        private void MoveTowardToDestination()
        {
            moveDirection      =  (mushPosition - transform.position).normalized;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        }

        private void RefreshLineRendererPositions()
        {
            lineRenderer.SetPosition(0 , center.position);
            lineRenderer.SetPosition(1 , mushPosition);
        }

    #endregion
    }
}