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
        /// <summary>
        /// Get a list of tasks in alphabetic orders. Only the main fields are reported.
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public IEnumerable<Task> GetTaskDetails(int limit, int skip)
        {
            var tasksCursor = this.MongoConnectionHandler.MongoCollection.FindAllAs<Task>()
                .SetSortOrder(SortBy<Task>.Descending(t => t.TaskName))
                .SetLimit(limit)
                .SetSkip(skip)
                .SetFields(Fields<Task>.Include(t => t.Id, t => t.TaskName, t => t.CreationDateTime));
            return tasksCursor;
        }

        /// <summary>
        /// Get the action list of the task.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public IEnumerable<AttiLA.Data.Entities.Action> GetActions(string taskId)
        {
            var entityQuery = Query<Task>.EQ(t => t.Id, new ObjectId(taskId));
            return this.MongoConnectionHandler.MongoCollection.FindOneById(new ObjectId(taskId)).Actions;
        }


        /// <summary>
        /// Put a contexts in the list of contexts using the task.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="contextId"></param>
        public void AddContext(string taskId, string contextId)
        {
            var taskObjectId = new ObjectId(taskId);
            var contextObjectId = new ObjectId(contextId);

            WriteConcernResult updateResult;
            try
            {
                updateResult = this.MongoConnectionHandler.MongoCollection.Update(
                    Query<Task>.EQ(t => t.Id, taskObjectId),
                    Update<Task>.Push(t => t.Contexts, contextObjectId),
                    new MongoUpdateOptions
                    {
                        WriteConcern = WriteConcern.Acknowledged
                    });
            }
            catch (MongoException ex)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorUpdate + taskId, ex);
            }

            if (updateResult.DocumentsAffected == 0)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorUpdate + taskId);
            }
        }

        public void RemoveContext(string taskId, string contextId)
        {
            var taskObjectId = new ObjectId(taskId);
            var contextObjectId = new ObjectId(contextId);

            WriteConcernResult updateResult;
            try
            {
                updateResult = this.MongoConnectionHandler.MongoCollection.Update(
                    Query<Task>.EQ(t => t.Id, taskObjectId),
                    Update<Task>.Pull(t => t.Contexts, contextObjectId),
                    new MongoUpdateOptions
                    {
                        WriteConcern = WriteConcern.Acknowledged
                    });
            }
            catch (MongoException ex)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorUpdate + taskId, ex);
            }

            if (updateResult.DocumentsAffected == 0)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorUpdate + taskId);
            }
        }


        public void AddAction(string taskId, AttiLA.Data.Entities.Action action)
        {
            var taskObjectId = new ObjectId(taskId);

            WriteConcernResult updateResult;
            try
            {
                updateResult = this.MongoConnectionHandler.MongoCollection.Update(
                    Query<Task>.EQ(t => t.Id, taskObjectId),
                    Update<Task>.Push(t => t.Actions, action),
                    new MongoUpdateOptions
                    {
                        WriteConcern = WriteConcern.Acknowledged
                    });
            }
            catch (MongoException ex)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorUpdate + taskId, ex);
            }

            if (updateResult.DocumentsAffected == 0)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorUpdate + taskId);
            }
        }

        public void RemoveAction(string taskId, AttiLA.Data.Entities.Action action)
        {
            var taskObjectId = new ObjectId(taskId);

            WriteConcernResult updateResult;
            try
            {
                updateResult = this.MongoConnectionHandler.MongoCollection.Update(
                    Query<Task>.EQ(t => t.Id, taskObjectId),
                    Update<Task>.Pull(t => t.Actions, action),
                    new MongoUpdateOptions
                    {
                        WriteConcern = WriteConcern.Acknowledged
                    });
            }
            catch (MongoException ex)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorUpdate + taskId, ex);
            }

            if (updateResult.DocumentsAffected == 0)
            {
                throw new DatabaseException(Properties.Resources.MsgErrorUpdate + taskId);
            }
        }

        public override void Update(Task entity)
        {
        } 


    }
}
