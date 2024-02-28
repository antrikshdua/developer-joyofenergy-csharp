namespace JOIEnergy.Services;

using Domain;

public interface IMeterReadingService
{
  IReadOnlyList<ElectricityReading> GetReadings(string smartMeterId);

  void StoreReadings(
    string smartMeterId,
    IReadOnlyList<ElectricityReading> electricityReadings);
}
