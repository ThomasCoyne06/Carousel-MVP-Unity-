using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HorizontalCarousel : CarouselView
{
    [SerializeField] RectTransform rectTransform;
    [SerializeField] float elementWidth;
    [SerializeField] float spacing = 0f;
    [SerializeField] float transitionDuration = 1f;

    [SerializeField] List<Transform> elements;

    Tween moveTween;
    int viewCurrentIndex;

    int PairOffset => MyMath.IsPair(elements.Count) ? 1 : 0;

    bool InAnimation => moveTween != null && moveTween.IsActive() && moveTween.IsPlaying();

    public override void Refresh(int modelCurrentIndex, bool canLooping)
    {
        viewCurrentIndex = canLooping ? elements.Count / 2 - PairOffset : modelCurrentIndex;

        if (canLooping)
        {
            int delta = modelCurrentIndex - viewCurrentIndex;
            int nbToLoop = Mathf.Abs(delta);
            int sign = (int)Mathf.Sign(delta);

            int elementToLoopId = sign > 0 ? 0 : elements.Count - 1;
            int targetId = MyMath.Modulo(elementToLoopId - sign, elements.Count);

            for (int i = 0; i < nbToLoop; i++)
            {
                var temp = elements[elementToLoopId];
                elements.RemoveAt(elementToLoopId);
                elements.Insert(targetId, temp);
            }
        }

        float offsetX = elementWidth + spacing;
        for (int i = 0; i < elements.Count; i++)
        {
            Vector3 pos = elements[i].position;
            pos.x = rectTransform.position.x + offsetX * (i - viewCurrentIndex); //Line
            elements[i].position = pos;
        }
    }

    public override void UpdatePrevious(bool canLooping)
    {
        if (canLooping)
            Loop(-1);
        Swap(-1);
    }

    public override void UpdateNext(bool canLooping)
    {
        if (canLooping)
            Loop(+1);
        Swap(+1);
    }

    void Loop(float dirX)
    {
        if (dirX == 0) return;

        float dx = Mathf.Sign(dirX);

        //int firstId = ViewToModelIndex(currentIndex, elements.Count, 0);
        //int lastId = ViewToModelIndex(currentIndex, elements.Count, elements.Count - 1);

        int elementToMoveId = dx > 0 ? 0 : elements.Count - 1;

        Vector3 newPos = elements[elementToMoveId].transform.position;
        float offset = (elementWidth + spacing) * elements.Count;

        newPos.x += offset * dx;
        elements[elementToMoveId].transform.position = newPos;
    }

    void Swap(float dirX)
    {
        if (dirX == 0) return;

        float dx = (int)Mathf.Sign(dirX);
        float offset = elementWidth + spacing;

        foreach (var element in elements)
        {
            float endValue = element.transform.position.x - offset * dx;
            moveTween = element.transform.DOMoveX(endValue, transitionDuration);
        }
    }

    public override bool CanSwap()
    {
        return !InAnimation;
    }
}