using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AttiLA.Data.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace AttiLA.Data.Services
{
    public class TaskService : EntityService<Task>
    {
        public override void Update(Task entity)
        {
            throw new NotImplementedException();
        } 
    }
}
