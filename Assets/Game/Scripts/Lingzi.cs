#region

using UnityEngine;

#endregion

namespace Game.Scripts
{
    public class Lingzi : MonoBehaviour
    {
    #region Private Variables

        private Vector3 mushPosition;

        [SerializeField]
        private LineRenderer lineRenderer;

        [SerializeField]
        private Transform center;

    #endregion

    #region Unity events

        private void Awake()
        {
            var mushroomController = FindObjectOfType<MushroomController>();
            mushPosition = mushroomController.transform.position;
            RefreshLineRendererPositions();
        }

        private void Update()
        {
            RefreshLineRendererPositions();
        }

    #endregion

    #region Private Methods

        private void RefreshLineRendererPositions()
        {
            lineRenderer.SetPosition(0 , center.position);
            lineRenderer.SetPosition(1 , mushPosition);
        }

    #endregion
    }
}