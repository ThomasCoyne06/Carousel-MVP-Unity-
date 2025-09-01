using System;
using System.Collections.Generic;
using UnityEngine;

public class CarouselModel : MonoBehaviour
{
    public Action OnGoToPrevious;
    public Action OnGoToNext;

    [SerializeField] bool canLooping;
    [SerializeField] int currentIndex;
    [SerializeField] List<string> elements;

    //[field: SerializeField]
    public List<string> Elements => elements;
    public string CurrentElement => elements[currentIndex];
    public int CurrentIndex => currentIndex;
    public int Count => elements.Count;
    public bool CanLooping => canLooping;

    private void Awake()
    {
        currentIndex = Mathf.Clamp(currentIndex, 0, elements.Count - 1);
    }


    //public CarouselModel(List<GameObject> elements, int currentIndex, bool canLooping)
    //{
    //    this.elements = elements ?? new List<GameObject>();
    //    this.currentIndex = currentIndex;
    //    this.canLooping = canLooping;
    //}

    public void Previous()
    {
        if (Count == 0) return;
        if (!canLooping && currentIndex == 0) return;

        OnGoToPrevious?.Invoke();

        currentIndex = MyMath.Modulo(currentIndex - 1, Count);
    }

    public void Next()
    {
        if (Count == 0) return;
        if (!canLooping && currentIndex == Count - 1) return;

        OnGoToNext?.Invoke();

        currentIndex = MyMath.Modulo(currentIndex + 1, Count);
    }

    public void AddElement(string element)
    {
        elements.Add(element);
    }

    public void RemoveElement(string element)
    {
        elements.Remove(element);
        if (currentIndex >= Count)
        {
            currentIndex = Mathf.Max(0, Count - 1);
        }
    }

    public void Clear()
    {
        elements.Clear();
        currentIndex = 0;
    }

}