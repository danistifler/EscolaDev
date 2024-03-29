﻿using EscolaParaDevs.Entities;
using EscolaParaDevs.Services;
using Microsoft.AspNetCore.Mvc;

namespace EscolaParaDevs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _service;

        public NoteController(INoteService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Note note) => Ok(await _service.Create(note));

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAll());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _service.GetById(id));

        [HttpPut("{id}")]

        public async Task<IActionResult> Update([FromBody] Note noteIn, int id)
        {
            await _service.Update(noteIn, id);
            return NoContent();
        }
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
