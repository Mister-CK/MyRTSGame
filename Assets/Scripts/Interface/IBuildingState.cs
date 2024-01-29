using System;
using MyRTSGame.Model;
namespace MyRTSGame.Interface
{

    public interface IBuildingState
    {
        void OnClick(Building building);
        void SetObject(Building building);
    }

}