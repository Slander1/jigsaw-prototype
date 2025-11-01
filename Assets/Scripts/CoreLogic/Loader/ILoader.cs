using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CoreLogic.Loader
{
    public interface ILoader
    {
        public UniTask<T> LoadScriptableConfigAsync<T>(string path) where T : UnityEngine.Object;
        public UniTask<Sprite> LoadSpriteAsync(bool isPreview, string categoryName, int id);
    }
}