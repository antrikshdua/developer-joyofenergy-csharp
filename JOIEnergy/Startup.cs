namespace JOIEnergy;

using Domain;
using Enums;
using Generator;
using Services;
using static Enums.Supplier;

public sealed class Startup
{
  private readonly IConfiguration _configuration;

  public Startup(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public void ConfigureServices(IServiceCollection services)
  {
    Dictionary<string, List<ElectricityReading>> readings =
      GenerateMeterElectricityReadings();

    List<PricePlan> pricePlans =
    [
      new PricePlan(DrEvilsDarkEnergy, 10m, new List<PeakTimeMultiplier>()),
      new PricePlan(TheGreenEco, 2m, new List<PeakTimeMultiplier>()),
      new PricePlan(PowerForEveryone, 1m, new List<PeakTimeMultiplier>())
    ];

    services.AddMvc(options => options.EnableEndpointRouting = false);
    services.AddTransient<IAccountService, AccountService>();
    services.AddTransient<IMeterReadingService, MeterReadingService>();
    services.AddTransient<IPricePlanService, PricePlanService>();
    services.AddSingleton(readings);
    services.AddSingleton(pricePlans);
    services.AddSingleton(_smartMeterToPricePlanAccounts);
  }

  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    if (env.IsDevelopment())
    {
      app.UseDeveloperExceptionPage();
    }

    app.UseMvc();
  }

  private Dictionary<string, List<ElectricityReading>>
    GenerateMeterElectricityReadings()
  {
    ElectricityReadingGenerator generator = new();

    IEnumerable<string> smartMeterIds =
      _smartMeterToPricePlanAccounts.Select(v => v.Key);

    return smartMeterIds.ToDictionary(id => id,
      _ => ElectricityReadingGenerator.Generate(20));
  }

  private readonly Dictionary<String, Supplier> _smartMeterToPricePlanAccounts =
    new()
    {
      { "smart-meter-0", DrEvilsDarkEnergy },
      { "smart-meter-1", TheGreenEco },
      { "smart-meter-2", DrEvilsDarkEnergy },
      { "smart-meter-3", PowerForEveryone },
      { "smart-meter-4", TheGreenEco }
    };
}
