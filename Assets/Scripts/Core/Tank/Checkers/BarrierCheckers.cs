using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BarrierCheckers : TankCheckers
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ResourceComponent resource))
        {

            if (resource.type == ResourceType.Ammo)
            {

                Debug.Log("Ammo Invoke");
                SendResourceType?.Invoke(resource);
                Destroy(other.gameObject, 0.28f);
            }

            else if (resource.type == ResourceType.Shield)
            {
                Debug.Log("shield Invoke");
                SendResourceType?.Invoke(resource);

                Destroy(other.gameObject, 0.28f);

            }
            else if (resource.type == ResourceType.Turbo)
            {
                SendResourceType?.Invoke(resource);

                Destroy(other.gameObject, 0.28f);
            }
        }

    }
}
