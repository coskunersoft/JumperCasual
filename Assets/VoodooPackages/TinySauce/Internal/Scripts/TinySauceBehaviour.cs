using System;
using com.adjust.sdk;
using UnityEngine;
using Voodoo.Sauce.Internal.Analytics;
using Voodoo.Sauce.Internal.IdfaAuthorization;

namespace Voodoo.Sauce.Internal
{
    internal class TinySauceBehaviour : MonoBehaviour
    {
        private const string TAG = "TinySauce";
        private static TinySauceBehaviour _instance;
        private TinySauceSettings _sauceSettings;
        private bool _advertiserTracking;


        private void Awake()
        {
    
            #if UNITY_IOS

                NativeWrapper.RequestAuthorization((status) =>
                {
                    _advertiserTracking = status == IdfaAuthorizationStatus.Authorized;
                    GetComponent<Adjust>().SetupAdjustAfterAtt();
                });

            #elif UNITY_ANDROID


            #endif
            
            if (transform != transform.root)
                throw new Exception("TinySauce prefab HAS to be at the ROOT level!");

            _sauceSettings = TinySauceSettings.Load();
            if (_sauceSettings == null)
                throw new Exception("Can't find TinySauce sauceSettings file.");
            
            if (_instance != null) {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this);
            
            VoodooLog.Initialize(VoodooLogLevel.WARNING);
            // init TinySauce sdk
            InitAnalytics();
        }
        

        private void OnHideUnity(bool isGameShown)
        {
            if (!isGameShown)
            {
                // Pause the game - we will need to hide
                Time.timeScale = 0;
            }
            else
            {
                // Resume the game - we're getting focus again
                Time.timeScale = 1;
            }
        }

        private void InitAnalytics()
        {
            VoodooLog.Log(TAG, "Initializing Analytics");
            
            AnalyticsManager.Initialize(_sauceSettings, true);
            
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            // Brought forward after soft closing 
            // Brought forward after soft closing 
            if (!pauseStatus) {
                AnalyticsManager.OnApplicationResume();
            }
        }
    }
}