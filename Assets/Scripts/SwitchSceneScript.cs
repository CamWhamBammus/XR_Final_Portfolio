using UnityEngine;
using UnityEngine.SceneManagement;
using Oculus.Interaction;
using UnityEngine.XR;

public class SwitchSceneScript : MonoBehaviour
{
    [Tooltip("The interactable to monitor for state changes.")]
    [SerializeField, Interface(typeof(IInteractableView))]
    private UnityEngine.Object _interactableView;

    [Tooltip("The mesh that will change color based on the current state.")]
    [SerializeField]
    private Renderer _renderer;

    [Tooltip("Displayed when the state is normal.")]
    [SerializeField]
    private Color _normalColor = Color.red;

    [Tooltip("Displayed when the state is hover.")]
    [SerializeField]
    private Color _hoverColor = Color.blue;

    [Tooltip("Displayed when the state is selected.")]
    [SerializeField]
    private Color _selectColor = Color.green;

    public Color NormalColor
    {
        get { return _normalColor; }
        set { _normalColor = value; }
    }

    public Color HoverColor
    {
        get { return _hoverColor; }
        set { _hoverColor = value; }
    }

    public Color SelectColor
    {
        get { return _selectColor; }
        set { _selectColor = value; }
    }

    private IInteractableView InteractableView;
    private Material _material;
    private bool _started = false;
    private bool _sceneChangeTriggered = false;

    public static SwitchSceneScript Instance;

    protected virtual void Awake()
    {
        InteractableView = _interactableView as IInteractableView;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //else
        //{
        //    Destroy(gameObject);
        //}
    }

    protected virtual void Start()
    {
        this.BeginStart(ref _started);

        if (InteractableView == null)
        {
            Debug.LogError("InteractableView is not assigned!");
            return;
        }

        if (_renderer == null)
        {
            Debug.LogError("Renderer is not assigned!");
            return;
        }

        _material = _renderer.material;
        UpdateVisual();

        this.EndStart(ref _started);
    }

    protected virtual void OnEnable()
    {
        if (_started && InteractableView != null)
        {
            InteractableView.WhenStateChanged += UpdateVisualState;
            UpdateVisual();
        }
    }

    protected virtual void OnDisable()
    {
        if (_started && InteractableView != null)
        {
            InteractableView.WhenStateChanged -= UpdateVisualState;
        }
    }

    private void OnDestroy()
    {
        if (_material != null)
        {
            Destroy(_material);
        }
    }

    private void UpdateVisual()
    {
        if (_material == null || InteractableView == null) return;

        switch (InteractableView.State)
        {
            case InteractableState.Normal:
                _material.color = _normalColor;
                _sceneChangeTriggered = false;
                break;
            case InteractableState.Hover:
                _material.color = _hoverColor;
                break;
            case InteractableState.Select:
                _material.color = _selectColor;
                if (!_sceneChangeTriggered)
                {
                    _sceneChangeTriggered = true;
                    LoadNextScene();
                }
                break;
        }
    }

    private void UpdateVisualState(InteractableStateChangeArgs args)
    {
        UpdateVisual();
    }

    private void LoadNextScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene("Main");


        if (sceneName == "GameOver")
        {
            Audiomanager.Instance.StartBGmusic();
        }
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
        Audiomanager.Instance.PlayGameOverSound();
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    private void Update()
    {
        // Get left and right hand devices
        InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // Read trigger values (float 0–1)
        leftHand.TryGetFeatureValue(CommonUsages.trigger, out float leftTrigger);
        rightHand.TryGetFeatureValue(CommonUsages.trigger, out float rightTrigger);

        // If both triggers pressed AND scene hasn't already changed
        if ( rightTrigger > 0.9f && leftTrigger > 0.9f)
        {
            Debug.LogError("MAIN SCENE LOADED");
            SceneManager.LoadScene("Main");   // Load your scene
        }
    }

}

