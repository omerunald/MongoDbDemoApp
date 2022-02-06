using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace MongoDbDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoCrud db = new MongoCrud("AdressBook");
            //PersonModel person = new PersonModel()
            //{
            //    FirstName = "Ali",
            //    LastName = "Ünaldı",
            //    PrimaryAddress = new AddressModel()
            //    {
            //        StreetAddress = "7.sokak",
            //        City = "İstanbul",
            //        State = "Esenyurt",
            //        ZipCode = "34500"
            //    }
            //};
            //db.InsertRecord("Users", person);
            //Console.WriteLine("ok");


            //var recs = db.LoadRecords<PersonModel>("Users");

            //foreach (var item in recs)
            //{
            //    Console.WriteLine($"{item.Id} : {item.FirstName} {item.LastName}");
            //    if (item.PrimaryAddress != null)
            //    {
            //        Console.WriteLine(item.PrimaryAddress.City);
            //    }
            //    Console.WriteLine(item.FirstName + "" + item.LastName);
            //}


            //var oneRec = db.LoadRecordById<PersonModel>("Users", new Guid("c92decd9-454b-4468-bbcc-397d353757e2"));
            //Console.WriteLine(oneRec.FirstName);

            //oneRec.DateOfBirth = new DateTime(1982, 10, 31, 0, 0, 0, DateTimeKind.Utc);
            //db.UpsertRecord("Users", oneRec.Id, oneRec);

            // db.DeleteRecord<PersonModel>("Users", oneRec.Id);


          

            Console.WriteLine("ok");
            Console.ReadLine();
        }
    }

  
    public class PersonModel
    {
        [BsonId]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AddressModel PrimaryAddress { get; set; }

        [BsonElement("dob")]
        public DateTime DateOfBirth { get; set; }
    }

    public class AddressModel
    {
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string ZipCode { get; set; }
    }


    public class MongoCrud
    {
        private IMongoDatabase db;

        public MongoCrud(string database)
        {
            var client = new MongoClient();
            db = client.GetDatabase(database);
        }
        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }
        public List<T> LoadRecords<T>(string table)
        {
            var collection = db.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }
        public T LoadRecordById<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);

            return collection.Find(filter).First();
        }

        public void UpsertRecord<T>(string table, Guid id, T record)
        {
            var collection = db.GetCollection<T>(table);
            var result = collection.ReplaceOne(
                new BsonDocument("_id", id),
                record,
                new UpdateOptions { IsUpsert = true });
        }

        public void DeleteRecord<T>(string table, Guid id)
        {
            var collection = db.GetCollection<T>(table);
            var filter = Builders<T>.Filter.Eq("Id", id);
            collection.DeleteOne(filter);
        }
    }
}
