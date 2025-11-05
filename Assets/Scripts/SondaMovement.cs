using UnityEngine;
using UnityEngine.InputSystem; // 1. IMPORTANTE: Adicionar esta linha

public class SondaMovement : MonoBehaviour
{
    [Header("Configurações")]
    public float moveSpeed = 1.5f;

    private Camera arCamera;

    void Start()
    {
        // Encontra a câmara principal da cena (normalmente a AR Camera)
        arCamera = Camera.main;

        if (arCamera == null)
        {
            Debug.LogError("SondaMovement.cs: Não foi possível encontrar a Camera.main! " +
                           "Verifique se a sua AR Camera tem a tag 'MainCamera'.");
        }
    }

    void Update()
    {
        // 2. Verifica se a câmara existe E se um teclado está presente
        if (arCamera == null || Keyboard.current == null) return; 

        // Obtém a orientação da câmara
        Transform camTransform = arCamera.transform;

        // Calcula a direção do movimento baseada nos inputs e na direção da câmara
        Vector3 moveDirection = Vector3.zero;

        // 3. Substituímos "Input.GetKey" por "Keyboard.current.[tecla].IsPressed()"

        // Cima / Baixo (relativo à câmara)
        if (Keyboard.current.wKey.IsPressed())
            moveDirection += camTransform.up;
        if (Keyboard.current.sKey.IsPressed())
            moveDirection -= camTransform.up;

        // Esquerda / Direita (relativo à câmara)
        if (Keyboard.current.aKey.IsPressed())
            moveDirection -= camTransform.right;
        if (Keyboard.current.dKey.IsPressed())
            moveDirection += camTransform.right;

        // Frente / Trás (relativo à câmara)
        if (Keyboard.current.eKey.IsPressed()) // E para a frente
            moveDirection += camTransform.forward;
        if (Keyboard.current.qKey.IsPressed()) // Q para trás
            moveDirection -= camTransform.forward;

        // Normaliza o vetor para que o movimento diagonal não seja mais rápido
        // E aplica a velocidade e o Time.deltaTime
        Vector3 movement = moveDirection.normalized * moveSpeed * Time.deltaTime;

        // Aplica o movimento no espaço do "Mundo" (World Space)
        transform.Translate(movement, Space.World);
    }
}