using UnityEngine;
using UnityEngine.UI;
//DONE
public class SettingPopup : BasePopup
{
    [SerializeField] private Image imgBtnSound;

    [SerializeField] private Image imgIconSound;

    [SerializeField] private Image imgBtnMusic;

    [SerializeField] private Image imgIconMusic;

    [SerializeField] private Image imgBtnNoAds;

    [SerializeField] private Sprite spriteBtnEnable;

    [SerializeField] private Sprite spriteBtnDisable;

    [SerializeField] private Sprite spriteSoundOn;

    [SerializeField] private Sprite spriteSoundOff;

    [SerializeField] private Sprite spriteMusicOn;

    [SerializeField] private Sprite spriteMusicOff;


    private void Start()
    {
        UISound();
        UIMusic();

#if UNITY_ANDROID
        //btnRestore.SetActive(false);
#else
		//btnRestore.SetActive(true);
#endif
    }


    public override void OnInit()
    {
        base.OnInit();
        UINoAds();
    }

    public override void Close()
    {
        base.Close();
        SoundManager.Instance.Play(SoundType.CLICK);
    }

    private void UISound()
    {
        imgBtnSound.sprite = DataManager.IsSoundOn ? spriteBtnEnable : spriteBtnDisable;
        imgIconSound.sprite = DataManager.IsSoundOn ? spriteSoundOn : spriteSoundOff;
    }


    private void UIMusic()
    {
        imgBtnMusic.sprite = DataManager.IsMusicOn ? spriteBtnEnable : spriteBtnDisable;
        imgIconMusic.sprite = DataManager.IsMusicOn ? spriteMusicOn : spriteMusicOff;
    }


    private void UINoAds()
    {
        imgBtnNoAds.sprite = DataManager.IsNoAds ? spriteBtnDisable : spriteBtnEnable;
        //btnNoAds.interactable = !DataManager.IsNoAds;
    }


    public void OnClickShop()
    {
        SoundManager.Instance.Play(SoundType.CLICK);
        UIManager.Instance.OpenUI<BasePopup>(DialogType.SHOP);
    }


    public void OnClickLevel()
    {
        SoundManager.Instance.Play(SoundType.CLICK);
        UIManager.Instance.OpenUI<BasePopup>(DialogType.LEVEL);
    }


    public void OnClickMusic()
    {
        DataManager.IsMusicOn = !DataManager.IsMusicOn;
        UIMusic();
        SoundManager.Instance.Play(SoundType.CLICK);
        MusicManager.Instance.Play();
    }


    public void OnClickSound()
    {
        DataManager.IsSoundOn = !DataManager.IsSoundOn;
        UISound();
        SoundManager.Instance.Play(SoundType.CLICK);
    }


    public void OnClickNoAds()
    {
        SoundManager.Instance.Play(SoundType.CLICK);
        UIManager.Instance.OpenUI<BasePopup>(DialogType.NO_ADS);
    }


    public void OnClickRestore()
    {
        SoundManager.Instance.Play(SoundType.CLICK);
        UIManager.Instance.CloseUI<SettingPopup>(DialogType.SETTING);
    }
}