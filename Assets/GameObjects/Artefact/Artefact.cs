using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Artefact : MonoBehaviour
{
    [SerializeField] [CanBeNull] private Category category;

    public Category Category => category;

    bool isCurrentArtefact = false;
    bool isHeld = false;
    bool isHovered;

    [SerializeField] SecondOrderMovement<Vector2> SOM;
    [SerializeField] Vector2 target;


    private void OnMouseOver() {
        //Allowing the player to pick up the current artefact by clicking it
        if (Input.GetMouseButtonDown(0)) {
            isHeld = !isHeld;
            isHovered = true;
            Drop();
             
        }
    }

    private void Update() {
        //This is here so that when not hovering over the artefact (for example its bouncing around past it) the artefact will still be dropped
        if (Input.GetMouseButtonDown(0) && !isHovered) {
            isHeld = false;
            Drop();
        }

        //Move the artefact towards the current target position with Second Order Movement
        if (isHeld) {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        } else {
            target = new Vector2(0, 0);
        }
        transform.position = SOM.Update(Time.deltaTime, (Vector2)transform.position, target);

        //Reset variable for letting go of artefact while not hovering over
        isHovered = false;
    }

    private void Drop() {
        //Call this when dropping the artefact, so it can be checked what group its being put in
    }
}