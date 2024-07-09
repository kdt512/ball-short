
public class DataProvider : Singleton<DataProvider>
{
    public BallData ballData;
    public BackgroundData backgroundData;
    public BottleData BottleData;

    public int coinShop;
    public int coinAds;
    public int starClaim;

#if UNITY_EDITOR
    public static bool isPlayLoading = false;
#endif
}