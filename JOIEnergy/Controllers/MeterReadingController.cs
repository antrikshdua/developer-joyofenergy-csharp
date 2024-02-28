namespace JOIEnergy.Controllers;

using Domain;
using Microsoft.AspNetCore.Mvc;
using Services;

[Route("readings")]
public sealed class MeterReadingController : Controller
{
  private readonly IMeterReadingService _meterReadingService;

  public MeterReadingController(IMeterReadingService meterReadingService)
  {
    _meterReadingService = meterReadingService;
  }

  [HttpPost("store")]
  public ObjectResult Post([FromBody] MeterReadings meterReadings)
  {
    if (!IsMeterReadingsValid(meterReadings))
    {
      return new BadRequestObjectResult("Internal Server Error");
    }

    _meterReadingService.StoreReadings(
      meterReadings.SmartMeterId,
      meterReadings.ElectricityReadings);

    return new OkObjectResult("{}");
  }

  private static bool IsMeterReadingsValid(MeterReadings meterReadings)
  {
    String smartMeterId = meterReadings.SmartMeterId;

    IReadOnlyList<ElectricityReading> electricityReadings =
      meterReadings.ElectricityReadings;

    return !string.IsNullOrEmpty(smartMeterId) &&
           electricityReadings != null &&
           electricityReadings.Count != 0;
  }

  [HttpGet("read/{smartMeterId}")]
  public ObjectResult GetReading(string smartMeterId)
  {
    IReadOnlyList<ElectricityReading> readings =
      _meterReadingService.GetReadings(smartMeterId);

    return new OkObjectResult(readings);
  }
}
