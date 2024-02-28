namespace JOIEnergy.Domain;

public sealed record MeterReadings(
  string SmartMeterId,
  IReadOnlyList<ElectricityReading> ElectricityReadings);
