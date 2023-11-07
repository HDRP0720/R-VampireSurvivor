using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UVMover : MonoBehaviour
{
  public RawImage rawImage;
  public float speed = 1.0f;
  
  private Rect _uvRect;

  private void Start()
  {
    rawImage = GetComponent<RawImage>();
  }

  private void Update()
  {
    _uvRect = rawImage.uvRect;
    _uvRect.x += Time.deltaTime * speed;
    _uvRect.y += Time.deltaTime * speed;
    rawImage.uvRect = _uvRect;
  }
}
