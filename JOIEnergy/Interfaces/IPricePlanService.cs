﻿using System.Collections.Generic;

namespace JOIEnergy.Interfaces
{
    public interface IPricePlanService
    {
        Dictionary<string, decimal> GetConsumptionCostOfElectricityReadingsForEachPricePlan(string smartMeterId);
    }
}