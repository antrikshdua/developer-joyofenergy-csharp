namespace JOIEnergy.Services;

public interface IPricePlanService
{
  Dictionary<string, decimal>
    GetConsumptionCostOfElectricityReadingsForEachPricePlan(
      string smartMeterId);
}
