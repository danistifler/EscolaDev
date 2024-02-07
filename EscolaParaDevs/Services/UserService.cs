using EscolaParaDevs.Entities;
using EscolaParaDevs.Exceptions;
using EscolaParaDevs.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using BC = BCrypt.Net.BCrypt;

namespace EscolaParaDevs.Services
{
    public interface IUserService
    {
        public Task<User> Create(User user);
        public Task<User> GetById(int id);
        public Task<List<User>> GetAll();
        public Task Update(User userIn,int id);
        public Task Delete(int id);


    }
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }
        public async Task<User> Create(User user)
        {
            if (!user.PassWord.Equals(user.ConfirmPassword))
                throw new BadRequestException("Password does not match ConfirmPassword");

            User userDb = await _context.Users
                .AsNoTracking()
               .SingleOrDefaultAsync(x => x.UserName == user.UserName);

            if(userDb is not null)             
                throw new BadRequestException($"UserName{user.UserName} already exist");

            user.PassWord = BC.HashPassword(user.PassWord);
                
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return user;            
        }

        public async Task Delete(int id)
        {
            User userDb = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);

            if (userDb is null)
                throw new KeyNotFoundException($"User {id} not found");

            _context.Users.Remove(userDb);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> GetAll() => await _context.Users.ToListAsync();
        
        public async Task<User> GetById(int id)
        {
            User userDb = await _context.Users.SingleOrDefaultAsync(x => x.Id == id);

            if (userDb is null)
                throw new KeyNotFoundException($"User {id} not found");

            return userDb;
        }

        public async Task Update(User userIn, int id)
        {
            if(userIn.Id != id)
                throw new BadRequestException("Route id is differs User id");

            else if (!userIn.PassWord.Equals(userIn.ConfirmPassword))
                throw new BadRequestException("Password does not match ConfirmPassword");

            User userDb = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (userDb is null)
                throw new KeyNotFoundException($"User {id} not found");
            else if (!BC.Verify(userIn.CurrentPassword, userDb.PassWord))
                throw new BadRequestException("Incorrect Password");

            userIn.CreatedAt = userDb.CreatedAt;
            userIn.PassWord = BC.HashPassword(userIn.PassWord);

            _context.Users.Update(userIn).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
