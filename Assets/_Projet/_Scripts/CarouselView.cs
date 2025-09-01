using System.Collections.Generic;
using UnityEngine;

public abstract class CarouselView : MonoBehaviour
{
    //CarouselPresenter presenter;

    //protected virtual void Awake()
    //{
    //    presenter = new CarouselPresenter(model, this);
    //}


    public virtual void Refresh(CarouselPresenter presenter, int currentIndex, bool canLooping)
    {

    }

    //public virtual void CommandPrevious()
    //{
    //    presenter.Previous();
    //}

    //public virtual void CommandNext()
    //{
    //    presenter.Next();
    //}

    public virtual void UpdatePrevious(bool canLooping)
    {

    }

    public virtual void UpdateNext(bool canLooping)
    {

    }


    public abstract void UpdateGoTo(int modelPrevId, int modelNextId);
    public abstract void UpdateButtonsGoToColor(int modelPrevId, int modelNextId);


    public abstract bool CanSwap();

}
