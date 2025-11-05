using UnityEngine;
using UnityEngine.InputSystem;

public class SondaMovement : MonoBehaviour
{
    [Header("Configurações")]
    public float moveSpeed = 1.5f;
    public float rotationSpeed = 180f; // Velocidade da rotação em graus/segundo

    private Camera arCamera;
    private bool isRotating180 = false;
    private Quaternion targetRotation;
    private Quaternion startRotation;
    private float rotationProgress = 0f;

    void Start()
    {
        arCamera = Camera.main;

        if (arCamera == null)
        {
            Debug.LogError("SondaMovement.cs: Não foi possível encontrar a Camera.main! " +
                           "Verifique se a sua AR Camera tem a tag 'MainCamera'.");
        }
    }

    void Update()
    {
        if (arCamera == null || Keyboard.current == null) return;

        SpaceshipPIPCamera spaceshipCameraScript = GetComponent<SpaceshipPIPCamera>();
        bool isSpaceshipCameraActive = spaceshipCameraScript != null && spaceshipCameraScript.IsSpaceshipCameraActive();

        Vector3 moveDirection = Vector3.zero;

        if (isSpaceshipCameraActive)
        {
            // Verifica se pressionou Z para dar a volta de 180°
            if (Keyboard.current.zKey.wasPressedThisFrame && !isRotating180)
            {
                StartRotation180();
            }

            // Executa a rotação de 180° se estiver ativa
            if (isRotating180)
            {
                PerformRotation180();
                // Não permite movimento enquanto está a rodar
                return;
            }

            // Controlos quando a câmara da nave está ativa (relativo à nave)
            // W - Frente (relativo à nave)
            if (Keyboard.current.wKey.IsPressed())
                moveDirection += transform.forward;
            
            // S - Trás (relativo à nave)
            if (Keyboard.current.sKey.IsPressed())
                moveDirection -= transform.forward;
            
            // A - Esquerda (relativo à nave)
            if (Keyboard.current.aKey.IsPressed())
                moveDirection -= transform.right;
            
            // D - Direita (relativo à nave)
            if (Keyboard.current.dKey.IsPressed())
                moveDirection += transform.right;
            
            // Normaliza o vetor para que o movimento diagonal não seja mais rápido
            Vector3 movement = moveDirection.normalized * moveSpeed * Time.deltaTime;

            // Aplica o movimento no espaço do "Mundo" (World Space)
            transform.Translate(movement, Space.World);
            
            // 1 - Subir (relativo à nave - eixo UP da nave)
            if (Keyboard.current.digit1Key.IsPressed())
            {
                transform.position += transform.up * moveSpeed * Time.deltaTime;
            }
            // 2 - Descer (relativo à nave - eixo DOWN da nave)
            if (Keyboard.current.digit2Key.IsPressed())
            {
                transform.position -= transform.up * moveSpeed * Time.deltaTime;
            }
        }
        else
        {
            // Controlos originais quando a câmara AR está ativa (relativo à câmara AR)
            Transform camTransform = arCamera.transform;

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
            if (Keyboard.current.eKey.IsPressed())
                moveDirection += camTransform.forward;
            if (Keyboard.current.qKey.IsPressed())
                moveDirection -= camTransform.forward;

            Vector3 movement = moveDirection.normalized * moveSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World);
        }
    }

    void StartRotation180()
    {
        isRotating180 = true;
        startRotation = transform.rotation;
        // Roda 180° no eixo Y (eixo vertical global)
        targetRotation = transform.rotation * Quaternion.Euler(0f, 180f, 0f);
        rotationProgress = 0f;
        Debug.Log("SondaMovement: Iniciando rotação de 180°");
    }

    void PerformRotation180()
    {
        // Incrementa o progresso da rotação
        rotationProgress += (rotationSpeed / 180f) * Time.deltaTime;

        if (rotationProgress >= 1f)
        {
            // Rotação completa
            transform.rotation = targetRotation;
            isRotating180 = false;
            rotationProgress = 1f;
            Debug.Log("SondaMovement: Rotação de 180° concluída");
        }
        else
        {
            // Interpola suavemente entre a rotação inicial e final
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, rotationProgress);
        }
    }
}