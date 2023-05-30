using UnityEngine;

public class InstantFollow : MonoBehaviour
{
    public Transform target; // O objeto a ser seguido

    private void Update()
    {
        if (target != null)
        {
            // Mantém a posição Y atual do objeto
            Vector3 newPosition = transform.position;
            newPosition.y = target.position.y;

            // Define a nova posição do objeto para seguir o jogador apenas em X e Z
            newPosition.x = target.position.x;
            newPosition.z = target.position.z;

            // Atualiza a posição do objeto
            transform.position = newPosition;
        }
    }
}
