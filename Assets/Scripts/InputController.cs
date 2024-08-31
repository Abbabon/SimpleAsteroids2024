using UnityEngine;
using UnityEngine.Serialization;

public class InputController : Singleton<InputController>
{
    [SerializeField] private CanvasButton _rotateRightButton;
    [SerializeField] private CanvasButton _rotateLeftButton;
    [SerializeField] private CanvasButton _thrustButton;
    [SerializeField] private CanvasButton _fireButton;
    
    private void Update()
    {
        RotatingLeft = Input.GetKey(KeyCode.LeftArrow) || (_rotateLeftButton != null ? _rotateLeftButton.Pressed : false);
        RotatingRight = Input.GetKey(KeyCode.RightArrow) || (_rotateRightButton != null ? _rotateRightButton.Pressed : false);
        PressingFire = Input.GetKey(KeyCode.Space) || (_fireButton != null ? _fireButton.Pressed : false);
        PressingThrust = Input.GetKey(KeyCode.UpArrow) || (_thrustButton != null ? _thrustButton.Pressed : false);
    }

    public bool RotatingLeft { get; set; }

    public bool RotatingRight { get; set; }

    public bool PressingThrust { get; set; }

    public bool PressingFire { get; set; }
}