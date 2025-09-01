using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class HorizontalCarousel : CarouselView
{
    [SerializeField] RectTransform windowTransform;
    [SerializeField] float elementWidth;
    [SerializeField] float spacing = 0f;
    [SerializeField] float transitionDuration = 1f;

    [SerializeField] List<Transform> elements;

    [SerializeField] RectTransform buttonsGoToParent;
    [SerializeField] Button buttonGoToPrefab;
    [SerializeField] Color buttonGoToColor;

    CarouselPresenter presenter;
    Tween moveTween;
    int viewCurrentIndex;

    int loopOffset;

    bool InAnimation => moveTween != null && moveTween.IsActive() && moveTween.IsPlaying();

    public override void Refresh(CarouselPresenter presenter, int modelCurrentIndex, bool canLooping)
    {
        this.presenter = presenter;
        viewCurrentIndex = modelCurrentIndex;

        float offsetX = elementWidth + spacing;
        for (int i = 0; i < elements.Count; i++)
        {
            Vector3 pos = elements[i].position;
            pos.x = windowTransform.position.x + offsetX * (i - viewCurrentIndex);
            elements[i].position = pos;

            if (buttonGoToPrefab != null && buttonsGoToParent != null)
            {
                Button button = Instantiate(buttonGoToPrefab, buttonsGoToParent);
                int index = i; // très important : sinon tous les boutons prennent la même valeur !
                button.onClick.AddListener(() => presenter.CommandGoTo(index));
            }
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

    public override void UpdateGoTo(int modelPrevId, int modelNextId)
    {
        int viewPrevId = ModelToViewId(modelPrevId);
        int viewNextId = ModelToViewId(modelNextId);
        int deltaX = viewNextId - viewPrevId;
        Swap(deltaX);
    }

    public override void UpdateButtonsGoToColor(int modelPrevId, int modelNextId)
    {
        if (buttonGoToPrefab == null || buttonsGoToParent == null) return;

        var prevButton = buttonsGoToParent.GetChild(modelPrevId);
        var nextButton = buttonsGoToParent.GetChild(modelNextId);

        if (prevButton.TryGetComponent(out Image prevImage) &&
            nextButton.TryGetComponent(out Image nextImage))
        {
            var temp = nextImage.color;
            nextImage.color = buttonGoToColor;
            prevImage.color = temp;
        }
    }

    public override bool CanSwap()
    {
        return !InAnimation;
    }

    void Loop(int deltaX)
    {
        if (deltaX == 0) return;

        int dx = (int)Mathf.Sign(deltaX);

        if ((viewCurrentIndex == 0 && dx < 0) || (viewCurrentIndex == elements.Count - 1 && dx > 0))
        {
            int elementToLoopId = dx > 0 ? 0 : elements.Count - 1;
            int targetId = MyMath.Modulo(elementToLoopId - dx, elements.Count);
            float offset = (elementWidth + spacing) * elements.Count;

            for (int i = 0; i < Mathf.Abs(deltaX); i++)
            {
                Vector3 newPos = elements[elementToLoopId].position;
                newPos.x += offset * dx;
                elements[elementToLoopId].position = newPos;

                var temp = elements[elementToLoopId];
                elements.RemoveAt(elementToLoopId);
                elements.Insert(targetId, temp);

                loopOffset = MyMath.Modulo(loopOffset + dx, elements.Count);
            }
        }
    }

    void Swap(int deltaX)
    {
        if (deltaX == 0) return;

        float offset = elementWidth + spacing;

        foreach (var element in elements)
        {
            float endValue = element.transform.position.x - offset * deltaX;
            moveTween = element.transform.DOMoveX(endValue, transitionDuration);
        }

        viewCurrentIndex = Mathf.Clamp(viewCurrentIndex + deltaX, 0, elements.Count - 1);
    }

    int ModelToViewId(int modelId)
    {
        return MyMath.Modulo(modelId - loopOffset, elements.Count);
    }

    int ViewToModelId(int viewId)
    {
        return MyMath.Modulo(viewId + loopOffset, elements.Count);
    }
}