using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;                 // O objeto que a câmera seguirá
    public float smoothSpeed = 0.125f;       // Velocidade de suavização do movimento da câmera

    private void LateUpdate()
    {
        if (target != null)
        {
            // Calcula a posição alvo da câmera com base na posição do objeto alvo
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y + 16, transform.position.z);

            // Suaviza o movimento da câmera em direção à posição alvo
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Atualiza a posição da câmera
            transform.position = smoothedPosition;
        }
    }
}