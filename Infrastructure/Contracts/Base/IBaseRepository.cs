using System.Threading.Tasks;
using Models.Base;

namespace Infrastructure.Contracts.Base
{
    public interface IBaseRepository<TModel> 
        where TModel : BaseModel
    {
        Task<TModel> Create(TModel data);
        Task<TModel> GetById(int id);
    }
}