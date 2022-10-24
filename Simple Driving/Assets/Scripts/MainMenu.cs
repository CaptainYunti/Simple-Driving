using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] TMP_Text energyText;
    [SerializeField] Button playButton;
    [SerializeField] AndroidNotificationHandler androidNotificationHandler;
    [SerializeField] IOSNotificationHandler iosNotificationHandler;
    [SerializeField] int maxEnergy;
    [SerializeField] int energyRechargeDuration;

    int energy;

    const string EnergyKey = "Energy";
    const string EnergyReadyKey = "EnergyReady";

    private void Start()
    {
        OnApplicationFocus(true);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) { return; }

        CancelInvoke();

        float score = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);
        highScoreText.text = $"High Score: {score}";

        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

        if(energy == 0)
        {
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);

            if(energyReadyString == string.Empty) { return; }

            DateTime energyReady = DateTime.Parse(energyReadyString);

            if(DateTime.Now > energyReady)
            {
                energy = maxEnergy;
                PlayerPrefs.SetInt(EnergyKey, energy);
            }
            else
            {
                playButton.interactable = false;
                Invoke(nameof(EnergyRecharged), (energyReady - DateTime.Now).Seconds);
            }
        }

        energyText.text = $"Play ({energy})";
    }

    void EnergyRecharged()
    {
        playButton.interactable = true;
        energy = maxEnergy;
        PlayerPrefs.SetInt(EnergyKey, energy);
        energyText.text = $"Play ({energy})";
    }

    public void Play()
    {
        if (energy <= 0) { return; }

        energy--;
        
        PlayerPrefs.SetInt(EnergyKey, energy);

        if (energy <= 0)
        {
            DateTime minutesFromNow = DateTime.Now.AddMinutes(energyRechargeDuration);
            PlayerPrefs.SetString(EnergyReadyKey, minutesFromNow.ToString());
#if UNITY_ANDROID
            androidNotificationHandler.ScheduleNotification(minutesFromNow);
#elif UNITY_IOS
            iosNotificationHandler.ScheduleNotification(energyRechargeDuration);
#endif
        }

        SceneManager.LoadScene(1);
    }
}
