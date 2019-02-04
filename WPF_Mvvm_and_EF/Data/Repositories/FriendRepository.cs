using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using WPF_Mvvm_and_EF.DataAccess;
using WPF_Mvvm_and_EF.Model;

namespace WPF_Mvvm_and_EF.Data.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private FriendOrganizerDbContext _contextCreator;
        public FriendRepository(FriendOrganizerDbContext contextCreateor)
        {
            _contextCreator = contextCreateor;
        }

        public void Add(Friend friend)
        {
            _contextCreator.Friends.Add(friend);
        }

        public void Delete(Friend friend)
        {
            _contextCreator.Friends.Remove(friend);
        }

        public async Task<Friend> GetByIdAsync(int? friendId)
        {
            return await _contextCreator.Friends.SingleAsync(f => (f.Id == friendId));
        }

        public bool HasChanges()
        {
            return _contextCreator.ChangeTracker.HasChanges();
        }

        public async Task SaveAsync()
        {
            await _contextCreator.SaveChangesAsync();
        }
    }
}
