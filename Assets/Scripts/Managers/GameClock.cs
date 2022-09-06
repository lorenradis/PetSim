using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameClock  {

/*
what's the behaviour i want from the clock?
to advance at a faster-than-real-time, some number of minutes per second
to allow time-based events to occur on minute, day and hour changes (mostly hour)
to allow scheduling ot events at specific set times.
to be able to reconstruct the exact date and time from one whole number
to be able to store the current time as an integer
and to be able to spit out a couple of different formats of the time as a string, for use on clocks and whatnot.


if this is a non-monobehaviour class it cannot exist on its own, only inside the game manager, will not have access to update
game manager will call clock.tick when..... when we're in a state that allows time to move.
*/

    private int ticks = 0;
    public int Ticks{get{return ticks;} set{}}
    private int minute;
    public int Minute{get{return minute;} set{}}
    private int hour;
    public int Hour{get{return hour;} set{}}
    private int day;
    public int Day{get{return day;}set{}}
    
    /*not used for now
    private int date;
    public int Date {get{} set{}}
    private int season;
    public int Season{get{} set{}}
    private int year;
    public int Year{get{}set{}}
    */

    public float interval = .7f;
    public float elapsedTime = 0f;

    public delegate void OnMinuteChanged();
    public static OnMinuteChanged onMinuteChangedCallback;
    public delegate void OnHourChanged();
    public static OnHourChanged onHourChangedCallback;
    public delegate void OnDayChanged();
    public static OnDayChanged onDayChangedCallback;
    public delegate void OnSeasonChanged();
    public static OnSeasonChanged onSeasonChangedCallback;

    public GameClock()
    {

    }

    public void SetTime(int newTicks)
    {
        ticks = newTicks;
        minute = ticks % 60;
        hour = (ticks / 60) % 24;
        day = 1 + (ticks / 60 / 24);
    }

    public void Tick()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= interval)
        {
            ticks++;
            elapsedTime -= interval;
            AdvanceMinute();
        }
    }

    private void AdvanceMinute()
    {
        minute++;
        if(minute >= 60)
        {
            minute -= 60;
            AdvanceHour();
        }
        if(onMinuteChangedCallback != null)
        {
            onMinuteChangedCallback.Invoke();
        }
    }

    private void AdvanceHour()
    {
        hour++;
        if(hour >= 24)
        {
            hour -= 24;
            AdvanceDay();
        }
        if(onHourChangedCallback != null)
        {
            onHourChangedCallback.Invoke();
        }
    }

    private void AdvanceDay()
    {
        day++;
        if(onDayChangedCallback != null)
        {
            onDayChangedCallback.Invoke();
        }
    }

    public string StringTime{get{
        string message = "Day " + day + ", ";
        if(hour < 10)
        {
            message += "0"+ hour;
        }else{
            message += hour;
        }
        message += ":";
        if(minute < 10)
        {
            message += "0" + minute;
        }else{
            message += minute;
        }

        if(hour > 11)
        {
            message += " PM";
        }else{
            message += " AM";
        }
        return message;
    }set{

    }}
}