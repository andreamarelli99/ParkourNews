using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class GroundControllerRedLine : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 2f;
    private SpriteRenderer _spiteRenderer;
    private Color _startColor;
    private Color _endColor;
    

    private void Awake()
    {
        _spiteRenderer = this.GetComponent<SpriteRenderer>();
        _startColor = _spiteRenderer.color;
        _endColor = new Color(_startColor.r, _startColor.g, _startColor.b, 0);
    }

    private void OnEnable()
    {
        Debug.Log(gameObject.name + " onEnable");
        _spiteRenderer.color = _startColor;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("RedLine"))
        {
            Debug.Log("Letter Hit");
            StartCoroutine(FadeAlphaToZero(_spiteRenderer, fadeDuration));
        }
    }

    private IEnumerator FadeAlphaToZero(SpriteRenderer _renderer, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            _renderer.color = Color.Lerp(_startColor, _endColor, time / duration);
            yield return null;
        }
        if (Spawner._stickmanCreated)
        {
            Debug.Log("Letter " + gameObject.name + " Deactivated");
            gameObject.SetActive(false);
        }
    }
}
