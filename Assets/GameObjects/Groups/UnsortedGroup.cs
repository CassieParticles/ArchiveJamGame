using System;
using System.Collections.Generic;
using UnityEngine;


public class UnsortedGroup : MonoBehaviour
{
    [SerializeField] private ArtefactGroup artefacts;

    private readonly Stack<Artefact> _artefacts = new Stack<Artefact>();

    public Artefact CurrentArtefact => _artefacts.Peek();

    private void Awake()
    {
        //Instantiate the artefacts and add to list
        foreach (var artefact in artefacts.artefacts)
        {
            Artefact instance = Instantiate(artefact);
            instance.name = artefact.name;
            _artefacts.Push(instance);
        }
    }

    private void Start()
    {
        //Set up each artefact
        foreach (var artefact in _artefacts)
        {
            Group artefactGroup = Group.GetGroup(artefact.Category);
            if (artefactGroup == null)
            {
                Debug.LogError("Group not found: " + artefact.Category.name);
                continue;
            }
            //TODO: Add description to group
            artefactGroup.GroupDescription.AddDescription(artefact);
            
            //TODO: Randomise positions slightly
            
            //Disable the game objects
            artefact.gameObject.SetActive(false);
        }
        
        //Enable the first game object
        CurrentArtefact.gameObject.SetActive(true);
    }

    public void TryGroup(Group group)
    {
        Artefact artefact = CurrentArtefact;
        if (group.AddArtefact(artefact))
        {
            //Success
            //Remove top artefact
            _artefacts.Pop();
            CurrentArtefact.gameObject.SetActive(true);
        }
        else
        {
            //Fail
            CurrentArtefact.isPlaced = false;
            //TODO: Fail logic
        }
    }
}
