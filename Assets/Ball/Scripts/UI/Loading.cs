using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtLoading;

    [SerializeField] private RectTransform rtCanvas;

    [SerializeField] private RectTransform rtImg;

    [SerializeField] private float updatingTime;

    private int _numDot = 1;


    private void Start()
    {
        ResizeImg();
        Invoke(nameof(NextScene), 3);
        InvokeRepeating(nameof(UpdateTextLoading), 0, updatingTime);

#if UNITY_EDITOR
        DataProvider.isPlayLoading = true;
#endif
    }


    private void ResizeImg()
    {
        var scaleX = rtCanvas.sizeDelta.x / rtImg.sizeDelta.x;
        var scaleY = rtCanvas.sizeDelta.y / rtImg.sizeDelta.y;
        var scale = Mathf.Max(Mathf.Max(scaleX, scaleY), 1);
        rtImg.localScale = Vector3.one * scale;
    }


    private void UpdateTextLoading()
    {
        switch (_numDot)
        {
            case 1:
                _numDot++;
                txtLoading.text = "Loading .";
                break;
            case 2:
                _numDot++;
                txtLoading.text = "Loading ..";
                break;
            case 3:
                _numDot = 1;
                txtLoading.text = "Loading ...";
                break;
            default:
                _numDot = 1;
                txtLoading.text = "Loading ...";
                break;
        }
    }


    private void NextScene()
    {
        SceneManager.LoadScene(1);
    }
}