
using UnityEngine;

public class TimerTextChanger : TextChanger
{

    private void OnEnable()
    {
        ResourceHolder.TimeChanged += ReceiveValue;   
    }

    void Start()
    {
        ReceiveValue(null,ResourceHolder.Timer);
    }

    private void OnDisable()
    {
        ResourceHolder.TimeChanged -= ReceiveValue;
    }

}
