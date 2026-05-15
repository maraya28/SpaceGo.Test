using Infrastructure.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Implementations
{
    public class SlotStore(SpaceGoDbContext dbContext) : ISlotStore
    {
        public async Task<Slot> Get()
        {
            var slot = await dbContext.Slots.AsNoTracking().FirstAsync();
            return slot;
        }

        public async Task Set(Slot slot)
        {
            dbContext.Update(slot);
            await dbContext.SaveChangesAsync();
        }
    }
}