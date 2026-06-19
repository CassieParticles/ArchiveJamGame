using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


public class Group : MonoBehaviour
{
    [SerializeField] Category category;
    public Category Category => category;
    
    private List<Artefact> _addedArtefacts = new List<Artefact>();
    private List<GroupDisplayPoint> _groupDisplayPoints = new List<GroupDisplayPoint>();
    private int _nextFreeDisplayIndex = 0;

    private GroupDisplayPoint NextFreeDisplay =>_groupDisplayPoints[_nextFreeDisplayIndex];
    
    private GroupDescription _groupDescription;
    public GroupDescription GroupDescription => _groupDescription;
    
    private void Awake()
    {
        transform.GetComponentsInChildren<GroupDisplayPoint>(_groupDisplayPoints);
        _groupDescription = GetComponent<GroupDescription>();
    }

    [CanBeNull]
    public static Group GetGroup(Category category)
    {
        Group[] groups = FindObjectsByType<Group>(FindObjectsSortMode.None);
        foreach (var group in groups)
        {
            if (group.Category == category)
            {
                return group;
            }
        }
        //ERROR: GROUP NOT FOUND
        return null;
    }

    //Add artefact if it matches group, then return if successful
    public bool AddArtefact(Artefact artefact)
    {
        if (artefact.Category != category)
        {
            //TODO: Wrong artefact
            return false;
        }
        
        //TODO: Right artefact
        _addedArtefacts.Add(artefact);
        
        //Artefact should be positioned and resized to the desired size
        GroupDisplayPoint nextDisplayPoint = NextFreeDisplay;
        artefact.transform.position = nextDisplayPoint.transform.position;
        artefact.transform.localScale = nextDisplayPoint.transform.localScale;
        //TODO: artefact told it should stop listening to mouse input
        
        _nextFreeDisplayIndex++;
        return true;
    }

    private void FixedUpdate()
    {
        Debug.Log(_groupDescription.Description);
    }
}
