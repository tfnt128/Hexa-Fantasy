using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;                 // O objeto que a c�mera seguir�
    public float smoothSpeed = 0.125f;       // Velocidade de suaviza��o do movimento da c�mera

    private void LateUpdate()
    {
        if (target != null)
        {
            // Calcula a posi��o alvo da c�mera com base na posi��o do objeto alvo
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y + 16, transform.position.z);

            // Suaviza o movimento da c�mera em dire��o � posi��o alvo
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Atualiza a posi��o da c�mera
            transform.position = smoothedPosition;
        }
    }
}