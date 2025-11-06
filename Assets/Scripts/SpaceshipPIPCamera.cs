using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class SpaceshipPIPCamera : MonoBehaviour
{
    [Header("Configurações da Câmara da Nave")]
    [Tooltip("Offset da câmara na direção X (esquerda/direita)")]
    public float cameraOffsetX = 0f;
    
    [Tooltip("Offset da câmara na direção Y (cima/baixo)")]
    public float cameraOffsetY = 0.2f;
    
    [Tooltip("Distância da câmara ao nariz da nave na direção Z (frente)")]
    public float cameraOffsetZ = 2f;
    
    [Tooltip("Field of View da câmara da nave")]
    [Range(30f, 120f)]
    public float cameraFOV = 90f;

    [Header("Configurações do Skybox da Nave")]
    [Tooltip("Material do Skybox (céu estrelado) a usar na câmara da nave. Se vazio, usa cor sólida.")]
    public Material spaceshipSkyboxMaterial;
    
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
        arCamera = Camera.main;
        if (arCamera == null)
        {
            Debug.LogError("SpaceshipPIPCamera: Não foi encontrada a câmara principal (MainCamera)!");
            return;
        }

        CreateSpaceshipCamera();
        
        if (startWithSpaceshipCamera)
        {
            SwitchToSpaceshipCamera();
        }
        else
        {
            SwitchToARCamera();
        }
    }

    void CreateSpaceshipCamera()
    {
        spaceshipCameraObject = new GameObject("SpaceshipCamera");
        spaceshipCameraObject.transform.SetParent(transform);
        spaceshipCameraObject.transform.localPosition = new Vector3(cameraOffsetX, cameraOffsetY, cameraOffsetZ);
        spaceshipCameraObject.transform.localRotation = Quaternion.identity;
        
        spaceshipCamera = spaceshipCameraObject.AddComponent<Camera>();
        spaceshipCamera.fieldOfView = cameraFOV;
        spaceshipCamera.depth = 0;
        spaceshipCamera.nearClipPlane = 0.05f; // Reduzido para evitar cortar objetos próximos
        spaceshipCamera.farClipPlane = 2000f;

        // --- CONFIGURAÇÃO DO SKYBOX ---
        if (spaceshipSkyboxMaterial != null)
        {
            // Se temos material, configuramos a câmara para usar Skybox
            spaceshipCamera.clearFlags = CameraClearFlags.Skybox;
            
            // Adiciona o componente Skybox à câmara e atribui o material
            Skybox camSkybox = spaceshipCameraObject.AddComponent<Skybox>();
            camSkybox.material = spaceshipSkyboxMaterial;
        }
        else
        {
            // Se não temos material, usamos a cor sólida azul escuro como fallback
            spaceshipCamera.clearFlags = CameraClearFlags.SolidColor;
            spaceshipCamera.backgroundColor = new Color(0.02f, 0.02f, 0.1f, 1f);
        }
        // ------------------------------

        if (arCamera != null)
        {
            int uiLayer = 5;
            spaceshipCamera.cullingMask = arCamera.cullingMask & ~(1 << uiLayer);
        }
        else
        {
            spaceshipCamera.cullingMask = ~0 & ~(1 << 5);
        }
        
        spaceshipCamera.enabled = false;

        var urpData = spaceshipCameraObject.GetComponent<UniversalAdditionalCameraData>();
        if (urpData == null)
        {
            urpData = spaceshipCameraObject.AddComponent<UniversalAdditionalCameraData>();
        }
        urpData.renderPostProcessing = false;
        urpData.requiresDepthTexture = false;
        urpData.requiresColorTexture = false;
    }
    
    void SwitchToSpaceshipCamera()
    {
        if (spaceshipCamera != null && arCamera != null)
        {
            arCamera.enabled = false;
            spaceshipCamera.enabled = true;
            
            AudioListener arListener = arCamera.GetComponent<AudioListener>();
            if (arListener != null) arListener.enabled = false;
            
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
            spaceshipCamera.enabled = false;
            arCamera.enabled = true;
            
            AudioListener spaceshipListener = spaceshipCamera.GetComponent<AudioListener>();
            if (spaceshipListener != null) spaceshipListener.enabled = false;
            
            AudioListener arListener = arCamera.GetComponent<AudioListener>();
            if (arListener != null) arListener.enabled = true;
            
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
        if (Keyboard.current != null && Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            ToggleCamera();
        }
    }

    void LateUpdate()
    {
        if (spaceshipCameraObject != null && transform != null)
        {
            spaceshipCameraObject.transform.localPosition = new Vector3(cameraOffsetX, cameraOffsetY, cameraOffsetZ);
            spaceshipCameraObject.transform.localRotation = Quaternion.identity;
        }
    }

    void OnDestroy()
    {
        if (isSpaceshipCameraActive && arCamera != null)
        {
            SwitchToARCamera();
        }
    }
    
    public void EnableSpaceshipCamera() => SwitchToSpaceshipCamera();
    public void EnableARCamera() => SwitchToARCamera();
    public bool IsSpaceshipCameraActive() => isSpaceshipCameraActive;
}