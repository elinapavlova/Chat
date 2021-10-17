using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Filter;
using Microsoft.EntityFrameworkCore;
using Models.Base;

namespace Infrastructure.Repository.Base
{
    public abstract class BaseRepository<TModel, TFilter>
        where TModel : BaseModel
        where TFilter : BaseFilter
    {
        private readonly AppDbContext _context;

        protected BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        private IQueryable<TModel> GetDataSet()
            => _context.Set<TModel>().AsNoTracking();
        

        public async Task<TModel> GetById(int id)
        {
            var data = await GetDataSet().FirstOrDefaultAsync(u => u.Id == id);
            return data;
        }

        public async Task<TModel> Create(TModel data)
        {
            await _context.Set<TModel>().AddAsync(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<ICollection<TModel>> GetFiltered(TFilter filter)
        {
            var result = GetDataSet();

            result = ApplySort(result, filter.Sort);
            result = ApplyPaging(result, filter.Paging);

            return await result.ToListAsync();
        }

        private IQueryable<TModel> ApplySort(IQueryable<TModel> source, FilterSortDto sort)
        {
            sort ??= new FilterSortDto
            {
                ColumnName = nameof(BaseModel.DateCreated),
                IsDescending = true
            };

            return sort.ColumnName switch
            {
                nameof(BaseModel.DateCreated) => sort.IsDescending
                    ? source.OrderByDescending(p => p.DateCreated)
                    : source.OrderBy(p => p.DateCreated),
                
                nameof(BaseModel.Id) => sort.IsDescending
                    ? source.OrderByDescending(p => p.Id)
                    : source.OrderBy(p => p.Id),
                _ => source
            };
        }

        private IQueryable<TModel> ApplyPaging(IQueryable<TModel> source, FilterPagingDto paging)
        {
            return source
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize);
        }
    }
}