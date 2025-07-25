﻿using Rampage.Core.Entities;
using Rampage.DAL.Context;
using Rampage.DAL.Repositories.Interfaces;

namespace Rampage.DAL.Repositories.Implementation
{
    public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
