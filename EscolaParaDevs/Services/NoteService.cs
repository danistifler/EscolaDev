using EscolaParaDevs.Entities;
using EscolaParaDevs.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EscolaParaDevs.Services
{
    public interface INoteService
    {
        public Task<Note> Create(Note note);
        public Task<Note> GetById(int id);
        public Task<List<Note>> GetAll();
        public Task Update(Note noteIn, int id);
        public Task Delete(int id);
    }

    public class NoteService : INoteService
    {
        private readonly DataContext _context;

        public NoteService(DataContext context)
        {
            _context = context;
        }

        public async Task<Note> Create(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return note;

            throw new NotImplementedException();
        }

        public async Task Delete(int id)
        {
            Note noteDb = await _context.Notes.SingleOrDefaultAsync(x => x.Id == id);

            if (noteDb is null)
                throw new Exception($"Note {id} not found");

            _context.Notes.Remove(noteDb);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Note>> GetAll() => await _context.Notes.ToListAsync();      

        public async Task<Note> GetById(int id)
        {
            Note noteDb = await _context.Notes.SingleOrDefaultAsync(x => x.Id == id);

            if (noteDb is null)
                throw new Exception($"Note {id} not found");

            return noteDb;
        }

        public async Task Update(Note noteIn, int id)
        {
            if (noteIn.Id != id)
                throw new Exception("Route id is differs Note id");

            Note noteDb = await _context.Notes
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id);

            if (noteDb is null)
                throw new Exception($"Note {id} not found");

            noteIn.CreatedAt = noteDb.CreatedAt;

            _context.Entry(noteIn).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
