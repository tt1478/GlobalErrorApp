﻿using GlobalErrorApp.Exceptions;
using GlobalErrorApp.IServices;
using GlobalErrorApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlobalErrorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly ILogger<DriversController> _logger;
        public DriversController(IDriverService driverService, ILogger<DriversController> logger)
        {
            _driverService = driverService;
            _logger = logger;
        }
        [HttpGet("DriverList")]
        public async Task<IActionResult> DriverList()
        {
            _logger.LogInformation("Driver list action is executing");
            var result = await _driverService.GetDrivers();
            return Ok(result);
        }
        [HttpPost("AddDriver")]
        public async Task<IActionResult> AddDriver(Driver driver)
        {
            var result = await _driverService.AddDriver(driver);
            return CreatedAtAction(nameof(GetDriverById), new { id = result.Id }, result);
        }
        [HttpGet("GetDriverById")]
        public async Task<IActionResult> GetDriverById(int id)
        {
            var result = await _driverService.GetDriverById(id);
            if(result == null)
            {
                throw new NotFoundException("The id is an invalid id");
            }
            return Ok(result);
        }
    }
}
