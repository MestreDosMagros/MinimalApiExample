using Domain;
using Infra;
using Microsoft.EntityFrameworkCore;

namespace Application
{
    public class MyEntityService : IMyEntityService
    {
        private readonly Context _context;

        public MyEntityService(Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MyEntity>> Add(MyEntity entity)
        {
            await _context.MyEntities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return await GetAll();
        }

        public async Task<IEnumerable<MyEntity>> Delete(Guid id)
        {
            var entity = await _context.MyEntities.FindAsync(id);
            _context.MyEntities.Remove(entity);
            await _context.SaveChangesAsync();
            return await GetAll();
        }

        public async Task<IEnumerable<MyEntity>> GetAll()
        {
            return await _context.MyEntities.ToListAsync();
        }

        public async Task<IEnumerable<MyEntity>> Update(MyEntity entity)
        {
            _context.MyEntities.Update(entity);
            await _context.SaveChangesAsync();
            return await GetAll();
        }
    }
}
