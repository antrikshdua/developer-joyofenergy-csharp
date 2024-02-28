namespace JOIEnergy.Tests;

using Controllers;
using Domain;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Enums.Supplier;

public class PricePlanComparisonTest
{
  private readonly MeterReadingService _meterReadingService;
  private readonly PricePlanComparatorController _controller;

  private const String SmartMeterId = "smart-meter-id";

  public PricePlanComparisonTest()
  {
    Dictionary<string, List<ElectricityReading>> readings = new();
    _meterReadingService = new MeterReadingService(readings);

    List<PricePlan> pricePlans =
    [
      new PricePlan(DrEvilsDarkEnergy, 10, NoMultipliers()),
      new PricePlan(TheGreenEco, 2, NoMultipliers()),
      new PricePlan(PowerForEveryone, 1, NoMultipliers())
    ];

    PricePlanService pricePlanService = new(pricePlans, _meterReadingService);

    _controller = new PricePlanComparatorController(pricePlanService);
  }

  [Fact]
  public void ShouldCalculateCostForMeterReadingsForEveryPricePlan()
  {
    ElectricityReading electricityReading =
      new(15.0m, DateTime.Now.AddHours(-1));
    ElectricityReading otherReading =
      new(5.0m, DateTime.Now);
    _meterReadingService.StoreReadings(SmartMeterId,
      new List<ElectricityReading> { electricityReading, otherReading });

    Dictionary<string, decimal> result =
      _controller.CalculatedCostForEachPricePlan(SmartMeterId).Value as
        Dictionary<string, decimal>;

    Assert.NotNull(result);
    Assert.Equal(3, result.Count);
    Assert.Equal(100m, result[DrEvilsDarkEnergy.ToString()], 3);
    Assert.Equal(20m, result[TheGreenEco.ToString()], 3);
    Assert.Equal(10m, result[PowerForEveryone.ToString()], 3);
  }

  [Fact]
  public void ShouldRecommendCheapestPricePlansNoLimitForMeterUsage()
  {
    _meterReadingService.StoreReadings(SmartMeterId,
      new List<ElectricityReading>
      {
        new(35m, DateTime.Now.AddMinutes(-30)), new(3m, DateTime.Now)
      });

    object result =
      _controller.RecommendCheapestPricePlans(SmartMeterId).Value;
    List<KeyValuePair<string, decimal>> recommendations =
      ((IEnumerable<KeyValuePair<string, decimal>>)result ??
       Array.Empty<KeyValuePair<string, decimal>>()).ToList();

    Assert.Equal("" + PowerForEveryone, recommendations[0].Key);
    Assert.Equal("" + TheGreenEco, recommendations[1].Key);
    Assert.Equal("" + DrEvilsDarkEnergy, recommendations[2].Key);
    Assert.Equal(38m, recommendations[0].Value, 3);
    Assert.Equal(76m, recommendations[1].Value, 3);
    Assert.Equal(380m, recommendations[2].Value, 3);
    Assert.Equal(3, recommendations.Count);
  }

  [Fact]
  public void ShouldRecommendLimitedCheapestPricePlansForMeterUsage()
  {
    _meterReadingService.StoreReadings(SmartMeterId,
      new List<ElectricityReading>
      {
        new(5m, DateTime.Now.AddMinutes(-45)), new(20m, DateTime.Now)
      });

    object result = _controller.RecommendCheapestPricePlans(SmartMeterId, 2)
      .Value;
    List<KeyValuePair<string, decimal>> recommendations =
      ((IEnumerable<KeyValuePair<string, decimal>>)result ??
       Array.Empty<KeyValuePair<string, decimal>>()).ToList();

    Assert.Equal("" + PowerForEveryone, recommendations[0].Key);
    Assert.Equal("" + TheGreenEco, recommendations[1].Key);
    Assert.Equal(16.667m, recommendations[0].Value, 3);
    Assert.Equal(33.333m, recommendations[1].Value, 3);
    Assert.Equal(2, recommendations.Count);
  }

  [Fact]
  public void
    ShouldRecommendCheapestPricePlansMoreThanLimitAvailableForMeterUsage()
  {
    _meterReadingService.StoreReadings(SmartMeterId,
      new List<ElectricityReading>
      {
        new(35m, DateTime.Now.AddMinutes(-30)), new(3m, DateTime.Now)
      });

    object result = _controller
      .RecommendCheapestPricePlans(SmartMeterId, 5).Value;

    List<KeyValuePair<string, decimal>> recommendations =
      ((IEnumerable<KeyValuePair<string, decimal>>)result ??
       Array.Empty<KeyValuePair<string, decimal>>()).ToList();

    Assert.Equal(3, recommendations.Count);
  }

  [Fact]
  public void GivenNoMatchingMeterIdShouldReturnNotFound()
  {
    Assert.Equal(404,
      _controller.CalculatedCostForEachPricePlan("not-found").StatusCode);
  }

  private static List<PeakTimeMultiplier> NoMultipliers()
  {
    return [];
  }
}
