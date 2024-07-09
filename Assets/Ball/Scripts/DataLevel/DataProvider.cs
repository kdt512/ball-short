
public class DataProvider : Singleton<DataProvider>
{
    public BallData ballData;
    public BackgroundData backgroundData;
    public BottleData BottleData;

    public int coinGetItemInShop;
    public int coinAdsInShop;
    public int valueStarClaim;

#if UNITY_EDITOR
    public static bool isPlayLoading = false;
#endif
}