﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using OfficeMicroService.Application.Extensions;

namespace OfficeMicroService.Data.Models
{
    [BsonCollection("Office")]
    public class Office : BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string PhotoId { get; set; }
        public string Status { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string RegistryPhoneNumber { get; set; }
    }
}
