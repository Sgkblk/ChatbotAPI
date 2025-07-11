using Chatbot.Application.Repositories;
using Chatbot.Infrastructure.Data;
using Chatbot.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Chatbot.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IAIService, SemanticKernelAIService>();

builder.Services.AddDbContext<PromptDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPromptLogRepository, PromptLogRepository>();



var app = builder.Build();

// HTTP request pipelineý
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
