using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  private Vector2 _moveDir = Vector2.zero;
  private float _speed = 5.0f;

  private void Start()
  {
    
  }
  private void Update()
  {
    HandleInput();

    MovePlayer();
  }

  private void HandleInput()
  {
    Vector2 moveDir = Vector2.zero;

    if (Input.GetKey(KeyCode.W))
      moveDir.y += 1;
    
    if (Input.GetKey(KeyCode.S))
      moveDir.y -= 1;
    
    if (Input.GetKey(KeyCode.A))
      moveDir.x -= 1;
    
    if (Input.GetKey(KeyCode.D))
      moveDir.x += 1;

    _moveDir = moveDir.normalized;
  }

  private void MovePlayer()
  {
    Vector3 dir = _moveDir * _speed * Time.deltaTime;
    transform.position += dir;
  }
}
