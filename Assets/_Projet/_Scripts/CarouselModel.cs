using System;
using System.Collections.Generic;
using UnityEngine;

public class CarouselModel : MonoBehaviour
{
    public Action OnPrevious;
    public Action OnNext;
    public Action<int, int> OnGoTo;

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

    public void Previous()
    {
        if (Count == 0) return;
        if (!canLooping && currentIndex == 0) return;

        currentIndex = MyMath.Modulo(currentIndex - 1, Count);
        OnPrevious?.Invoke();
    }

    public void Next()
    {
        if (Count == 0) return;
        if (!canLooping && currentIndex == Count - 1) return;

        currentIndex = MyMath.Modulo(currentIndex + 1, Count);
        OnNext?.Invoke();
    }

    public void GoTo(int index)
    {
        if (Count == 0) return;
        OnGoTo?.Invoke(currentIndex, index);
        currentIndex = index;
    }

    public void AddElement(string element)
    {
        elements.Add(element);
    }

    public void RemoveElement(string element)
    {
        if (Count == 0) return;

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