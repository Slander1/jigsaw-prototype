using CoreLogic.Configs.Categories;
using CoreLogic.Configs.PiecesCount;
using Cysharp.Threading.Tasks;

namespace CoreLogic.Configuration
{
    public interface IConfigStorage
    {
        public CategoriesList CategoriesList { get; }
        public PiecesDifficultLevels PiecesDifficultLvl { get;  }
        public UniTask LoadCategoriesConfig();
        public UniTask LoadDifficultConfig();
    }
}