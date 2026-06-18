using System;
using UnityEngine;


public class UnsortedGroup : MonoBehaviour
{
    [SerializeField] private ArtefactGroup group;

    private void Start()
    {
        foreach (var a in group.artefacts)
        {
            Artefact artefact = Instantiate(a);
            Group artefactGroup = Group.GetGroup(artefact.Category);
            //TODO: Add description to group
            //TODO: Randomise positions slightly
        }
    }
}
