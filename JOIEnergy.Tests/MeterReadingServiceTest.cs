namespace JOIEnergy.Tests;

using Domain;
using Services;
using System;
using System.Collections.Generic;
using Xunit;

public class MeterReadingServiceTest
{
  private const string SmartMeterId = "smart-meter-id";

  private readonly MeterReadingService _meterReadingService;

  public MeterReadingServiceTest()
  {
    _meterReadingService =
      new MeterReadingService(
        new Dictionary<string, List<ElectricityReading>>());

    _meterReadingService.StoreReadings(SmartMeterId,
      new List<ElectricityReading>
      {
        new(35m, DateTime.Now.AddMinutes(-30)),
        new(30m, DateTime.Now.AddMinutes(-15))
      });
  }

  [Fact]
  public void GivenMeterIdThatDoesNotExistShouldReturnNull()
  {
    Assert.Empty(_meterReadingService.GetReadings("unknown-id"));
  }

  [Fact]
  public void GivenMeterReadingThatExistsShouldReturnMeterReadings()
  {
    _meterReadingService.StoreReadings(SmartMeterId,
      new List<ElectricityReading> { new(25m, DateTime.Now) });

    IReadOnlyList<ElectricityReading> electricityReadings =
      _meterReadingService.GetReadings(SmartMeterId);

    Assert.Equal(3, electricityReadings.Count);
  }
}
