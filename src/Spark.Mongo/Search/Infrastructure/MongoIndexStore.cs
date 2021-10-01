/*
 * Copyright (c) 2020-2021, Incendi (info@incendi.no) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/FirelyTeam/spark/stu3/master/LICENSE
 */

using MongoDB.Bson;
using MongoDB.Driver;
using Spark.Engine.Core;
using System.Threading.Tasks;
using Spark.Store.Mongo;
using Spark.Engine.Model;
using Spark.Mongo.Search.Indexer;
using Spark.Engine.Store.Interfaces;

namespace Spark.Mongo.Search.Common
{
    public class MongoIndexStore : IIndexStore, IAsyncIndexStore
    {
        private readonly IMongoDatabase _database;
        private readonly MongoIndexMapper _indexMapper;
        public readonly IMongoCollection<BsonDocument> Collection;

        public MongoIndexStore(string mongoUrl, MongoIndexMapper indexMapper)
        {
            _database = MongoDatabaseFactory.GetMongoDatabase(mongoUrl);
            _indexMapper = indexMapper; 
            Collection = _database.GetCollection<BsonDocument>(Config.MONGOINDEXCOLLECTION);
        }

        public void Save(IndexValue indexValue)
        {
            var result = _indexMapper.MapEntry(indexValue);

            foreach (var doc in result)
            {
                Save(doc);
            }
        }

        public async Task SaveAsync(IndexValue indexValue)
        {
            var result = _indexMapper.MapEntry(indexValue);

            foreach (var doc in result)
            {
                await SaveAsync(doc).ConfigureAwait(false);
            }
        }

        public void Save(BsonDocument document)
        {
            string keyvalue = document.GetValue(InternalField.ID).ToString();
            var query = Builders<BsonDocument>.Filter.Eq(InternalField.ID, keyvalue);
            Collection.ReplaceOne(query, document, new ReplaceOptions { IsUpsert = true });
        }

        public async Task SaveAsync(BsonDocument document)
        {
            string keyvalue = document.GetValue(InternalField.ID).ToString();
            var query = Builders<BsonDocument>.Filter.Eq(InternalField.ID, keyvalue);
            await Collection.ReplaceOneAsync(query, document, new ReplaceOptions { IsUpsert = true }).ConfigureAwait(false);
        }

        public void Delete(Entry entry)
        {
            string id = entry.Key.WithoutVersion().ToOperationPath();
            var query = Builders<BsonDocument>.Filter.Eq(InternalField.ID, id);
            Collection.DeleteMany(query);
        }
        public async Task DeleteAsync(Entry entry)
        {
            string id = entry.Key.WithoutVersion().ToOperationPath();
            var query = Builders<BsonDocument>.Filter.Eq(InternalField.ID, id);
            await Collection.DeleteManyAsync(query).ConfigureAwait(false);
        }

        public void Clean()
        {
            Collection.DeleteMany(Builders<BsonDocument>.Filter.Empty);
        }

        public async Task CleanAsync()
        {
            await Collection.DeleteManyAsync(Builders<BsonDocument>.Filter.Empty).ConfigureAwait(false);
        }
    }
}
