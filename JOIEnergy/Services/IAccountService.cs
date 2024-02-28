namespace JOIEnergy.Services;

using Enums;

public interface IAccountService
{
  Supplier GetPricePlanIdForSmartMeterId(string smartMeterId);
}
