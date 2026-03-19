using Geometrix.WebApi.Modules.AppModules;
using TheAppManager.Startup;

AppManager.Start(args, modules =>
{
    // Module order matters: it determines the order of ConfigureServices,
    // ConfigureMiddleware, and ConfigureEndpoints calls.
    //
    // Services-only modules are listed first to match the original
    // registration order. Modules with middleware are ordered to
    // reproduce the original middleware pipeline exactly.
    modules
        // --- Services registration (order matches original Program.cs) ---
        .Add<ServiceDefaultsModule>()       // builder.AddServiceDefaults()
        .Add<FeatureFlagsModule>()           // AddFeatureFlags (must be first)
        .Add<LoggingModule>()                // AddInvalidRequestLogging
        .Add<VersioningModule>()             // AddVersioning
        .Add<UseCasesModule>()              // AddUseCases
        .Add<ControllersModule>()           // AddCustomControllers
        .Add<DataProtectionModule>()        // AddCustomDataProtection
        // --- Modules with both services and middleware ---
        // Middleware order: ExceptionHandler > StaticFiles > Proxy > HealthChecks
        //   > Cors > HttpMetrics > Routing > Swagger > RateLimiter > Auth > Endpoints
        .Add<MiddlewarePipelineModule>()    // error handling, redirect, static files
        .Add<ProxyModule>()                 // AddProxy + UseProxy
        .Add<HealthChecksModule>()          // AddHealthChecks + UseHealthChecks
        .Add<CorsModule>()                  // AddCustomCors + UseCustomCors
        .Add<MetricsModule>()              // UseCustomHttpMetrics + MapMetrics
        .Add<RoutingModule>()              // UseRouting + MapControllers
        .Add<SwaggerModule>()             // AddSwagger + UseVersionedSwagger
        .Add<RateLimitingModule>()        // AddCustomRateLimiting + UseRateLimiter
        .Add<AuthenticationModule>();     // AddAuthentication + UseAuth + UseAuthz
});
