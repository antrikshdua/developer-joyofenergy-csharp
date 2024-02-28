namespace JOIEnergy.Domain;

using Enums;

public sealed record PricePlan(
  Supplier EnergySupplier,
  decimal UnitRate,
  IReadOnlyList<PeakTimeMultiplier> PeakTimeMultiplier)
{
  public decimal GetPrice(DateTime datetime)
  {
    PeakTimeMultiplier multiplier = PeakTimeMultiplier
      .FirstOrDefault(m => m.DayOfWeek == datetime.DayOfWeek);

    if (multiplier?.Multiplier != null)
    {
      return multiplier.Multiplier * UnitRate;
    }

    return UnitRate;
  }
}

public sealed record PeakTimeMultiplier(
  DayOfWeek DayOfWeek,
  decimal Multiplier);
