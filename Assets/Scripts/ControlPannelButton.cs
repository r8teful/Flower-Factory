
public class ControlPannelButton : Interactable {
    protected override void OnMouseDown() {
        gameObject.GetComponent<MovingButtonLocal>().ActivateButton();
        gameObject.GetComponentInParent<ControlPannel>().ButtonPressed(transform.GetSiblingIndex());
        AudioController.Instance.PlaySound3D("buttonPushQuick", transform.position, 0.3f);
    }
}