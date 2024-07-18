package com.tiger.games;

import android.app.Activity;
import android.app.Application;
import android.app.GameManager;
import android.os.Handler;
import android.util.Log;
import android.widget.CompoundButton;

import com.transsion.game.analytics.GameAnalytics;
import com.transsion.gamead.AdHelper;
import com.transsion.gamead.GameAdBannerListener;
import com.transsion.gamead.GameAdLoadListener;
import com.transsion.gamead.GameAdRewardShowListener;
import com.transsion.gamead.GameAdShowListener;
import com.transsion.gamead.GameRewardItem;
import com.transsion.gamead.impl.TGBannerView;
import com.unity3d.player.UnityPlayer;

public class AhaSDK {
    private static String TAG = "BALLSORT";
    private static boolean isGetRewarded = false;
    private static boolean isAdRewardAvailable = false;
    private static boolean isInterstitialAdLoaded = false;
    private static boolean isInterstitialAdRequesting = false;
    private static boolean isBannerAdRequesting = false;
    private static boolean isBannerAdShown = false;
    private static boolean isRewardedRequest = false;
    private static Handler handler;
    private static TGBannerView mTGBannerView;

    public static void initialize(Application application) {
    }

    public static void showAppOpenAd(Activity activity) {
        AdHelper.showAppOpen(5, new GameAdLoadListener() {
            @Override
            public void onAdLoaded() {
                Log.d(TAG, "Successfully loaded");
            }

            @Override
            public void onAdFailedToLoad(int code, String message) {
                //Open screen advertisements are usually only displayed during the open screen period. Please do not try again in failed callbacks
                Log.d(TAG, "Loading failed with error code="+code+","+message);
            }
        });
    }

    public static void showPrivacyAgreement(Activity activity) {
    }

    public static void showFloatAds(Activity activity) {
        AdHelper.showFloat(activity, 4, 0.0000, 1.6925, new GameAdShowListener() {
            @Override
            public void onShow() {
                Log.i("FloatAd", "Float show");
            }

            @Override
            public void onClose() {
                Log.i("FloatAd", "Float close");
            }

            @Override
            public void onClick() {
                Log.i("FloatAd", "Float onClick");
            }

            @Override
            public void onAdImpression() {
                Log.i("FloatAd", "Float onAdImpression");
            }

            @Override
            public void onShowFailed(int code, String message) {
                Log.i("FloatAd", "Float show fail " + code + " " + message);
            }
        });
    }

