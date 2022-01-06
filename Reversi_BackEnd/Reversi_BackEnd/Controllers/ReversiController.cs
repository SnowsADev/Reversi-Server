using Microsoft.AspNetCore.Mvc;
using Reversi_CL.Helpers;
using Reversi_CL.Models;
using System.Text.Json;

namespace Reversi_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReversiController : ControllerBase
    {
        private Spel Spel { get; set; }
        private readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions 
            { 
                Converters = 
                {
                    new KleurMultiArrayJsonConverter()
                } 
            };
        

        public ReversiController()
        {
            this.Spel = new Spel();
        }

        // api/Spel/<Token> - GET
        // - Omschrijving
        // - Tokens van het spel en van de spelers
        // - bord en elk vakje de bezetting
        // - Wie er aan de beurt is
        // - Status van het spel (bijv. De winnaar, opgeven door)
        [HttpGet("/Api/Spel")]
        public ActionResult GetSpel()
        {
            //var tempVariable = spelToken;
            string json = JsonSerializer.Serialize(Spel, serializerOptions);
            return Ok(json);
        }

        // Api/Spel/Beurt - GET
        // Opvragen wie er aan de beurt is.

        [HttpGet("/Api/Spel/Beurt")]
        public ActionResult<Kleur> GetAandeBeurt([FromRoute] string spelToken)
        {
            var tempVariable = spelToken;
            return Ok((int)Spel.AandeBeurt);
        }

        //Api/Spel/Zet - PUT
        // Stuurt het veld naar de server waar een fiche wordt geplaatst. Het token van
        // het spel en speler moeten meegegeven worden. Ook passen moet mogelijk zijn.
        [HttpPut("/Api/Spel/Zet")]
        public ActionResult MaakZet([FromBody] int rijZet, int kolomZet, string spelToken, string spelerToken)
        {
            var tempVariable1 = spelToken;
            var tempVariable2 = spelerToken;
            if (Spel.DoeZet(rijZet, kolomZet))
            {
                return GetSpel();
            }
            return Ok("Onmogelijke Zet");
        }


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