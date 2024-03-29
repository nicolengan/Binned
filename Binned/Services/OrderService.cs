﻿using Binned.Model;
using Binned.Pages.Admin;
using Microsoft.EntityFrameworkCore;

namespace Binned.Services
{
    public class OrderService
    {
        private readonly MyDbContext _context;

        public OrderService(MyDbContext context)
        {
            _context = context;
        }
        public List<Order> GetAll()
        {
            return _context.Orders
                .Include(i => i.Products)
                .OrderByDescending(p => p.OrderDate)
                .ToList();
        }
        public Order? GetOrderById(string id)
        {
            Order? order = _context.Orders
                .Include(i => i.Products)
                .FirstOrDefault(x => x.OrderId.Equals(id));
            return order;
        }

        public List<Order> GetOrderByUserId(string userId)
        {
            return _context.Orders
                .Include(i => i.Products)
                .Where(item => item.UserId == userId)
                .OrderByDescending(p => p.OrderDate)
                .ToList();
        }
        public List<Order> FilterOrder(string userId, string status)
        {
            return _context.Orders
                .Include(i => i.Products)
                .Where(item => item.UserId == userId)
                .Where(s => s.Status == status)
                .OrderByDescending(p => p.OrderDate)
                .ToList();
        }
        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order); // update vs attatched update updates all even if u only change one code, attatched only updates that one column 
            _context.SaveChanges();
        }
        public void UpdateStatusById(string id, string status)
        {
            var current = _context.Orders.FirstOrDefault(item => item.OrderId == id);
            if (current != null)
            {
                current.Status = status;
                _context.SaveChanges();
            }

        }
        public List<Order> GetOrderByMonth(int month, int year)
        {
            //var year = DateTime.Now.Year;
            var monthList = _context.Orders
                .Include(i => i.Products)
                .Where(date => date.OrderDate.Year == year)
                .Where(date => date.OrderDate.Month == month)
                .ToList();

            return monthList;
        }
        public async Task<decimal> CalculateTotal(string id)
        {
            var current = _context.Orders
                .Include(item => item.Products)
                .FirstOrDefault(item => item.OrderId == id);
            var productList = current?.Products;
            decimal total = 0;
            if (productList != null)
            {
                foreach (var i in productList)
                {
                    total += i.ProductPrice;
                }
            }
            if (current != null)
            {
                current.Amount = total;
                await _context.SaveChangesAsync();
            }
            return total;
        }
    }
}