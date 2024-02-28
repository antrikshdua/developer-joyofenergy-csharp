// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JOIEnergy.Controllers;

using Microsoft.AspNetCore.Mvc;
using Services;

[Route("price-plans")]
public sealed class PricePlanComparatorController : Controller
{
  private readonly IPricePlanService _pricePlanService;

  public PricePlanComparatorController(IPricePlanService pricePlanService)
  {
    _pricePlanService = pricePlanService;
  }

  [HttpGet("compare-all/{smartMeterId}")]
  public ObjectResult CalculatedCostForEachPricePlan(string smartMeterId)
  {
    Dictionary<string, decimal> costPerPricePlan = _pricePlanService
      .GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);

    return costPerPricePlan.Count != 0
      ? new ObjectResult(costPerPricePlan)
      : new NotFoundObjectResult($"Smart Meter ID ({smartMeterId}) not found");
  }

  [HttpGet("recommend/{smartMeterId}")]
  public ObjectResult RecommendCheapestPricePlans(
    string smartMeterId,
    int? limit = null)
  {
    Dictionary<string, decimal> consumptionForPricePlans = _pricePlanService
      .GetConsumptionCostOfElectricityReadingsForEachPricePlan(smartMeterId);

    if (consumptionForPricePlans.Count == 0)
    {
      return new NotFoundObjectResult(
        $"Smart Meter ID ({smartMeterId}) not found");
    }

    IReadOnlyList<KeyValuePair<string, decimal>> recommendations =
      consumptionForPricePlans.OrderBy(
          pricePlanComparison => pricePlanComparison.Value)
        .ToList();

    return limit.HasValue && limit.Value < recommendations.Count
      ? new ObjectResult(recommendations.Take(limit.Value))
      : new ObjectResult(recommendations);
  }
}
