using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorPlayer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Referência para o componente SpriteRenderer do objeto
    public Color targetColor; // Cor para a qual você deseja transicionar

    private Color originalColor; // Cor original do objeto

    void Start()
    {
        originalColor = spriteRenderer.color;
        originalColor.a = 1f; // Definir o canal alpha como 1 para a cor original
    }

    public void StartTransition()
    {
        Color targetColorWithAlpha = targetColor;
        targetColorWithAlpha.a = originalColor.a; // Manter o mesmo valor de alpha para a cor de destino

        LeanTween.value(spriteRenderer.gameObject, originalColor, targetColorWithAlpha, 1f)
            .setOnUpdate((Color value) =>
            {
                spriteRenderer.color = value;
            })
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {
                LeanTween.value(spriteRenderer.gameObject, targetColorWithAlpha, originalColor, 1f)
                    .setOnUpdate((Color value) =>
                    {
                        spriteRenderer.color = value;
                    })
                    .setEase(LeanTweenType.easeInOutQuad);
            });
    }
}
