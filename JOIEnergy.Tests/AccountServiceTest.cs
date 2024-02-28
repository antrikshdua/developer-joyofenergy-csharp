namespace JOIEnergy.Tests;

using Enums;
using Services;
using System;
using System.Collections.Generic;
using Xunit;
using static Enums.Supplier;

public class AccountServiceTest
{
  private const String SmartMeterId = "smart-meter-id";
  private const Supplier PricePlanId = PowerForEveryone;

  private readonly AccountService _accountService;

  public AccountServiceTest()
  {
    Dictionary<String, Supplier> smartMeterToPricePlanAccounts =
      new() { { SmartMeterId, PricePlanId } };

    _accountService = new AccountService(smartMeterToPricePlanAccounts);
  }

  [Theory]
  [MemberData(nameof(Data))]
  public void Test(KeyValuePair<string, Supplier> pair)
  {
    (string value, Supplier supplier) = (pair.Key, pair.Value);

    Supplier result = _accountService.GetPricePlanIdForSmartMeterId(value);

    Assert.Equal(supplier, result);
  }

  public static readonly TheoryData<KeyValuePair<string, Supplier>> Data = new(
    new KeyValuePair<string, Supplier>("bob-carolgees", NullSupplier),
    new KeyValuePair<string, Supplier>("smart-meter-id", PowerForEveryone));
}
