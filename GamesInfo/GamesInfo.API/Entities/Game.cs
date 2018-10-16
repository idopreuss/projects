using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GamesInfo.API.Entities
{
    public class Game 
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [Key]
        public string Key { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string Sport { get; set; }
        public string League { get; set; }

        [Required]
        [MaxLength(100)]
        public string Home { get; set; }
        [Required]
        [MaxLength(100)]
        public string Away { get; set; }



    }
}
