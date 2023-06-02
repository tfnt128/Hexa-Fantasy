using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class BlinkEffectExample : MonoBehaviour
{
    public float blinkDuration = 0.2f; // Duração de cada piscada
    public int blinkCount = 5; // Número de piscadas
    public float blinkInterval = 0.2f; // Intervalo entre as piscadas

    private SpriteRenderer spriteRenderer; // Componente SpriteRenderer do objeto
    private Color originalColor; // Cor original do objeto

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void TriggerBlinkEffect()
    {
        StartCoroutine(BlinkEffect());
    }

    IEnumerator BlinkEffect()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            LeanTween.alpha(spriteRenderer.gameObject, 1f, blinkDuration).setEase(LeanTweenType.easeInOutQuad);

            yield return new WaitForSeconds(blinkDuration + blinkInterval);

            spriteRenderer.color = originalColor;
            LeanTween.alpha(spriteRenderer.gameObject, 0f, blinkDuration).setEase(LeanTweenType.easeInOutQuad);

            yield return new WaitForSeconds(blinkDuration + blinkInterval);
        }

        spriteRenderer.color = originalColor;
    }
}
