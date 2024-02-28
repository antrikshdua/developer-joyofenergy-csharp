namespace JOIEnergy.Generator;

using Domain;

public class ElectricityReadingGenerator
{
  private static readonly Random Random = new();

  public static List<ElectricityReading> Generate(int number)
  {
    return Enumerable
      .Range(1, number)
      .Select(i => new ElectricityReading(
        (decimal)Random.NextDouble(),
        DateTime.Now.AddSeconds(-i * 10)))
      .OrderBy(v => v.Time)
      .ToList();
  }
}
