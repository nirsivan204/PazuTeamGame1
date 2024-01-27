using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squenceTrigger : MonoBehaviour
{
    [SerializeField] SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        SquenceManager.instance.ChangeSpriteOnTrigger.AddListener(SpriteChange);
        SquenceManager.instance.RemoveSpriteOnTrigger.AddListener(SpriteRemove);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Legs")
        {
            SquenceManager.instance.OnOtherEnter();
            Debug.Log("Legs here");
        }
    }
    private void SpriteChange(Sprite sprite)
    {
        if (sprite != null) {
            _spriteRenderer.enabled = true;
            _spriteRenderer.sprite = sprite;
        }
    }
    private void SpriteRemove()
    {
        _spriteRenderer.enabled = false;
    }
}
