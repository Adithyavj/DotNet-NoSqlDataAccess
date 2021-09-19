using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLibrary
{
    public class MongoDBDataAccess
    {
        private IMongoDatabase _db;

        public MongoDBDataAccess(string dbName, string connectionString)
        {
            var client = new MongoClient(connectionString);
            _db = client.GetDatabase(dbName);
        }

        // Inserting an object to the db
        public void InsertRecord<T>(string table, T record)
        {
            // establish a connection to the table
            var collection = _db.GetCollection<T>(table);
            collection.InsertOne(record);
        }

        // Get All
        public List<T> LoadRecords<T>(string table)
        {
            var collection = _db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList(); // return all the records
        }

        // GetById
        public T LoadRecordById<T>(string table, Guid id)
        {
            var collection = _db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            return collection.Find(filter).First();
        }

        // Create/update record
        [Obsolete]
        public void UpsertRecord<T>(string table, Guid id, T record)
        {
            var collection = _db.GetCollection<T>(table);

            var result = collection.ReplaceOne(
                new BsonDocument("_id", id),
                record,
                new UpdateOptions { IsUpsert = true }
                );
        }

        // Delete
        public void DeleteRecord<T>(string table, Guid id)
        {
            var collection = _db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            collection.DeleteOne(filter);
        }
    }
}
