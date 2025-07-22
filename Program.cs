using Chatbot.Application.Repositories;
using Chatbot.Domain.Hubs;
using Chatbot.Infrastructure.Data;
using Chatbot.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using OpenAI;
using System.ClientModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
    
builder.Services.AddControllers();

builder.Services
    .AddKernel()
    .AddOpenAIChatCompletion(
        modelId: "google/gemini-2.0-flash-001",
        openAIClient: new OpenAIClient(
            credential: new ApiKeyCredential("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"),
            options: new OpenAIClientOptions
            {
                Endpoint = new Uri("https://openrouter.ai/api/v1")
            })
    );

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization(); // Rol bazlý kontrol istersen

var app = builder.Build();

app.UseAuthentication(); // Bu sýraya dikkat
app.UseAuthorization();

app.MapControllers();

app.Run();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PromptDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPromptLogRepository, PromptLogRepository>();
builder.Services.AddScoped<IAIService, AIService>();

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(_ => true)
            .AllowCredentials();
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll"); 
app.UseAuthorization();

app.MapControllers();

app.MapHub<AIHub>("/ai-hub");

//app.MapPost("/chat", async (AIService aiService, [FromBody] ChatRequestVM chatRequest, CancellationToken cancellationToken)
//    => await aiService.GetMessageStreamAsync(chatRequest.Prompt, chatRequest.ConnectionId, cancellationToken));

app.Run();
