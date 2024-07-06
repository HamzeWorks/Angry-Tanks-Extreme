using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collision2D))]
public class Trigger2DEventHandler : MonoBehaviour
{
    public event Action<Collider2D> onTriggerEnter2D;
    public event Action<Collider2D> onTriggerStay2D;
    public event Action<Collider2D> onTriggerExit2D;

    private void OnTriggerEnter2D(Collider2D collision) => onTriggerEnter2D?.Invoke(collision);
    private void OnTriggerStay2D(Collider2D collision) => onTriggerStay2D?.Invoke(collision);
    private void OnTriggerExit2D(Collider2D collision) => onTriggerExit2D?.Invoke(collision);
}
