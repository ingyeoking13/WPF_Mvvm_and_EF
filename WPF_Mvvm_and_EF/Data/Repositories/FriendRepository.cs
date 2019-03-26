using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WPF_Mvvm_and_EF.DataAccess;
using WPF_Mvvm_and_EF.Model;

namespace WPF_Mvvm_and_EF.Data.Repositories
{

    public class FriendRepository : GenericRepository<Friend, FriendOrganizerDbContext>, 
                                    IFriendRepository
    {
        public FriendRepository(FriendOrganizerDbContext context) : base(context)
        {
        }

        public override async Task<Friend> GetByIdAsync(int? friendId)
        {
            return await context.Friends
                .Include(f=>f.PhoneNumbers)
                .SingleAsync(f => (f.Id == friendId));
        }

        public async Task<bool> HasMeetingsAsync(int friendId)
        {
            return await context.
                Meetings.AsNoTracking().
                Include(m => m.Friends).
                AnyAsync(m => m.Friends.Any(f => f.Id == friendId));
        }

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            context.FriendPhoneNumbers.Remove(model);
        }
    }
}
