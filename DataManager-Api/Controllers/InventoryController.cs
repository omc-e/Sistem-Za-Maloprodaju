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
        
            [Authorize(Roles = "Manager,Admin")]
            public List<InventoryModel> Get()
            {
                InventoryData data = new InventoryData();

                return data.GetInventory();
            }

            [Authorize(Roles = "Admin")]
            public void Post(InventoryModel item)
            {
                InventoryData data = new InventoryData();
                data.SaveInventoryRecord(item);
            }
        }
    }

