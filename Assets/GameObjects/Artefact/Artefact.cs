using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Artefact : MonoBehaviour
{
    [SerializeField] [CanBeNull] private Category category;

    public Category Category => category;

    bool isCurrentArtefact = false;
    bool isHeld = false;
    bool isHovered;
    public bool isPlaced = false;

    [SerializeField] SecondOrderMovement<Vector2> SOM;
    [SerializeField] Vector3 SmoothMovementValues;
    [SerializeField] SecondOrderMovement<Vector2> SOMShrink;
    [SerializeField] Vector2 target;

    [SerializeField] private LayerMask layermask;

    private void OnMouseOver() {
        //Allowing the player to pick up the current artefact by clicking it
        if (Input.GetMouseButtonDown(0) && !isPlaced) {
            isHeld = !isHeld;
            isHovered = true;
            Drop();
             
        }
    }

    private void Update() {
        //This is here so that when not hovering over the artefact (for example its bouncing around past it) the artefact will still be dropped
        if (Input.GetMouseButtonDown(0) && isHeld && !isHovered) {
            isHeld = false;
            Drop();
        }

        //Move the artefact towards the current target position with Second Order Movement
        if (!isPlaced) {
            if (isHeld) {
                target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            } else {
                target = new Vector2(0, 0);
            }
            transform.position = SOM.Update(Time.deltaTime, (Vector2)transform.position, target);
        }

        //Reset variable for letting go of artefact while not hovering over
        isHovered = false;
    }

    private void Drop() {
        //Call this when dropping the artefact, so it can be checked what group its being put in
        //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        UnsortedGroup unsorted = FindAnyObjectByType<UnsortedGroup>();

        //Debug.Log("Sent Ray " + ray.ToString());
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider.gameObject.GetComponent<Group>()) {
                isPlaced = true;
                StartCoroutine(DelayBeforeCheck(1, hit.transform.position, hit));
                
            }
        }

    }

    //Forgive me for this mess
    IEnumerator DelayBeforeCheck(float delay, Vector3 targetPos, RaycastHit hit) {
        float timer = 0;

        Vector3 oldValues = new Vector3(SOM.frequency, SOM.zeta, SOM.response);
        SOM.frequency = SmoothMovementValues.x;
        SOM.zeta = SmoothMovementValues.y;
        SOM.response = SmoothMovementValues.z;

        target = targetPos;
        Vector2 targetSize = Vector2.zero;

        UnsortedGroup unsorted = FindAnyObjectByType<UnsortedGroup>();

        while (timer < delay) {
            transform.position = SOM.Update(Time.deltaTime, (Vector2)transform.position, target);
            transform.localScale = SOMShrink.Update(Time.deltaTime, (Vector2)transform.localScale, targetSize);

            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;
        }
        unsorted.TryGroup(hit.collider.gameObject.GetComponent<Group>());
        if (!isPlaced) {
            SOM.frequency = oldValues.x;
            SOM.zeta = oldValues.y;
            SOM.response = oldValues.z;
            transform.localScale = new Vector3(1, 1, 1);
            SOMShrink = new SecondOrderMovement<Vector2>();
            SOMShrink.frequency = SmoothMovementValues.x;
            SOMShrink.zeta = SmoothMovementValues.y;
            SOMShrink.response = SmoothMovementValues.z;
        }
    }
}