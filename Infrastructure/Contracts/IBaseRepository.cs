using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Filter;
using Models.Base;

namespace Infrastructure.Contracts
{
    public interface IBaseRepository<TModel, in TFilter> 
        where TModel : BaseModel
        where TFilter : BaseFilter
    {
        Task<TModel> Create(TModel data);
        Task<TModel> GetById(int id);
        Task<ICollection<TModel>> GetFiltered(TFilter filter);
    }
}