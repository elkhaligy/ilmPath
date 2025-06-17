using IlmPath.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlmPath.Application.Carts.DTOs.Response
{
    public class CartResponse
    {
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal TotalPrice => Items.Sum(item => item.Price);
    }
}
