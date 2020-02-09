﻿using System;
using System.Collections.Generic;

namespace Musaca.Models
{
    public class Order
    {
        public Order()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Products = new HashSet<Product>();
        }

        public string Id { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime IssuedOn  { get; set; }

        public ICollection<Product> Products { get; set; }

        public string CashierId { get; set; }

        public User Cashier { get; set; }
    }
}
