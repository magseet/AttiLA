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
        /// <summary>
        /// View Model
        /// </summary>
        [BsonIgnore]
        public String Type
        {
            get
            {

                if (GetType() == typeof(OpeningAction))
                {
                    return "Open";
                }
                else if (GetType() == typeof(ClosingAction))
                {
                    return "Close";
                }
                else if (GetType() == typeof(ServiceAction))
                {
                    return "Service";
                }
                else if (GetType() == typeof(NotificationAction))
                {
                    return "Notification";
                }
                else
                {
                    return "Other";
                }
            }
        }


        /// <summary>
        /// View Model
        /// </summary>
        [BsonIgnore]
        public String Summary { 
            get { 
                return ToString(); 
            } 
        }

    }
}
