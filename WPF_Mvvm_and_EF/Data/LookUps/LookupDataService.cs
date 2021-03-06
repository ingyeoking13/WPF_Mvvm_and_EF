﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Mvvm_and_EF.DataAccess;
using WPF_Mvvm_and_EF.Model;

namespace WPF_Mvvm_and_EF.Data.LookUps
{
    public class LookupDataService : 
        IFriendLookupDataService, 
        IProgramminLanguageLookupDataService, 
        IMeetingLookupDataService
    {
        private Func<FriendOrganizerDbContext> _contextCreator;

        public LookupDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }
        public async Task<IEnumerable<LookupItem>> GetFriendLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Friends.AsNoTracking().
                    Select(f =>
                    new LookupItem
                    {
                        Id = f.Id,
                        DisplayMember = f.FirstName + " " + f.LastName
                    }).ToListAsync();
            };
        }

        public async Task<IEnumerable<LookupItem>> GetProgrammingLanguageLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.ProgrammingLanguages.AsNoTracking().
                    Select(f =>
                    new LookupItem
                    {
                        Id = f.Id,
                        DisplayMember = f.Name
                    }).ToListAsync();
            };
        }

        public async Task<List<LookupItem>> GetMeetingLookupAsync()
        {
            using (var ctx = _contextCreator())
            {
                var items = await ctx.Meetings.AsNoTracking().Select(
                    m => new LookupItem
                    {
                        Id = m.Id,
                        DisplayMember = m.Title
                    }).ToListAsync();
                return items;
            }
        }
    }
}
