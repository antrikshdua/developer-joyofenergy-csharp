namespace JOIEnergy.Services;

using Enums;
using SupplierMap = Dictionary<string, Enums.Supplier>;

public sealed class AccountService : IAccountService
{
  private readonly SupplierMap _smartMeterToPricePlanAccounts;

  public AccountService(SupplierMap smartMeterToPricePlanAccounts)
  {
    _smartMeterToPricePlanAccounts = smartMeterToPricePlanAccounts;
  }

  public Supplier GetPricePlanIdForSmartMeterId(string smartMeterId)
  {
    return _smartMeterToPricePlanAccounts
      .GetValueOrDefault(smartMeterId, Supplier.NullSupplier);
  }
}
