#region

using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#endregion

public class ScenceControll : MonoBehaviour
{
#region Private Variables

    // Start is called before the first frame update
    [SerializeField]
    private Button quitBtn;

    [SerializeField]
    private Button startBtn;

    [SerializeField]
    private string bossRoomSceneName = "BossRoom";

    [SerializeField]
    private GameObject front;

    [SerializeField]
    private Image bg;

#endregion

#region Unity events

    void Start()
    {
        if (quitBtn != null) quitBtn.onClick.AddListener(Quit);
        startBtn.onClick.AddListener(EnterBossScene);
    }

#endregion

#region Private Methods

    // Update is called once per frame
    private void EnterBossScene()
    {
        var duration = 1.5f;
        front.SetActive(false);
        bg.transform.DOScale(3 , duration).SetEase(Ease.OutQuad);
        bg.DOFade(0 , duration).SetEase(Ease.OutQuad);
        SceneManager.LoadScene(bossRoomSceneName , LoadSceneMode.Single);
    }

    private void Quit()
    {
        Application.Quit();
    }

#endregion
}