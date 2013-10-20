using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AttiLA.Data.Entities
{

    [BsonIgnoreExtraElements]
    [BsonKnownTypes(
        typeof(OpeningAction), 
        typeof(ClosingAction), 
        typeof(ServiceAction), 
        typeof(NotificationAction))]
    public class Action
    {

    }
}