    public static void loadReward(Activity activity) {
        Log.v(TAG, "load reward aha");
        if (isRewardedRequest) {
            return;
        }
        isRewardedRequest = true;
        Log.v(TAG, "load reward aha");
        isAdRewardAvailable = false;
        activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                AdHelper.loadReward(activity, new GameAdLoadListener()
                {
                    @Override
                    public void onAdFailedToLoad(int code, String message) {
                        Log.i(TAG, "Reward onRewardedAdFailedToLoad " + code + " " + message);
                        isAdRewardAvailable = false;
                        isRewardedRequest = false;
                    }

                    @Override
                    public void onAdLoaded() {
                        Log.i(TAG, "Reward onRewardedAdLoaded");
                        isAdRewardAvailable = true;
                        isRewardedRequest = false;
                    }
                });
            }
        });

    }

    public static boolean isRewardedAdLoaded() {
        return isAdRewardAvailable && AdHelper.isRewardReady();
    }

    public static void showReward(Activity activity) {
        Log.v(TAG, "show reward aha");
        isGetRewarded = false;
        activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if (!AdHelper.isRewardReady()) {
                    UnityPlayer.UnitySendMessage("AdManager", "OnRewarded", "unavailable");
                    return;
                }
                AdHelper.showReward(activity, new GameAdRewardShowListener(){
                    @Override
                    public void onShow() {
                        Log.i(TAG, "Reward show");
                    }

                    @Override
                    public void onClose() {
                        Log.i(TAG, "Reward close");
                        UnityPlayer.UnitySendMessage("AdManager", "OnRewarded", isGetRewarded ? "rewarded" : "close");
                    }

                    @Override
                    public void onClick() {
                        Log.i(TAG, "Reward onClick");
                    }

                    @Override
                    public void onAdImpression() {
                        Log.i(TAG, "Reward onAdImpression");
                    }

                    @Override
                    public void onShowFailed(int code, String message) {
                        Log.i(TAG, "Reward show fail " + code + " " + message);
                    }

                    @Override
                    public void onUserEarnedReward(GameRewardItem rewardItem) {
                        isGetRewarded = true;

                        Log.i(TAG, "Reward onUserEarnedReward " + rewardItem.getType() + " " + rewardItem.getAmount());
                    }
                });
            }
        });
    }

    public static void loadInterstitial(Activity activity) {
        Log.v(TAG, "load inter aha");

        if (isInterstitialAdRequesting) {
            return;
        }

        isInterstitialAdRequesting = true;
        Log.v(TAG, "load inter aha");
        activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {

                AdHelper.loadInterstitial(activity, new GameAdLoadListener()
                {
                    @Override
                    public void onAdFailedToLoad(int code, String message) {
                        Log.i(TAG, "Interstitial onAdFailedToLoad " + code + " " + message);
                        isInterstitialAdLoaded = false;
                        isInterstitialAdRequesting = false;
                    }
                    @Override
                    public void onAdLoaded() {
                        isInterstitialAdLoaded = true;
                        isInterstitialAdRequesting = false;
                        Log.i(TAG, "Interstitial onAdLoaded");
                    }
                });
            }
        });
    }

    public static void showInterstitial(Activity activity) {
        Log.v(TAG, "show inter aha");

        if (!isInterstitialAdLoaded || !AdHelper.isInterstitialReady()) {
            if (!isInterstitialAdRequesting) {
                loadInterstitial(activity);
            }
            return;
        }
        Log.v(TAG, "show inter aha");
        activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                AdHelper.showInterstitial(activity, new GameAdShowListener() {
                    @Override
                    public void onShow() {
                        Log.i(TAG, "Interstitial show");
                    }

                    @Override
                    public void onClose() {
                        Log.i(TAG, "Interstitial close");
                    }

                    @Override
                    public void onClick() {
                        Log.i(TAG, "Interstitial onClick");
                    }

                    @Override
                    public void onAdImpression() {
                        Log.i(TAG, "Interstitial onAdImpression");
                    }

                    @Override
                    public void onShowFailed(int code, String message) {
                        Log.i(TAG, "Interstitial show fail " + code + " " + message);
                    }
                });
            }
        });
    }

    public static void loadBanner(Activity activity) {
        Log.v(TAG, "load banner aha");

        activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if (isBannerAdShown || isBannerAdRequesting) {
                    return;
                }
                isBannerAdRequesting = true;
                if(mTGBannerView==null){
                    //TGBannerView only needs to be created once. This object can be used to perform various operations, such as displaying and closing ads.
                    mTGBannerView = AdHelper.newInstanceTGBannerView(activity);
                }
                //Set the ad listener.
                mTGBannerView.setListener(new GameAdBannerListener() {
                    @Override
                    public void onAdFailedToLoad(int code, String message) {
                        Log.i(TAG, "Banner ad loading failed. Error code:" + code + "; error message:" + message);
                        isBannerAdRequesting = false;
                    }

                    @Override
                    public void onAdOpened() {
                        Log.i(TAG, "Banner onAdOpened");
                    }

                    @Override
                    public void onAdImpression() {
                        Log.i(TAG, "Banner onAdImpression");
                    }

                    @Override
                    public void onAdLoaded() {
                        Log.i(TAG, "Banner onAdLoaded");
                        isBannerAdRequesting = false;
                        isBannerAdShown = true;
                    }

                    @Override
                    public void onAdClosed() {
                        Log.i(TAG, "Banner onAdClosed");
                        //This callback exists for some ads.
                        isBannerAdShown = false;
                    }
                });
                //Load the ad. (This method will automatically add the ad to the layout by default and display it in full at the bottom.)
                mTGBannerView.load(activity);
            }
        });
    }

    public static void showBottomBanner(Activity activity) {
        loadBanner(activity);
    }

    public static void hideBanner(Activity activity) {
        Log.v(TAG, "hide banner aha");

        //Close the banner ad.
        mTGBannerView.close(activity);
        //If you want to load the ad again after it is closed, you can use the mTGBannerView object and do not need to call the AdHelper.newInstanceTGBannerView API again.
        //mTGBannerView.load(this);

        //If you exit the game or the current page no longer needs to display the banner ad, call the API below to destroy the ad.
        mTGBannerView.destroy(activity);
        mTGBannerView=null;
    }

    public static void tracking(String action, String param1, String param2) {
        if (param1 == null) return;
        GameAnalytics.tracker(action, param1, param2 == null ? "" : param2);
    }
}