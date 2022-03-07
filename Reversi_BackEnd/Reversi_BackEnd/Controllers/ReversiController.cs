using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reversi_CL.Data.ReversiDbContext;
using Reversi_CL.Helpers;
using Reversi_CL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Reversi_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReversiController : ControllerBase
    {
        private readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions 
            { 
                Converters = 
                {
                    new KleurMultiArrayJsonConverter(),
                    new SpelerJsonConverter()
                } 
            };
        
        private readonly ReversiDbContext _context;

        public ReversiController(ReversiDbContext context)
        {
            _context = context;
            
        }

        private Spel GetSpelByToken(string spelToken)
        {
            return  _context.Spellen
                .Include(spel => spel.Spelers)
                .Where(spel => spel.ID == spelToken)
                .FirstOrDefault();
        }

        // api/Spel/<Token> - GET
        // - Omschrijving
        // - Tokens van het spel en van de spelers
        // - bord en elk vakje de bezetting
        // - Wie er aan de beurt is
        // - Status van het spel (bijv. De winnaar, opgeven door)
        [HttpGet("/Api/Spel/{spelToken}")]
        public IActionResult GetSpel([FromRoute] string spelToken)
        {
            Spel spel = GetSpelByToken(spelToken);

            if (spel == null)
            {
                return NotFound();
            }

            string json = JsonSerializer.Serialize(spel, serializerOptions);
            
            return Ok(json);
        }

        [HttpPost("/Api/Spel/Pass")]
        public IActionResult SlaBeurtOver([FromBody] SlaBeurtOverDTO dto)
        {
            Spel spel = GetSpelByToken(dto.SpelId);

            //Spel bestaat niet
            if (spel == null) return NotFound();

            //Speler niet in spel
            if (!spel.Spelers.Any(speler => speler.Id == dto.SpelerId))
            {
                return BadRequest();
            }

            spel.Pas();
            _context.Update(spel);
            _context.SaveChanges();

            return GetSpel(dto.SpelId);
        }


        public class SlaBeurtOverDTO
        {
            public string SpelerId { get; set; }
            public string SpelId { get; set; }
        }


        // Api/Spel/Beurt - GET
        // Opvragen wie er aan de beurt is.
        [HttpGet("/Api/Spel/Beurt")]
        public IActionResult GetAandeBeurt([FromRoute] string spelToken)
        {
            
            Spel spel = GetSpelByToken(spelToken);

            if (spel == null)
            {
                return BadRequest();
            }


            return Ok(spel.AandeBeurt);
        }

        [HttpPost("/Api/Spel/ResetBord")]
        public IActionResult ResetBord([FromQuery] string spelToken)
        {
            Spel spel = GetSpelByToken(spelToken);

            spel.ResetBord();

            _context.Spellen.Update(spel);
            _context.SaveChanges();

            return Ok();
        }

        //Api/Spel/Zet - PUT
        // Stuurt het veld naar de server waar een fiche wordt geplaatst. Het token van
        // het spel en speler moeten meegegeven worden. Ook passen moet mogelijk zijn.
        [HttpPut("/Api/Spel/Zet")]
        public IActionResult MaakZet([FromBody] MaakZetDTO dto)
        {
            Spel spel = GetSpelByToken(dto.SpelToken);

            if (spel.DoeZet(dto.RijZet, dto.KolomZet))
            {
                _context.Spellen.Update(spel);
                _context.SaveChanges();

                return GetSpel(dto.SpelToken);
            }

            if (spel.SpelIsAfgelopen)
            {
                _context.Spellen.Update(spel);
                _context.SaveChanges();
                return GetSpel(dto.SpelToken);
            }

            return BadRequest("Onmogelijke Zet");
        }

        public class MaakZetDTO
        {
            public int RijZet { get; set; }
            public int KolomZet { get; set; }
            public string SpelToken { get; set; }
            public string SpelerToken { get; set; }
        }

        //ToDo
        // Api/Spel/Opgeven - PUT
        // Hiermee geeft de speler op
        [HttpPut("Api/Spel/Opgeven")]
        public async Task<ActionResult> GeefOp([FromBody] GeefOpDTO dto)
        {
            Spel spel = GetSpelByToken(dto.SpelId);
            spel.SpelIsAfgelopen = true;

            _context.Update(spel);
            await _context.SaveChangesAsync();
            
            return Ok();
        }

        public class GeefOpDTO
        {
            public string SpelId { get; set; }
            public string spelerId { get; set; }
        }
    }
}