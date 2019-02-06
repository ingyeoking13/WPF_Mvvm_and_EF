using System.Data.Entity;
using System.Threading.Tasks;

namespace WPF_Mvvm_and_EF.Data.Repositories
{
    public class GenericRepository<TEntity, Context> : IGenericRepository<TEntity>
        where TEntity : class
        where Context : DbContext
    {
        protected readonly Context context;
        public GenericRepository(Context context)
        {
            this.context = context;
        }
        public void Add(TEntity data)
        {
            context.Set<TEntity>().Add(data);
        }

        public void Delete(TEntity data)
        {
            context.Set<TEntity>().Remove(data);
        }

        public virtual async Task<TEntity> GetByIdAsync(int? dataId)
        {
            return await context.Set<TEntity>().FindAsync(dataId);
        }

        public bool HasChanges()
        {
            return context.ChangeTracker.HasChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
