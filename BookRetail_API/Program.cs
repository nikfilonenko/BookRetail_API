using BookRetail_API.Data;
using BookRetail_API.GraphQL.Schemas;
using EasyNetQ;
using GraphQL;
using GraphQL.Server;
using GraphQL.Types;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<IBookRetailStorage, BookCsvFileStorage>();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// Add GraphQL
builder.Services.AddScoped<ISchema, BookSchema>();
builder.Services.AddGraphQL(options => 
    { options.EnableMetrics = true; })
    .AddSystemTextJson();

// Add RabbitMQ support
var bus = RabbitHutch.CreateBus(builder.Configuration.GetConnectionString("AutoRabbitMQ"));
builder.Services.AddSingleton<IBus>(bus);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
    app.UseGraphQLAltair();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseGraphQL<ISchema>();

app.MapControllers();

app.Run();