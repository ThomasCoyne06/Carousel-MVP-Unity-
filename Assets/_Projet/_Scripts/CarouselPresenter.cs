using System.Collections.Generic;
using UnityEngine;

public class CarouselPresenter : MonoBehaviour
{
    [SerializeField] CarouselModel model;
    [SerializeField] CarouselView view;

    private void Awake()
    {
        model.OnGoToPrevious += () => view.UpdatePrevious(model.CanLooping);
        model.OnGoToNext += () => view.UpdateNext(model.CanLooping);
    }

    void Start()
    {
        view.Refresh(model.CurrentIndex, model.CanLooping);
    }


    //public CarouselPresenter(CarouselModel model, CarouselView view)
    //{
    //    model.OnGoToPrevious += () => view.UpdatePrevious(model.Elements, model.CurrentIndex, model.CanLooping);
    //    model.OnGoToNext += () => view.UpdateNext(model.Elements, model.CurrentIndex, model.CanLooping);

    //    this.model = model;
    //    this.view = view;
    //    this.view.Refresh(model.Elements, model.CurrentIndex, model.CanLooping);
    //}



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

    public void UpdatePrevious()
    {
        view.UpdatePrevious(model.CanLooping);
    }

    public void UpdateNext()
    {
        view.UpdateNext(model.CanLooping);
    }

    int ViewToModelIndex(int currentIndex, int count, int viewIndex)
    {
        bool pair = count % 2 == 0;
        int offset = pair ? 1 : 0;
        return MyMath.Modulo(currentIndex - count / 2 + viewIndex + offset, count);
    }

    int ModelToViewIndex(int currentIndex, int count, int modelIndex)
    {
        bool pair = count % 2 == 0;
        int offset = pair ? 1 : 0;
        return MyMath.Modulo(modelIndex - currentIndex + count / 2 - offset, count);
    }
}
