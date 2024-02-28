namespace JOIEnergy.Domain;

public sealed record ElectricityReading(
  decimal Reading,
  DateTime Time);
