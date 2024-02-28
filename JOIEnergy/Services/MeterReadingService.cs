namespace JOIEnergy.Services;

using Domain;

public sealed class MeterReadingService : IMeterReadingService
{
  private readonly Dictionary<string, List<ElectricityReading>>
    _meterAssociatedReadings;

  public MeterReadingService(
    Dictionary<string, List<ElectricityReading>> meterAssociatedReadings)
  {
    _meterAssociatedReadings = meterAssociatedReadings;
  }

  public IReadOnlyList<ElectricityReading> GetReadings(string smartMeterId)
  {
    return _meterAssociatedReadings.TryGetValue(
      smartMeterId,
      out List<ElectricityReading> readings)
      ? readings
      : ArraySegment<ElectricityReading>.Empty;
  }

  public void StoreReadings(
    string smartMeterId,
    IReadOnlyList<ElectricityReading> electricityReadings)
  {
    if (_meterAssociatedReadings
        .TryGetValue(smartMeterId, out List<ElectricityReading> reading))
    {
      reading.AddRange(electricityReadings);
    }
    else
    {
      _meterAssociatedReadings.Add(smartMeterId, [..electricityReadings]);
    }
  }
}
