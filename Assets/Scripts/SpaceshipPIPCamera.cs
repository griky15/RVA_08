using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class SpaceshipPIPCamera : MonoBehaviour
{
    [Header("Configurações da Câmara da Nave")]
    [Tooltip("Offset da câmara na direção X (esquerda/direita)")]
    public float cameraOffsetX = 0f;
    
    [Tooltip("Offset da câmara na direção Y (cima/baixo)")]
    public float cameraOffsetY = 0f;
    
    [Tooltip("Distância da câmara ao nariz da nave na direção Z (frente)")]
    public float cameraOffsetZ = 1f;
    
    [Tooltip("Field of View da câmara da nave")]
    [Range(30f, 120f)]
    public float cameraFOV = 70f;
    
    [Header("Configurações de Troca de Câmara")]
    [Tooltip("Tecla para alternar entre câmara AR e câmara da nave (padrão: V)")]
    public Key toggleKey = Key.V;
    
    [Tooltip("Começar com a câmara da nave ativa? (False = começa com AR Camera)")]
    public bool startWithSpaceshipCamera = false;
    
    private Camera spaceshipCamera;
    private GameObject spaceshipCameraObject;
    private Camera arCamera;
    private bool isSpaceshipCameraActive = false;

    void Start()
    {
        // Guarda referência à câmara AR (MainCamera)
        arCamera = Camera.main;
        
        if (arCamera == null)
        {
            Debug.LogError("SpaceshipPIPCamera: Não foi encontrada a câmara principal (MainCamera)!");
            return;
        }

        // Cria a câmara da nave
        CreateSpaceshipCamera();
        
        // Configura o estado inicial
        if (startWithSpaceshipCamera)
        {
            SwitchToSpaceshipCamera();
        }
        else
        {
            SwitchToARCamera();
        }
        
        Debug.Log($"SpaceshipPIPCamera: Iniciado. Câmara inicial: {(isSpaceshipCameraActive ? "Nave" : "AR")}");
        Debug.Log($"SpaceshipPIPCamera: Pressiona '{toggleKey}' para alternar entre câmaras.");
    }

    void CreateSpaceshipCamera()
    {
        // Cria um GameObject para a câmara da nave
        spaceshipCameraObject = new GameObject("SpaceshipCamera");
        
        // Define como filho da nave para seguir o movimento
        spaceshipCameraObject.transform.SetParent(transform);
        
        // Posiciona a câmara no nariz da nave usando coordenadas locais
        // Z positivo = frente da nave
        spaceshipCameraObject.transform.localPosition = new Vector3(cameraOffsetX, cameraOffsetY, cameraOffsetZ);
        
        // A câmara olha para a frente (direção da nave)
        spaceshipCameraObject.transform.localRotation = Quaternion.identity;
        
        // Adiciona o componente Camera
        spaceshipCamera = spaceshipCameraObject.AddComponent<Camera>();
        spaceshipCamera.fieldOfView = cameraFOV;
        spaceshipCamera.depth = 0; // Mesma profundidade que a câmara principal
        spaceshipCamera.clearFlags = CameraClearFlags.SolidColor;
        spaceshipCamera.backgroundColor = Color.black;
        spaceshipCamera.nearClipPlane = 0.01f;
        spaceshipCamera.farClipPlane = 2000f;

        // Copia o cullingMask da câmara AR (exclui UI - Layer 5)
        if (arCamera != null)
        {
            int uiLayer = 5;
            spaceshipCamera.cullingMask = arCamera.cullingMask & ~(1 << uiLayer);
        }
        else
        {
            spaceshipCamera.cullingMask = ~0 & ~(1 << 5);
        }
        
        // Inicialmente desativada (será ativada quando trocarmos)
        spaceshipCamera.enabled = false;

        // Configuração específica do URP
        var urpData = spaceshipCameraObject.GetComponent<UniversalAdditionalCameraData>();
        if (urpData == null)
        {
            urpData = spaceshipCameraObject.AddComponent<UniversalAdditionalCameraData>();
        }
        urpData.renderPostProcessing = false;
        urpData.requiresDepthTexture = false;
        urpData.requiresColorTexture = false;
        
        Debug.Log("SpaceshipPIPCamera: Câmara da nave criada no nariz.");
        Debug.Log($"SpaceshipPIPCamera: Posição local da câmara: ({cameraOffsetX}, {cameraOffsetY}, {cameraOffsetZ})");
    }
    
    void SwitchToSpaceshipCamera()
    {
        if (spaceshipCamera != null && arCamera != null)
        {
            // Desativa a câmara AR
            arCamera.enabled = false;
            
            // Ativa a câmara da nave
            spaceshipCamera.enabled = true;
            
            // Remove o AudioListener da câmara AR para evitar conflitos
            AudioListener arListener = arCamera.GetComponent<AudioListener>();
            if (arListener != null)
            {
                arListener.enabled = false;
            }
            
            // Adiciona AudioListener à câmara da nave se não existir
            AudioListener spaceshipListener = spaceshipCamera.GetComponent<AudioListener>();
            if (spaceshipListener == null)
            {
                spaceshipCameraObject.AddComponent<AudioListener>();
            }
            else
            {
                spaceshipListener.enabled = true;
            }
            
            isSpaceshipCameraActive = true;
            Debug.Log("SpaceshipPIPCamera: Trocado para câmara da nave.");
        }
    }
    
    void SwitchToARCamera()
    {
        if (spaceshipCamera != null && arCamera != null)
        {
            // Desativa a câmara da nave
            spaceshipCamera.enabled = false;
            
            // Ativa a câmara AR
            arCamera.enabled = true;
            
            // Remove o AudioListener da câmara da nave
            AudioListener spaceshipListener = spaceshipCamera.GetComponent<AudioListener>();
            if (spaceshipListener != null)
            {
                spaceshipListener.enabled = false;
            }
            
            // Reativa o AudioListener da câmara AR
            AudioListener arListener = arCamera.GetComponent<AudioListener>();
            if (arListener != null)
            {
                arListener.enabled = true;
            }
            
            isSpaceshipCameraActive = false;
            Debug.Log("SpaceshipPIPCamera: Trocado para câmara AR.");
        }
    }
    
    void ToggleCamera()
    {
        if (isSpaceshipCameraActive)
        {
            SwitchToARCamera();
        }
        else
        {
            SwitchToSpaceshipCamera();
        }
    }

    void Update()
    {
        // Verifica se a tecla de toggle foi pressionada
        if (Keyboard.current != null && Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            ToggleCamera();
        }
    }

    void LateUpdate()
    {
        // Garante que a câmara sempre está posicionada e alinhada com a nave
        if (spaceshipCameraObject != null && transform != null)
        {
            // Mantém a posição local relativa à nave
            spaceshipCameraObject.transform.localPosition = new Vector3(cameraOffsetX, cameraOffsetY, cameraOffsetZ);
            
            // Mantém a rotação alinhada com a nave (olha para a frente)
            spaceshipCameraObject.transform.localRotation = Quaternion.identity;
        }
    }

    void OnDestroy()
    {
        // Se a câmara da nave estiver ativa, volta para a AR antes de destruir
        if (isSpaceshipCameraActive && arCamera != null)
        {
            SwitchToARCamera();
        }
    }
    
    // Métodos públicos para controlar a câmara (opcional)
    public void EnableSpaceshipCamera()
    {
        SwitchToSpaceshipCamera();
    }
    
    public void EnableARCamera()
    {
        SwitchToARCamera();
    }
    
    public bool IsSpaceshipCameraActive()
    {
        return isSpaceshipCameraActive;
    }
}

