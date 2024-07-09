using UnityEngine;

public class ThemeBGUI : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer skin;


    private void Awake()
    {
        EventDispatcher.RegisterListener(EventId.UPDATE_BACKGROUND, OnInit);
    }

    private void OnInit(object obj)
    {
        var itemData = DataProvider.Instance.backgroundData.GetItemDataById(DataManager.ICurrentIdBackground);
        if (itemData != null)
        {
            skin.sprite = itemData.spriteInGame;

            var sizeBg = itemData.spriteInGame.rect.size / itemData.spriteInGame.pixelsPerUnit;
            if (Camera.main != null)
            {
                var mainCam = Camera.main;
                var sizeCam = new Vector2(mainCam.orthographicSize * 2 * mainCam.aspect,
                    mainCam.orthographicSize * 2);
                var scale = sizeCam / sizeBg;
                var maxScale = Mathf.Max(scale.x, scale.y);
                skin.transform.localScale = new Vector3(maxScale, maxScale, 1);
            }
        }
    }

    private void OnDestroy()
    {
        EventDispatcher.RemoveListener(EventId.UPDATE_BACKGROUND, OnInit);
    }
}