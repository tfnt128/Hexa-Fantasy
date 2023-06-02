using UnityEngine;
using System.Collections;

public class MoveObjectExample : MonoBehaviour
{
    public Transform targetPoint; // Ponto de destino para onde o objeto deve se mover

    private Vector3 originalPosition; // Posição original do objeto

    private Vector3 originalScale;

    void Start()
    {
        targetPoint = GameObject.FindGameObjectWithTag("EnemyTargetPoint").transform;
        originalScale = transform.localScale;
        originalPosition = transform.position;
    }

    public void MoveToTargetPoint()
    {
        LeanTween.move(gameObject, targetPoint.position, .2f)
            .setEase(LeanTweenType.easeInQuad)
            .setOnUpdate(UpdateScale)
            .setOnComplete(() =>
            {
                LeanTween.move(gameObject, originalPosition, 1f)
                    .setEase(LeanTweenType.easeOutQuad)
                    .setOnUpdate(UpdateScale)
                    .setOnComplete(() => transform.localScale = originalScale);
            });
    }

    void UpdateScale(float ratio)
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPoint.position);
        float maxScale = 1.5f;
        float minScale = 1.0f;
        float scaleMultiplier = Mathf.Lerp(maxScale, minScale, distanceToTarget / Vector3.Distance(originalPosition, targetPoint.position));
        transform.localScale = originalScale * scaleMultiplier;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToTargetPoint();
        }
    }
}
