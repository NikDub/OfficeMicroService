﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OfficeMicroService.Data.Models
{
    public interface IBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
