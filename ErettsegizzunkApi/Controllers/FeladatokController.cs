using ErettsegizzunkApi.DTOs;
using ErettsegizzunkApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace ErettsegizzunkApi.Controllers
{
    [Route("erettsegizzunk/[controller]")]
    [ApiController]
    public class FeladatokController : ControllerBase
    {
        private readonly ErettsegizzunkContext _context;
        public FeladatokController(ErettsegizzunkContext context)
        {
            _context = context;
        }

        [HttpPost("get-sok-feladat")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetFeladatoks([FromBody] int mettol)
        {
            return await _context.Tasks
                .Include(x => x.Level)
                .Include(x => x.Subject)
                .Include(x => x.Themes)
                .Include(x => x.Type)
                .Where(x => x.Id > mettol)
                .Take(50)//100 lassú teszt kevesebbel
                .ToListAsync();
        }

        //Random 15 feladat tantárgy és szint (közép felső) paraméter alapján
        [HttpPost("get-random-feladatok")]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetFeladatoksTipusSzint([FromBody] FeladatokGetRandomDTO get)
        {
            if (get.Tantargy is null || get.Szint is null)
            {
                return BadRequest("Keresési adat nem lehet null.");
            }

            List<Models.Task> randomFeladatok = await _context.Tasks
                .Include<Models.Task, Level>(x => x.Level)
                .Include<Models.Task, Subject>(x => x.Subject)
                .Include(x => x.Themes)
                .Include<Models.Task, Models.Type>(x => x.Type)
                .Where(x => x.Subject.Name == get.Tantargy && x.Level.Name == get.Szint)
                .OrderBy(x => EF.Functions.Random())
                .Take(15)
                .ToListAsync();

            return Ok(randomFeladatok);
        }

        //Egy feladat lekérése id alapján
        // GET: erettsegizzunk/Feladatok/get-egy-feladat
        [HttpPost("get-egy-feladat")]
        public async Task<ActionResult<Models.Task>> GetFeladatok([FromBody] FeladatokGetSpecificDTO get)
        {
            if (get.Id is null)
            {
                return BadRequest("Keresési adat nem lehet null.");
            }

            Models.Task? feladat = await _context.Tasks
                .Include<Models.Task, Level>(x => x.Level)
                .Include<Models.Task, Subject>(x => x.Subject)
                .Include(x => x.Themes)
                .Include<Models.Task, Models.Type>(x => x.Type)
                .Where(x => x.Id == get.Id)
                .FirstOrDefaultAsync();

            if (feladat == null)
            {
                return NotFound("Nincs a keresésnek megfelelő elem.");
            }

            return feladat;
        }

        //-----------------Kell egy get get random feladat témára való szűrésre is------------------------------------
        //adminban 100 egyszerre ne legyen dublikát: a lekért lista utolsó eleme alapjén kérjük le a következő adatokat --> elv működik 50 van 100 helyett

        //Egy feladat módosítása id alapján
        // PUT: api/Feladatoks/put-egy-feladat
        [HttpPut("put-egy-feladat")]
        public async Task<IActionResult> PutFeladatok([FromBody]FeladatokPutPostDTO put)
        {
            if (!Program.LoggedInUsers.ContainsKey(put.Token) && Program.LoggedInUsers[put.Token].Permission.Level != 9)
            {
                return BadRequest("Nincs jogosultságod!");
            }

                if (put.Id < 1)
            {
                return BadRequest("Nincs ilyen id");
            }

            Models.Task? feladat = await _context.Tasks.FindAsync(put.Id);

            if (feladat is null)
            {
                return BadRequest("Nincs ilyen id-vel rendelkező feladat");
            }

            feladat.Description = put.Leiras;
            feladat.Answers = put.Megoldasok;
            feladat.IsCorrect = put.Helyese;
            feladat.SubjectId = put.TantargyId;
            feladat.TypeId = put.TipusId;
            feladat.LevelId = put.SzintId;
            feladat.PicName = put.KepNev;
            
            _context.Entry(feladat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba: {ex.Message}");
            }

            return Ok("Sikeres adatmódosítás");
        }

        //Egy feladat felvitele
        // POST: api/Feladatoks/post-egy-feladat
        [HttpPost("post-egy-feladat")]
        public async Task<ActionResult<Models.Task>> PostFeladat([FromBody]FeladatokPutPostDTO post)
        {
            Models.Task feladatok = new Models.Task
            {
                Description = post.Leiras,
                Answers = post.Megoldasok,
                IsCorrect = post.Helyese,
                SubjectId = post.TantargyId,
                TypeId = post.TipusId,
                LevelId = post.SzintId,
                PicName = post.KepNev                
            };

            try
            {
                ActionResult<Token> eredmeny = await VanToken(post);//atirni??
                switch (eredmeny.Result)
                {
                    case NotFoundResult:
                        {
                            throw new Exception("Felhasználónak nincs megfelelő jogosultsága");
                        }
                    case BadRequestResult:
                        {
                            throw new Exception(eredmeny.Result.ToString());
                        }
                    case NotFoundObjectResult:
                        {
                            throw new Exception("Felhasználónak nincs megfelelő jogosultsága");
                        }
                }

                _context.Tasks.Add(feladatok);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba: {ex.Message}");
            }

            return Ok("Erőforrás sikeresen létrehozva");
        }

        //Több feladat felvitele
        // POST: api/Feladatoks/post-egy-feladat
        [HttpPost("post-tobb-feladat")]
        public async Task<ActionResult<Models.Task>> PostFeladatok([FromBody] List<FeladatokPutPostDTO> post)
        {
            if (!Program.LoggedInUsers.ContainsKey(post[0].Token) && Program.LoggedInUsers[post[0].Token].Permission.Level != 9)
            {
                return BadRequest("Nincs jogosultságod!");
            }

            try
            {
                foreach (FeladatokPutPostDTO feladatok in post)
                {
                    Models.Task feladat = new Models.Task
                    {
                        Description = feladatok.Leiras,
                        Text = feladatok.Szoveg,
                        Answers = feladatok.Megoldasok,
                        IsCorrect = feladatok.Helyese,
                        SubjectId = feladatok.TantargyId,
                        TypeId = feladatok.TipusId,
                        LevelId = feladatok.SzintId,
                        PicName = feladatok.KepNev
                    };

                    _context.Tasks.Add(feladat);
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba: {ex.Message}");
            }

            return Ok("Erőforrás sikeresen létrehozva");
        }

        //Egy feladat törlése id alapján
        // DELETE: api/Feladatoks/delete-egy-feladat
        [HttpDelete("delete-feladatok")]
        public async Task<IActionResult> DeleteFeladatok([FromBody] FeladatokDeleteDTO feladatokDeleteDTO)
        {
            if (!Program.LoggedInUsers.ContainsKey(feladatokDeleteDTO.Token) && Program.LoggedInUsers[feladatokDeleteDTO.Token].Permission.Level != 9)
            {
                return BadRequest("Nincs jogosultságod!");
            }
            try
            {
                List<Models.Task> feladatok = await _context.Tasks.Where(x => feladatokDeleteDTO.Ids.Contains(x.Id)).ToListAsync();
                if (feladatok == null)
                {
                    return NotFound("Nincs feladat ilyen id-vel.");
                }
                _context.Tasks.RemoveRange(feladatok);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba: {ex.Message}");
            }

            return Ok("Törlés sikeresen végrahajtva");
        }

        
        private async Task<ActionResult<Token>> VanToken(FeladatokDeleteDTO deleteDTO)
        {
            if (deleteDTO is null)
            {
                return BadRequest();
            }

            Token vaneToken;

            try
            {
                vaneToken = await _context.Tokens.FirstOrDefaultAsync(x => x.TokenString == deleteDTO.Token && x.Active);
            }
            catch(ArgumentNullException nex)
            {
                return NotFound(nex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (vaneToken is null)
            {
                return NotFound();
            }

            return vaneToken;
        }

        private async Task<ActionResult<Token>> VanToken(FeladatokPutPostDTO putPostDTO)
        {
            if (putPostDTO is null)
            {
                return BadRequest();
            }

            Token vaneToken;

            try
            {
                vaneToken = await _context.Tokens.FirstOrDefaultAsync(x => x.TokenString == putPostDTO.Token && x.Active);
            }
            catch (ArgumentNullException nex)
            {
                return NotFound(nex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            if (vaneToken is null)
            {
                return NotFound("Nincs ilyen token bazzeg!");
            }

            return vaneToken;
        }

        private bool FeladatokExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
