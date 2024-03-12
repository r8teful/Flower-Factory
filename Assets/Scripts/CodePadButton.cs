public class CodePadButton : Interactable {
    private bool _canInteract = true;


    protected override void OnMouseDown() {
        if (_canInteract) {
            gameObject.GetComponent<MovingButton>().ActivateButton();
            gameObject.GetComponentInParent<CodeHandler>().ButtonPressed(transform.GetSiblingIndex());
        }
    }
}
