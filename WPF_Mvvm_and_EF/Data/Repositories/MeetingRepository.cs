using System.Threading.Tasks;
using WPF_Mvvm_and_EF.DataAccess;
using WPF_Mvvm_and_EF.Model;
using System.Data.Entity;
using System.Collections.Generic;

namespace WPF_Mvvm_and_EF.Data.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, FriendOrganizerDbContext>, 
        IMeetingRepository
    {
        public MeetingRepository(FriendOrganizerDbContext context) : base(context)
        {
        }

        public override async Task<Meeting> GetByIdAsync(int? dataId)
        {
            return await context.Meetings
                .Include(m => m.Friends)
                .SingleAsync(m=>m.Id==dataId);
        }

        public async Task<List<Friend>> GetAllFriendsAsync()
        {
            return await context.Set<Friend>().ToListAsync();
        }
    }
}
