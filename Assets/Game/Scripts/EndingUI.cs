#region

using DG.Tweening;
using UnityEngine;

#endregion

namespace Game.Scripts
{
    public class EndingUI : MonoBehaviour
    {
    #region Private Variables

        [SerializeField]
        private Transform group;

    #endregion

    #region Unity events

        private void Start()
        {
            Invoke(nameof(DoMove) , 2f);
        }

    #endregion

    #region Private Methods

        private void DoMove()
        {
            group.DOLocalMoveY(700 , 1.5f).SetEase(Ease.InOutExpo);
        }

    #endregion
    }
}