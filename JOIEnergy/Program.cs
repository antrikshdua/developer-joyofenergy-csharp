using JOIEnergy;
using Microsoft.AspNetCore;

WebHost.CreateDefaultBuilder(args)
  .UseStartup<Startup>()
  .Build()
  .Run();
