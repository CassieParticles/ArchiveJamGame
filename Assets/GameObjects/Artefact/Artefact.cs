using JetBrains.Annotations;
using UnityEngine;

public class Artefact : MonoBehaviour
{
    [SerializeField] [CanBeNull] private Category category;

    public Category Category => category; 
}