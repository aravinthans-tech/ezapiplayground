using QRCodeAPI.Middleware;
using QRCodeAPI.Services;
using QRCodeAPI.Swagger;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Add HttpClientFactory for external API calls with timeout configuration
builder.Services.AddHttpClient("OpenRouter", client =>
{
    client.Timeout = TimeSpan.FromSeconds(180); // 3 minutes for LLM API calls
});

builder.Services.AddHttpClient("Unstract", client =>
{
    client.Timeout = TimeSpan.FromSeconds(300); // 5 minutes for OCR processing
});

// Default HttpClient for other services
builder.Services.AddHttpClient();

// Register QrCodeService
builder.Services.AddScoped<QrCodeService>();

// Register FileSummaryService
builder.Services.AddScoped<FileSummaryService>();

// Register KycAgentService
builder.Services.AddScoped<KycAgentService>();

// Register KYC Verification Services
builder.Services.AddScoped<DocumentProcessingService>();
builder.Services.AddScoped<AddressVerificationService>();
builder.Services.AddScoped<ConsistencyCheckService>();

// Register AWS Rekognition service first (required by FaceMatchingService)
builder.Services.AddScoped<AwsRekognitionMatchingService>();

// Register FaceMatchingService (wrapper that uses AWS Rekognition)
builder.Services.AddScoped<FaceMatchingService>();

builder.Services.AddScoped<KycVerificationService>();

// Configure CORS for React frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000", 
                "http://localhost:3001",
                "https://ezplaygroundapp.vercel.app",
                "https://*.vercel.app"  // Allow all Vercel preview deployments
              )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "QRCode API",
        Version = "v1",
        Description = "API for QR Code Generation, File Summary, and KYC Verification"
    });
    
    // Add API Key authentication to Swagger
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key authentication using X-API-Key header. Get your API key from /api/Client/apiKey endpoint.",
        Name = "X-API-Key",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    
    // Ignore Obsolete APIs
    options.IgnoreObsoleteActions();
    options.IgnoreObsoleteProperties();
    
    // Resolve conflicting actions
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    
    // Custom schema IDs to avoid conflicts
    options.CustomSchemaIds(type => type.FullName);
    
    // Configure to handle form parameters correctly
    // Note: IFormFile and Microsoft.AspNetCore.Http.IFormFile are the same type, so only map once
    options.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });
    
    // Add filters to fix duplicate ContentType issue with multiple IFormFile parameters
    options.ParameterFilter<FormFileParameterFilter>();
    options.OperationFilter<FixFormFileContentTypeFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseCors("AllowReactApp");

// Enable Swagger UI - MUST be before API key middleware
app.UseSwagger(c =>
{
    c.RouteTemplate = "swagger/{documentName}/swagger.json";
});

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "QRCode API v1");
    c.RoutePrefix = "swagger";
    c.DisplayRequestDuration();
});

// Add API Key middleware (after Swagger so Swagger endpoints are accessible)
app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
