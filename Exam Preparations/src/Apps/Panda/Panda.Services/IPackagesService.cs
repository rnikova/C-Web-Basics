﻿using Panda.Data.Models;
using System.Linq;

namespace Panda.Services
{
    public interface IPackagesService
    {
        void Create(string description, decimal weight, string shippingAddress, string recipientName);

        IQueryable<Package> GetAllByStatus(PackageStatus status);

        void Deliver(string id);
    }
}
