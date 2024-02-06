using EscolaParaDevs.Entities;
using EscolaParaDevs.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EscolaParaDevs.Services
{
    public interface ICourseService
    {
        public Task<Course> Create(Course course);
        public Task<Course> GetById(int id);
        public Task<List<Course>> GetAll();
        public Task Update(Course courseIn, int id);
        public Task Delete(int id);
    }

    public class CourseService : ICourseService
    {
        private readonly DataContext _context;

        public CourseService(DataContext context)
        {
            _context = context;
        }

        public async Task<Course> Create(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return course;

            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
            Course courseDb = await _context.Courses.SingleOrDefaultAsync(x => x.Id == id);

            if (courseDb is null)
                throw new Exception($"Course {id} not found");

            _context.Courses.Remove(courseDb);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Course>> GetAll() => await _context.Courses.ToListAsync();      

        public async Task<Course> GetById(int id)
        {
            Course coursesDb = await _context.Courses.SingleOrDefaultAsync(x => x.Id == id);

            if (coursesDb is null)
                throw new Exception($"Note {id} not found");

            return coursesDb;
        }

        public async Task Update(Course courseIn, int id)
        {
            if (courseIn.Id != id)
                throw new Exception("Route id is differs Note id");

            Course courseDb = await _context.Courses
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (courseDb is null)
                throw new Exception($"Note {id} not found");

            courseIn.CreatedAt = courseDb.CreatedAt;

            _context.Entry(courseIn).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
