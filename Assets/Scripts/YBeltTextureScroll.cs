using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YBeltTextureScroll : MonoBehaviour, IGantryComponent
{
    private Material materialInstance;
    private float tileSize = 0.3f;

    void Start()
    {
        materialInstance = GetComponent<Renderer>().material;
    }
    public void FollowPrintHead(Vector3 printHeadDelta)
    {
        float uvDelta = -printHeadDelta.z / tileSize;

        Vector2 currentOffset = materialInstance.mainTextureOffset;
        currentOffset.y += uvDelta;
        materialInstance.mainTextureOffset = currentOffset;
    }
}
