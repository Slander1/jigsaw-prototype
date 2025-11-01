using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CoreLogic.Loader
{
    public class ResourcesLoader : ILoader
    {
        private const string PreviewFolderName = "Previews";
        private const string FullResFolderName = "FullImage";
        private const string PuzzleImagesRootPath = "PuzzleImages";
        
        public async UniTask<T> LoadScriptableConfigAsync<T>(string path) where T : UnityEngine.Object
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));

            var request = Resources.LoadAsync<T>(path);
            await request;

            if (request.asset is not T asset || asset == null)
                throw new NullReferenceException($"[ResourcesLoader] Не удалось загрузить {typeof(T).Name} по пути: {path}");

            return asset;
        }

        public async UniTask<Sprite> LoadSpriteAsync(bool isPreview, string categoryName, int id)
        {
            var path = Path.Combine(PuzzleImagesRootPath, 
                isPreview ? PreviewFolderName : FullResFolderName, categoryName, id.ToString());
            
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be null or empty.", nameof(path));

            var spriteRequest = Resources.LoadAsync<Sprite>(path);
            await spriteRequest;

            if (spriteRequest.asset is Sprite sprite && sprite != null)
                return sprite;

            var textureRequest = Resources.LoadAsync<Texture2D>(path);
            await textureRequest;

            if (textureRequest.asset is not Texture2D tex || tex == null)
                throw new NullReferenceException($"[ResourcesLoader] Не удалось загрузить Sprite или Texture2D по пути: {path}");

            var createdSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100f);
            return createdSprite;
        }
    }
}