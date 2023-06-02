using UnityEngine;
using System.Collections;

public class MoveObjectExample : MonoBehaviour
{
    public Transform targetPoint; // Ponto de destino para onde o objeto deve se mover

    private Vector3 originalPosition; // Posição original do objeto

    private Vector3 originalScale;

    public bool isPlayer;

    void Start()
    {
        if (!isPlayer)
        {
            targetPoint = GameObject.FindGameObjectWithTag("EnemyTargetPoint").transform;
        }
        else
        {
            targetPoint = GameObject.FindGameObjectWithTag("PlayerTargetPoint").transform;
        }
        
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
    float maxScaleEnemy = 1.5f;
    float minScaleEnemy = 1.0f;

    float maxScalePlayer = 1.0f;
    float minScalePlayer = .7f;
    void UpdateScale(float ratio)
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPoint.position);
        
        if (!isPlayer)
        {
            float scaleMultiplier = Mathf.Lerp(maxScaleEnemy, minScaleEnemy, distanceToTarget / Vector3.Distance(originalPosition, targetPoint.position));
            transform.localScale = originalScale * scaleMultiplier;
        }
        else
        {
            float scaleMultiplier = Mathf.Lerp(minScalePlayer, maxScalePlayer, distanceToTarget / Vector3.Distance(originalPosition, targetPoint.position));
            transform.localScale = originalScale * scaleMultiplier;
        }
        
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveToTargetPoint();
        }
    }
}
