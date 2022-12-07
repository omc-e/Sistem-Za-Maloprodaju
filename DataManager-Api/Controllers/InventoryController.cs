using DataManager.Library.DataAccess;
using DataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DataManager_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
       
        private readonly IInventoryData _inventoryData;

        public InventoryController( IInventoryData inventoryData)
        {
            _inventoryData = inventoryData;
        }
        
            [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
            public List<InventoryModel> Get() { 
       

                return _inventoryData.GetInventory();
            }

            [Authorize(Roles = "Admin")]
        [HttpPost]
            public void Post(InventoryModel item)
            {
              
            _inventoryData.SaveInventoryRecord(item);
            }
        }
    }

