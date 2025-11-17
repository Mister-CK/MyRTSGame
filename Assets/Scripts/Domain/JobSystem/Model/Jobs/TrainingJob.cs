using Enums;
using System.Collections.Generic;

namespace Domain.Model
{
    public class TrainingJob
    {
        public Resource[] Input { get; set; }
        public UnitType UnitType { get; set; }
        public int Quantity { get; set; }
    }
}