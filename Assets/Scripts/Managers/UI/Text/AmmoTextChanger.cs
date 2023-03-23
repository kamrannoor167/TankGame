
using UnityEngine;

public class AmmoTextChanger : TextChanger
{
    private void OnEnable()
    {
        ResourceHolder.AmmoChanged += ReceiveValue;
        Debug.Log("ammo changed even");
    }
    void Start()
    {
        ReceiveValue(null,ResourceHolder.CurrentAmmo);
    }
    private void OnDisable()
    {
        ResourceHolder.AmmoChanged -= ReceiveValue;
    }
}
