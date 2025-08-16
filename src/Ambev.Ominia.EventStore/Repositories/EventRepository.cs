using Ambev.Ominia.Domain.Events;
using Ambev.Ominia.Domain.Common;
using Ambev.Ominia.Domain.Interfaces.Infrastructure;
using Ambev.Ominia.EventStore.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Ambev.Ominia.Domain.Events.Sales;
using Ambev.Ominia.Domain.Entities.Sales;
using Ambev.Ominia.Domain.Entities.Products;
using Ambev.Ominia.Domain.ValueObjects;

namespace Ambev.Ominia.EventStore.Repositories;

public class EventRepository : IEventRepository
{
    private readonly IMongoCollection<EventModel> _eventStoreCollection;

    public EventRepository(IOptions<MongoDbConfig> config)
    {
        // Configure GUID serializer for MongoDB
        try
        {
            if (!BsonSerializer.IsTypeDiscriminated(typeof(Guid)))
            {
                BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            }
        }
        catch (BsonSerializationException)
        {
            // Serializer already registered, ignore
        }
        
        // Register event types for MongoDB discriminator
        ConfigureEventSerialization();
        
        var mongoClient = new MongoClient(config.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);

        _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection);
    }
    
    private static void ConfigureEventSerialization()
    {
        try
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(BaseEvent)))
            {
                BsonClassMap.RegisterClassMap<BaseEvent>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIsRootClass(true);
                    cm.AddKnownType(typeof(SaleCreatedEvent));
                    cm.AddKnownType(typeof(SaleModifiedEvent));
                    cm.AddKnownType(typeof(SaleCancelledEvent));
                    cm.AddKnownType(typeof(ItemCancelledEvent));
                });
            }
            
            // Register SaleItem for proper serialization
            if (!BsonClassMap.IsClassMapRegistered(typeof(SaleItem)))
            {
                BsonClassMap.RegisterClassMap<SaleItem>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
            
            // Register Product for proper serialization
            if (!BsonClassMap.IsClassMapRegistered(typeof(Product)))
            {
                BsonClassMap.RegisterClassMap<Product>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
            
            // Register Rating for proper serialization
            if (!BsonClassMap.IsClassMapRegistered(typeof(Rating)))
            {
                BsonClassMap.RegisterClassMap<Rating>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(SaleCreatedEvent)))
            {
                BsonClassMap.RegisterClassMap<SaleCreatedEvent>(cm =>
                {
                    cm.AutoMap();
                    cm.MapMember(e => e.SaleId);
                    cm.MapMember(e => e.SaleNumber);
                    cm.MapMember(e => e.Date);
                    cm.MapMember(e => e.Customer);
                    cm.MapMember(e => e.Branch);
                    cm.MapMember(e => e.Items);
                    cm.SetIgnoreExtraElements(true);
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(SaleModifiedEvent)))
            {
                BsonClassMap.RegisterClassMap<SaleModifiedEvent>(cm =>
                {
                    cm.AutoMap();
                    cm.MapMember(e => e.SaleNumber);
                    cm.MapMember(e => e.Date);
                    cm.MapMember(e => e.Customer);
                    cm.MapMember(e => e.Branch);
                    cm.MapMember(e => e.Items);
                    cm.SetIgnoreExtraElements(true);
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(SaleCancelledEvent)))
            {
                BsonClassMap.RegisterClassMap<SaleCancelledEvent>();
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(ItemCancelledEvent)))
            {
                BsonClassMap.RegisterClassMap<ItemCancelledEvent>();
            }
        }
        catch (BsonSerializationException)
        {
            // Class maps already registered, ignore
        }
    }

    public async Task<List<IEventModel>> FindAllAsync()
    {
        var events = await _eventStoreCollection.Find(_ => true).ToListAsync().ConfigureAwait(false);
        return events.Cast<IEventModel>().ToList();
    }

    public async Task<List<IEventModel>> FindByAggregateIdAsync(Guid aggregateId)
    {
        var events = await _eventStoreCollection.Find(x => x.AggregateIdentifier == aggregateId).ToListAsync().ConfigureAwait(false);
        return events.Cast<IEventModel>().ToList();
    }

    public async Task SaveAsync(IEventModel @event)
    {
        var eventModel = new EventModel
        {
            Id = ObjectId.GenerateNewId().ToString(),
            TimeStamp = DateTime.UtcNow,
            AggregateIdentifier = @event.AggregateIdentifier,
            AggregateType = @event.AggregateType,
            Version = @event.Version,
            EventType = @event.EventType,
            EventData = @event.EventData
        };
        
        await _eventStoreCollection.InsertOneAsync(eventModel).ConfigureAwait(false);
    }
}