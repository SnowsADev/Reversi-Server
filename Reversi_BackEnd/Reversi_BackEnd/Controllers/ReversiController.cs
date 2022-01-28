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
                    new KleurMultiArrayJsonConverter()
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
                .FirstOrDefault(x => x.ID == spelToken);
        }

        // api/Spel/<Token> - GET
        // - Omschrijving
        // - Tokens van het spel en van de spelers
        // - bord en elk vakje de bezetting
        // - Wie er aan de beurt is
        // - Status van het spel (bijv. De winnaar, opgeven door)
        [HttpGet("/Api/Spel")]
        public IActionResult GetSpel([FromQuery] string spelToken)
        {
            Spel spel = GetSpelByToken(spelToken);

            if (spel == null)
            {
                return NotFound();
            }

            string json = JsonSerializer.Serialize(spel, serializerOptions);
            
            return Ok(json);
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

        //Api/Spel/Zet - PUT
        // Stuurt het veld naar de server waar een fiche wordt geplaatst. Het token van
        // het spel en speler moeten meegegeven worden. Ook passen moet mogelijk zijn.
        [HttpPut("/Api/Spel/Zet")]
        public IActionResult MaakZet([FromBody] int rijZet, int kolomZet, string spelToken, string spelerToken)
        {
            Spel spel = GetSpelByToken(spelToken);

            if (spel.DoeZet(rijZet, kolomZet))
            {
                return GetSpel(spelToken);
            }

            return BadRequest("Onmogelijke Zet");
        }

        //ToDo
        // Api/Spel/Opgeven - PUT
        // Hiermee geeft de speler op
        [HttpPut("Api/Spel/Opgeven")]
        public ActionResult GeefOp([FromBody] string spelToken, string spelerToken)
        {
            var tempVariable1 = spelToken;
            var tempVariable2 = spelerToken;
            return Ok();
        }
    }
}