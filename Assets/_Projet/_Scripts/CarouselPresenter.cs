using System.Collections.Generic;
using UnityEngine;

public class CarouselPresenter : MonoBehaviour
{
    [SerializeField] CarouselModel model;
    [SerializeField] CarouselView view;

    private void Awake()
    {
        model.OnPrevious += () => UpdatePrevious();
        model.OnNext += () => UpdateNext();
        model.OnGoTo += UpdateGoTo;
    }

    void Start()
    {
        view.Refresh(this, model.CurrentIndex, model.CanLooping);
        view.UpdateButtonsGoToColor(0, model.CurrentIndex);
    }

    public void CommandPrevious()
    {
        if (!view.CanSwap()) return;
        model.Previous();
    }

    public void CommandNext()
    {
        if (!view.CanSwap()) return;
        model.Next();
    }

    public void CommandGoTo(int order)
    {
        if (!view.CanSwap()) return;
        model.GoTo(order);
    }

    public void UpdatePrevious()
    {
        view.UpdatePrevious(model.CanLooping);

        int prevId = MyMath.Modulo(model.CurrentIndex + 1, model.Count);
        view.UpdateButtonsGoToColor(prevId, model.CurrentIndex);
    }

    public void UpdateNext()
    {
        view.UpdateNext(model.CanLooping);

        int prevId = MyMath.Modulo(model.CurrentIndex - 1, model.Count);
        view.UpdateButtonsGoToColor(prevId, model.CurrentIndex);
    }

    public void UpdateGoTo(int modelPrevId, int modelNextId)
    {
        view.UpdateGoTo(modelPrevId, modelNextId);
        view.UpdateButtonsGoToColor(modelPrevId, modelNextId);
    }

}
