using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class GlobalLightManager
{
    public List<TimeOfDayLight> timeOfDayLights = new List<TimeOfDayLight>();

    public Light2D globalLight;

    public Color currentColor { get { return globalLight.color; } }

    public GlobalLightManager() { }

    public void Setup()
    {
        GameClock.onMinuteChangedCallback += UpdateGlobalLight;
        UpdateGlobalLight();
    }

    private void UpdateGlobalLight()
    {
        float currentTime = GameManager.instance.gameClock.Minute + GameManager.instance.gameClock.Hour * 60;
        float previousTime = currentTime;
        float nextTime = 0;
        Color startColor = Color.white;
        Color endColor = Color.white;

        for (int i = 0; i < timeOfDayLights.Count; i++)
        {
            if (timeOfDayLights[i].hour * 60 < currentTime)
            {
                previousTime = timeOfDayLights[i].hour * 60;
                startColor = timeOfDayLights[i].color;
                int nextIndex = (i + 1) < timeOfDayLights.Count ? i + 1 : 0;
                nextTime = timeOfDayLights[nextIndex].hour * 60;
                endColor = timeOfDayLights[nextIndex].color;
            }
        }

        if(nextTime < previousTime)
        {
            nextTime += 24 * 60;
        }

        //Debug.Log("Current time is " + currentTime + " previous time is " + previousTime + " and the next time should be " + nextTime);

        currentTime -= previousTime;
        nextTime -= previousTime;
        previousTime -= previousTime;

//        Debug.Log("Progress from start of " + currentTime + " to " + nextTime + " is " + currentTime / nextTime);

        Color color = Color.Lerp(startColor, endColor, currentTime / nextTime);

        //Debug.Log("start color was " + startColor + ", end color was " + endColor + " , current color should be " + color);

        globalLight.color = color;
    }
}

[System.Serializable]
    public struct TimeOfDayLight
    {
        public int hour;
        public Color color;
    }