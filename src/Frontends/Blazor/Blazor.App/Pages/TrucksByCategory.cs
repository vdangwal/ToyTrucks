﻿using Blazor.App.Components;
using Blazor.App.Models;
using Blazor.App.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.App.Pages
{
    public partial class TrucksByCategory
    {
        public IEnumerable<TruckDto> Trucks { get; set; }
        
        [Parameter]
        public string CategoryId { get; set; }
        public string Message { get; set; }
        [Inject]
        public ITruckService TruckService { get; set; }
 

        public TruckDetail TruckDetailComponent { get; set; }

       

        protected async Task ShowTruckDetails(Guid truckId)
        {
          await  TruckDetailComponent.ShowDetails(truckId);
        }

        protected override async Task OnParametersSetAsync()
        {
            Message = null;
            Trucks = await TruckService.GetTrucksByCategoryId(int.Parse(CategoryId));

            if (Trucks == null || !Trucks.Any())
            {// Trucks = await FakeData();
                Message = "No trucks returned";
            }

        }

        async Task<IEnumerable<TruckDto>> FakeData()
        { 
         return await  Task.Run<List<TruckDto>>(()=> { 
              return   new List<TruckDto> {
                    new TruckDto { Name = "T1", Description = " t1 desc" },
                    new TruckDto { Name = "T2", Description = " t2 desc" },
                    new TruckDto { Name = "T3", Description = " t3 desc" },
                };
            });
        } 
    }
}
