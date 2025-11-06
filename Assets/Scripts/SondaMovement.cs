using UnityEngine;
using UnityEngine.InputSystem;

public class SondaMovement : MonoBehaviour
{
    [Header("Configurações")]
    public float moveSpeed = 0.5f;
    public float rotationSpeed = 180f; 

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
            Debug.LogError("SondaMovement.cs: Não foi possível encontrar a Camera.main! ");
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
            if (Keyboard.current.zKey.wasPressedThisFrame && !isRotating180)
            {
                StartRotation180();
            }

            if (isRotating180)
            {
                PerformRotation180();
                return;
            }

            // W - Frente
            if (Keyboard.current.wKey.IsPressed())
                moveDirection += transform.forward;
            
            // S - Trás
            if (Keyboard.current.sKey.IsPressed())
                moveDirection -= transform.forward;
            
            // A - Esquerda
            if (Keyboard.current.aKey.IsPressed())
                moveDirection -= transform.right;
            
            // D - Direita
            if (Keyboard.current.dKey.IsPressed())
                moveDirection += transform.right;
            
            Vector3 movement = moveDirection.normalized * moveSpeed * Time.deltaTime;

            transform.Translate(movement, Space.World);
            
            // 1 - Subir
            if (Keyboard.current.digit1Key.IsPressed())
            {
                transform.position += transform.up * moveSpeed * Time.deltaTime;
            }
            // 2 - Descer
            if (Keyboard.current.digit2Key.IsPressed())
            {
                transform.position -= transform.up * moveSpeed * Time.deltaTime;
            }
        }
        else
        {
            Transform camTransform = arCamera.transform;

            if (Keyboard.current.wKey.IsPressed())
                moveDirection += camTransform.up;
            if (Keyboard.current.sKey.IsPressed())
                moveDirection -= camTransform.up;

            if (Keyboard.current.aKey.IsPressed())
                moveDirection -= camTransform.right;
            if (Keyboard.current.dKey.IsPressed())
                moveDirection += camTransform.right;

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
        targetRotation = transform.rotation * Quaternion.Euler(0f, 180f, 0f);
        rotationProgress = 0f;
        Debug.Log("SondaMovement: Iniciar rotação de 180°");
    }

    void PerformRotation180()
    {
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
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, rotationProgress);
        }
    }
}