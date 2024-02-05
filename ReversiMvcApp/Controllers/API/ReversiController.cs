using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ReversiMvcApp.Helpers;
using ReversiMvcApp.Interfaces;
using ReversiMvcApp.Models;
using ReversiMvcApp.SignalR;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReversiMvcApp.Controllers.API
{
    [EnableCors(PolicyName = "AllowAllOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReversiController : ControllerBase
    {
        private readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            Converters =
                {
                    new KleurMultiArrayJsonConverter(),
                    new SpelerJsonConverter(),
                    new MogelijkeZettenJsonConverter(),
                }
        };

        private readonly ISpelRepository _spelAccessLayer;
        private readonly IHubContext<SpelHub> _hubContext;

        public ReversiController(ISpelRepository spelAccessLayer, IHubContext<SpelHub> hubContext)
        {
            _spelAccessLayer = spelAccessLayer;
            _hubContext = hubContext;
        }


        // api/Spel/<Token> - GET
        // - Omschrijving
        // - Tokens van het spel en van de spelers
        // - bord en elk vakje de bezetting
        // - Wie er aan de beurt is
        // - Status van het spel (bijv. De winnaar, opgeven door)
        [HttpGet("/Api/Spel/{spelId}")]
        public IActionResult GetSpel([FromRoute] string spelId)
        {
            Spel spel = _spelAccessLayer.GetSpel(spelId);

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
            Spel spel = _spelAccessLayer.GetOnafgerondeSpel(dto.SpelId);

            //Spel bestaat niet
            if (spel == null) return NotFound();

            //Speler niet in spel
            if (!spel.Spelers.Any(speler => speler.Id == dto.SpelerId))
            {
                return BadRequest();
            }

            spel.Pas();
            _spelAccessLayer.EditSpel(spel);

            return GetSpel(dto.SpelId);
        }


        public class SlaBeurtOverDTO
        {
            public string SpelerId { get; set; }
            public string SpelId { get; set; }
        }


        // Api/Spel/Beurt - GET
        // Opvragen wie er aan de beurt is.
        [HttpGet("/Api/Spel/{spelId}/Beurt/")]
        public IActionResult GetAandeBeurt([FromRoute] string spelId)
        {
            Spel spel = _spelAccessLayer.GetOnafgerondeSpel(spelId);

            if (spel == null)
            {
                return BadRequest();
            }

            return Ok(spel.AandeBeurt);
        }

        // Api/Spel/Zet - PUT
        // Stuurt het veld naar de server waar een fiche wordt geplaatst. Het token van
        // het spel en speler moeten meegegeven worden. Ook passen moet mogelijk zijn.
        [HttpPut("/Api/Spel/Zet")]
        public async Task<IActionResult> MaakZet([FromBody] MaakZetDTO dto)
        {
            Spel spel = _spelAccessLayer.GetOnafgerondeSpel(dto.SpelToken);
            Speler speler = spel.Spelers.FirstOrDefault(speler => speler.Id == dto.SpelerToken);

            if (spel == null || speler == null) return BadRequest("Speler of Spel niet gevonden");
            if (spel.AandeBeurt != speler.Kleur) return BadRequest("Speler is niet aan de beurt");

            if (spel.DoeZet(dto.RijZet, dto.KolomZet))
            {
                _spelAccessLayer.EditSpel(spel);
                await _hubContext.Clients.User(spel.Spelers.FirstOrDefault(s => s.Id != speler.Id).Id).SendAsync("ReceiveRefreshGameNotification");
                string json = JsonSerializer.Serialize(spel, serializerOptions);
                return Ok(json);
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

        // Api/Spel/Opgeven - PUT
        // Hiermee geeft de speler op
        [HttpPut("Api/Spel/Opgeven")]
        public ActionResult GeefOp([FromBody] GeefOpDTO dto)
        {
            Spel spel = _spelAccessLayer.GetOnafgerondeSpel(dto.SpelId);

            if (spel == null) return NotFound();

            spel.SpelIsAfgelopen = true;
            _spelAccessLayer.EditSpel(spel);

            return Ok();
        }

        public class GeefOpDTO
        {
            public string SpelId { get; set; }
            public string SpelerId { get; set; }
        }
    }
}