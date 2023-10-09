using LightInject;
using LightInject.Microsoft.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using Wishlist.Api.Services;
using Wishlist.Api.Settings;
using Wishlist.DAL;

using Telegram.Bot;

using Wishlist.Api;

var builder = WebApplication
    .CreateBuilder(args);

builder.Host.UseLightInject(sr => sr.RegisterFrom<CompositionRoot>());

builder.Configuration.AddJsonFile("secret.json");

var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.ConfigurationName);
var messageTextsConfigurationSection = builder.Configuration.GetSection(MessageTextsConfiguration.ConfigurationName);
builder.Services.Configure<BotConfiguration>(botConfigurationSection);
builder.Services.Configure<MessageTextsConfiguration>(messageTextsConfigurationSection);


builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
    {
        var botConfig = sp.GetRequiredService<IOptions<BotConfiguration>>().Value;
        TelegramBotClientOptions options = new(botConfig.BotToken);
        return new TelegramBotClient(options, httpClient);
    });

builder
    .Services
    .AddControllers()
    .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<ConfigureWebhook>();
builder.Services.AddScoped<UpdateHandlers>();

// RegisterMessageHandlers(builder.Services);
// RegisterCallbackQueryHandlers(builder.Services);


builder
    .Services
    .AddDbContextPool<WishlistDbContext>(optionsBuilder =>
        optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    MigrateDatabase();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.MapControllers();

app.Run();
return;



void MigrateDatabase()
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<WishlistDbContext>();
    context.Database.Migrate();
}