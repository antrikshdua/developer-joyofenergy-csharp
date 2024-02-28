namespace JOIEnergy.Services;

using Domain;

public class PricePlanService : IPricePlanService
{
  private readonly List<PricePlan> _pricePlans;
  private readonly IMeterReadingService _meterReadingService;

  public PricePlanService(
    List<PricePlan> pricePlan,
    IMeterReadingService meterReadingService)
  {
    _pricePlans = pricePlan;
    _meterReadingService = meterReadingService;
  }

  private static decimal CalculateAverageReading(
    IReadOnlyCollection<ElectricityReading> electricityReadings)
  {
    decimal newSummedReadings = electricityReadings
      .Select(readings => readings.Reading)
      .Aggregate((reading, accumulator) => reading + accumulator);

    return newSummedReadings / electricityReadings.Count;
  }

  private static decimal CalculateTimeElapsed(
    IReadOnlyCollection<ElectricityReading> electricityReadings)
  {
    DateTime first = electricityReadings.Min(reading => reading.Time);
    DateTime last = electricityReadings.Max(reading => reading.Time);

    return (decimal)(last - first).TotalHours;
  }

  private static decimal CalculateCost(
    IReadOnlyCollection<ElectricityReading> electricityReadings,
    PricePlan pricePlan)
  {
    decimal average = CalculateAverageReading(electricityReadings);
    decimal timeElapsed = CalculateTimeElapsed(electricityReadings);
    decimal averagedCost = average / timeElapsed;

    return averagedCost * pricePlan.UnitRate;
  }

  public Dictionary<String, decimal>
    GetConsumptionCostOfElectricityReadingsForEachPricePlan(String smartMeterId)
  {
    IReadOnlyList<ElectricityReading> electricityReadings =
      _meterReadingService.GetReadings(smartMeterId);

    if (!electricityReadings.Any())
    {
      return new Dictionary<string, decimal>();
    }

    return _pricePlans.ToDictionary(
      plan => plan.EnergySupplier.ToString(),
      plan => CalculateCost(electricityReadings, plan));
  }
}
