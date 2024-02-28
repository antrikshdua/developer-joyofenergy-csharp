namespace JOIEnergy.Tests;

using Domain;
using System;
using System.Collections.Generic;
using Xunit;
using static Enums.Supplier;

public class PricePlanTest
{
  private readonly PricePlan _pricePlan = new(TheGreenEco, 20m,
    new List<PeakTimeMultiplier>
    {
      new(DayOfWeek.Saturday, 2m), new(DayOfWeek.Sunday, 10m)
    });

  [Fact]
  public void TestGetEnergySupplier()
  {
    Assert.Equal(TheGreenEco, _pricePlan.EnergySupplier);
  }

  [Fact]
  public void TestGetBasePrice()
  {
    Assert.Equal(20m, _pricePlan.GetPrice(new DateTime(2018, 1, 2)));
  }

  [Fact]
  public void TestGetPeakTimePrice()
  {
    Assert.Equal(40m, _pricePlan.GetPrice(new DateTime(2018, 1, 6)));
  }
}
