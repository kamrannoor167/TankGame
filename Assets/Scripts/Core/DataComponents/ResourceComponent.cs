using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceComponent : MonoBehaviour
{

    [SerializeField]
    private ResourceType resourceType;
    [SerializeField]
    private int count;

    public int Count => count;
    public ResourceType type => resourceType;
  
    
}
