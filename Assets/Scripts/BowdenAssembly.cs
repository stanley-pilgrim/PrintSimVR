using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowdenAssembly : MonoBehaviour
{
    [SerializeField] private TubeController bowdenTube; 
    [SerializeField] private TubeController filament;
    [SerializeField] private TubeController cable;

    public void UpdateBowdenAssembly(Vector3 printHeadDelta)
    {
        bowdenTube.UpdateTube(printHeadDelta);
        filament.UpdateTube(printHeadDelta);
        cable.UpdateTube(printHeadDelta);
    }
}
