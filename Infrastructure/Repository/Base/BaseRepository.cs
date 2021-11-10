using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database;
using Infrastructure.Filter;
using Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Models.Base;

namespace Infrastructure.Repository.Base
{
    public abstract class BaseRepository<TModel, TFilter>
        where TModel : BaseModel
        where TFilter : BaseFilter
    {
        private readonly AppDbContext _context;
        private readonly PagingOptions _pagingOptions;

        protected BaseRepository
        (
            AppDbContext context,
            PagingOptions pagingOptions
        )
        {
            _context = context;
            _pagingOptions = pagingOptions;
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

        protected async Task<ICollection<TModel>> GetFilteredSource(IQueryable<TModel> source, TFilter filter)
        {
            source = ApplySort(source, filter.Sort);
            var result = ApplyPaging(source, filter.Paging);

            return await result.ToListAsync();
        }
        
        protected async Task<ICollection<TModel>> GetFilteredSource(TFilter filter)
        {
            var source = GetDataSet();
            ApplySort(source, filter.Sort);
            var result = ApplyPaging(source, filter.Paging);

            return await result.ToListAsync();
        }

        protected IQueryable<TModel> ApplySort(IQueryable<TModel> source, FilterSortDto sort)
        {
            sort ??= new FilterSortDto
            {
                ColumnName = nameof(BaseModel.DateCreated),
                IsDescending = true
            };
            sort.ColumnName ??= nameof(BaseModel.DateCreated);
            
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

        protected IQueryable<TModel> ApplyPaging(IQueryable<TModel> source, FilterPagingDto paging)
        {
            if (paging.PageSize < 1)
                paging.PageSize = _pagingOptions.DefaultPageSize;
            
            if (paging.PageNumber < 1)
                paging.PageNumber = _pagingOptions.DefaultPageNumber;
            
            return source
                .Skip((paging.PageNumber - 1) * paging.PageSize)
                .Take(paging.PageSize);
        }
    }
}