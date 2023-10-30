using GlobalErrorApp.Data;
using GlobalErrorApp.IServices;
using GlobalErrorApp.Models;

namespace GlobalErrorApp.Services
{
    public class DriverService : IDriverService
    {
        private readonly AppDbContext _context;
        public DriverService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Driver> AddDriver(Driver driver)
        {
            var result = await _context.Drivers.AddAsync(driver);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<bool> DeleteDriver(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if(driver != null)
            {
                _context.Drivers.Remove(driver);
                await _context.SaveChangesAsync();
            }
            return driver != null;
        }

        public async Task<Driver?> GetDriverById(int id)
        {
            return await _context.Drivers.FindAsync(id);
        }

        public Task<IEnumerable<Driver>> GetDrivers()
        {
            throw new NotImplementedException();
        }

        public async Task<Driver> UpdateDriver(Driver driver)
        {
            var result = _context.Drivers.Update(driver);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}
