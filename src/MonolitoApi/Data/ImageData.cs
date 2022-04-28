using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MonolitoApi.Models;
using MonolitoApi.Settings;

namespace MonolitoApi.Data
{
    public class ImageData
    {
        private readonly IMongoCollection<FileImage> _imageColletion;

        public ImageData(IOptions<FileImageStorageData> imageDatabaseSettings)
        {
            var mongoClient = new MongoClient(imageDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(imageDatabaseSettings.Value.DatabaseName);
            var collections = mongoDatabase.ListCollectionNames().ToList();
            if (collections == null || !collections.Any() || !collections.Contains(imageDatabaseSettings.Value.ImageCollectionName))
            {
                mongoDatabase.CreateCollectionAsync(imageDatabaseSettings.Value.ImageCollectionName, new CreateCollectionOptions<FileImage>());
            }
            _imageColletion = mongoDatabase.GetCollection<FileImage>(imageDatabaseSettings.Value.ImageCollectionName);
        }

        public async Task<List<FileImage>> GetAsync() =>
            await _imageColletion.Find(_ => true).ToListAsync();

        public async Task<FileImage?> GetAsync(string id) =>
            await _imageColletion.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<FileImage?> GetLastIdAsync() =>
            await _imageColletion.Find(_ => true).FirstOrDefaultAsync();

        public async Task CreateAsync(FileImage fileImage) =>
            await _imageColletion.InsertOneAsync(fileImage);

        public async Task UpdateAsync(string id, FileImage fileImage) =>
            await _imageColletion.ReplaceOneAsync(x => x.Id == id, fileImage);

        public async Task RemoveAsync(string id) =>
            await _imageColletion.DeleteOneAsync(x => x.Id == id);
    }
}