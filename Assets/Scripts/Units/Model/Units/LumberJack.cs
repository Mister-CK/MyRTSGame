using Unity.VisualScripting;

namespace MyRTSGame.Model
{
    public class LumberJack: Unit
    {
        
        public LumberJack()
        {
            UnitType = UnitType.LumberJack;
        }

        protected override void Start()
        {
            base.Start();
            IsLookingForBuilding = true;
        }
    }
}