using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] RawImage image;
    [SerializeField] float xScrollSpeed, yScrollSpeed;
 
    void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(xScrollSpeed,yScrollSpeed) * Time.deltaTime,image.uvRect.size);
    }
}
