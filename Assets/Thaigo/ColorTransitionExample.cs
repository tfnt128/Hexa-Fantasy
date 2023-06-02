using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorTransitionExample : MonoBehaviour
{
    public Image image; // Referência para o componente Image do objeto Canvas
    public Color targetColor; // Cor para a qual você deseja transicionar

    private Color originalColor; // Cor original do objeto

    void Start()
    {
        originalColor = image.color;
        originalColor.a = 1f; // Definir o canal alpha como 1 para a cor original
    }

    public void StartTransition()
    {
        Color targetColorWithAlpha = targetColor;
        targetColorWithAlpha.a = originalColor.a; // Manter o mesmo valor de alpha para a cor de destino

        LeanTween.value(image.gameObject, originalColor, targetColorWithAlpha, 1f)
            .setOnUpdate((Color value) =>
            {
                image.color = value;
            })
            .setEase(LeanTweenType.easeInOutQuad)
            .setOnComplete(() =>
            {

                LeanTween.value(image.gameObject, targetColorWithAlpha, originalColor, 1f)
                    .setOnUpdate((Color value) =>
                    {
                        image.color = value;
                    })
                    .setEase(LeanTweenType.easeInOutQuad);
            });
    }
}
