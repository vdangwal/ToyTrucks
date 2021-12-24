﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToyTrucks.Catalog.Api.DbContexts;
using ToyTrucks.Catalog.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToyTrucks.Catalog.Api.Services
{
    public class TruckRepository : ITruckRepository
    {
        private readonly CatalogDbContext _context;
        private readonly ILogger<TruckRepository> _logger;

        public TruckRepository(CatalogDbContext context, ILogger<TruckRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        async Task<bool> TruckExists(Guid truckId)
        {
            return await _context.Trucks.AnyAsync(t => t.TruckId == truckId);
        }

        public async Task AddTruck(Truck truck)
        {
            if (truck == null)
            {
                _logger.LogError("Truck to add is null");
                throw new ArgumentNullException(nameof(truck));
            }
            await _context.Trucks.AddAsync(truck);
        }

        public async Task<Truck> GetTruckById(Guid truckId)
        {
            if (truckId == Guid.Empty)
            {
                _logger.LogError("Truckid to query is null");
                throw new ArgumentException(nameof(truckId));
            }
            return await _context.Trucks.Where(t => t.OutOfStock == false)
                                        .Include(t => t.Categories)
                                        .Include(t => t.Photos)
                                        .AsSplitQuery()
                                        .FirstOrDefaultAsync(t => t.TruckId == truckId);
        }

        public async Task<Truck> GetTruckByName(string truckName)
        {
            if (string.IsNullOrEmpty(truckName))
            {
                _logger.LogError("Truck name to query is null");
                throw new ArgumentException(nameof(truckName));
            }
            return await _context.Trucks.Where(t => t.OutOfStock == false)
                                        .Include(t => t.Categories)
                                        .Include(t => t.Photos)
                                        .AsSplitQuery()
                                        .FirstOrDefaultAsync(t => t.Name == truckName);
        }

        public async Task<IEnumerable<Truck>> GetTrucks()
        {
            return await _context.Trucks.Where(t => t.OutOfStock == false)
                                        .Include(t => t.Categories)
                                        .Include(t => t.Photos)
                                        .AsSplitQuery()
                                        .ToListAsync();
        }

        public async Task<IEnumerable<Truck>> GetTrucksByCategoryId(int categoryId)
        {
            return await _context.Trucks.Where(t => t.Categories.Any(c => c.CategoryId == categoryId))
                                        .Where(t => t.OutOfStock == false)
                                        .Include(t => t.Categories)
                                        .Include(t => t.Photos)
                                        .AsSplitQuery()
                                        .ToListAsync();
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }

        public async Task<bool> UpdateTruck(Truck truck)
        {
            if (truck == null)
            {
                _logger.LogError("Truck to update is null");
                throw new ArgumentNullException(nameof(truck));
            }

            var truckToUpdate = await _context.Trucks.SingleOrDefaultAsync(t => t.TruckId == truck.TruckId);
            if (truckToUpdate == null)
            {
                _logger.LogError("Truck to update doesnt exist in the db");
                throw new ArgumentNullException(nameof(truck));
            }
            truckToUpdate.Damaged = truck.Damaged;
            truckToUpdate.DefaultPhotoPath = truck.DefaultPhotoPath;
            truckToUpdate.Description = truck.Description;
            truckToUpdate.Hidden = truck.Damaged;
            truckToUpdate.Name = truck.Name;
            truckToUpdate.PreviousPrice = truck.PreviousPrice;
            truckToUpdate.Price = truck.Price;
            // truckToUpdate.Quantity = truck.Quantity;
            truckToUpdate.Year = truck.Year;

            return await SaveChanges();
        }

        public async Task<bool> UpdateTruckInventory(TruckInventory truckInventory)
        {
            if (truckInventory == null)
            {
                _logger.LogError("Truck to update is null");
                throw new ArgumentNullException(nameof(truckInventory));
            }

            var truckToUpdate = await _context.Trucks.SingleOrDefaultAsync(t => t.TruckId == truckInventory.TruckId);
            if (truckToUpdate == null)
            {
                _logger.LogError("Truck to update doesn't exist in the db");
                throw new ArgumentNullException(nameof(truckInventory));
            }

            if (truckToUpdate.Quantity - truckInventory.Quantity < 0)
            {
                truckToUpdate.Quantity = 0;
                // throw new exception should not be less than zero but ill ignore for now.
            }
            else
            {
                truckToUpdate.Quantity -= truckInventory.Quantity;
            }
            truckToUpdate.OutOfStock = truckToUpdate.Quantity == 0;
            return await SaveChanges();
        }

        public async Task<TruckInventory> GetTruckInventory(Guid truckId)
        {
            if (truckId == Guid.Empty)
            {
                _logger.LogError("Truckid to query is null");
                throw new ArgumentException(nameof(truckId));
            }

            var truck = await _context.Trucks.FirstOrDefaultAsync(t => t.TruckId == truckId);
            if (truck == null)
            {
                _logger.LogError("Truck in basket doesn't exist");
                throw new ArgumentNullException(nameof(truckId));
            }
            return new TruckInventory
            {
                TruckId = truck.TruckId,
                Quantity = truck.Quantity
            };
        }
    }
}
