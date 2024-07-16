package com.unity3d.player;

import android.app.Application;
import android.util.Log;

import com.transsion.gamead.AdHelper;
import com.transsion.gamead.AdInitializer;
import com.transsion.gamead.GameAdLoadListener;
import com.transsion.gamead.InitListener;
import com.transsion.gamead.constant.InitState;

import java.util.EnumMap;
import java.util.Map;

public class MyApplication extends Application {
    private static String TAG = "PoolBallSort";

    @Override
    public void onCreate() {
        super.onCreate();

        Log.v(TAG, "initialize aha");

        //Initialize GameSDK
        AdInitializer.init(
                new AdInitializer.Builder(this)
                        .setDebuggable(false)
                        .setEnv("release")
                        .setTotalSwitch(true)
        );

        AdInitializer.setInitListener(new InitListener() {
            @Override
            public void onStateChange(int state, String message) {
                UnityPlayer.UnitySendMessage("AdManager", "OnInitializationCompleted", "");
                if (state == InitState.INIT_STATE_COMPLETE) {
                    Log.d(TAG, "Initialization successful. It is recommended to preload interstitial and rewarded ads here.");
                } else if (state == InitState.INIT_STATE_ERROR) {
                    //Most initialization failures are caused by incorrect configuration file locations.
                    Log.d(TAG, "Initialization failed. Cause:" + message);
                }
            }
        });

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
}
