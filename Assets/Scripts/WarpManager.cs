using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : Singleton<WarpManager>
{
    [SerializeField] private Camera _camera;
    
    public void KeepInBounds(Transform keptTransform)
    {
        var screenPoint = _camera.WorldToViewportPoint(keptTransform.position);
        var outOfBounds = false;

        switch (screenPoint.x)
        {
            case < 0:
                screenPoint.x = 1;
                outOfBounds = true;
                break;
            case > 1:
                screenPoint.x = 0;
                outOfBounds = true;
                break;
        }
        
        switch (screenPoint.y)
        {
            case < 0:
                screenPoint.y = 1;
                outOfBounds = true;
                break;
            case > 1:
                screenPoint.y = 0;
                outOfBounds = true;
                break;
        }

        if (!outOfBounds) return;
        
        var updatedWorldPosition = _camera.ViewportToWorldPoint(screenPoint);
        keptTransform.position = updatedWorldPosition;
    }
}
