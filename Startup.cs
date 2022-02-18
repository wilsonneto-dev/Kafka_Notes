builder.Services.AddScoped<IEventBus, KafkaEventBus>();
builder.Services.AddHostedService<Subscriber>();
