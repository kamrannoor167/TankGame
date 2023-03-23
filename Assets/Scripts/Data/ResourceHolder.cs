using System;


public static class ResourceHolder
{
    public static event EventHandler<int> ScoreChanged;

    public static event EventHandler<int> AmmoChanged;

    public static event EventHandler<int> TimeChanged;

    private static int time = 0;

    private static int currentAmmo;


    private static int score = 0;


    public static int CurrentAmmo
    {

        get => currentAmmo;
        set
        {

            currentAmmo = value;
            AmmoChanged?.Invoke(null, currentAmmo);
        }

    }
    public static int Score
    {
        get => score;
        set
        {
            score = value;
            ScoreChanged?.Invoke(null, score);
        }
    }

    public static int Timer
    {

        get => time;
        set
        {

            time = value;
            TimeChanged?.Invoke(null, time);
        }
    }


    public static void Reset()
    {
        ResetScore();

    }


    public static void ResetScore()
    {
        Score = 0;
    }

    public static void ResetAmmo()
    {

        currentAmmo = 0;
    }
}
