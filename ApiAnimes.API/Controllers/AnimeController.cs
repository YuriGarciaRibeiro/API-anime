using System.Linq.Expressions;
using ApiAnimes.Application.Services;
using ApiAnimes.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using LinqKit;
using ApiAnimes.API.Responses;
using AutoMapper;
using ApiAnimes.Application.Dto.Anime;

namespace ApiAnimes.API.Controllers
{
    /// <summary>
    /// Controller para gerenciar os animes.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AnimeController : ControllerBase
    {
        private readonly IAnimeService animeService;
        private readonly ILogger<AnimeController> _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor do AnimeController.
        /// </summary>
        /// <param name="animeService">Serviço de gerenciamento de animes.</param>
        /// <param name="logger">Logger para rastreamento de informações.</param>
        /// <param name="mapper">Mapper para conversão de objetos.</param>
        public AnimeController(IAnimeService animeService, ILogger<AnimeController> logger, IMapper mapper)
        {
            this.animeService = animeService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Lista todos os animes com suporte a paginação e filtros.
        /// </summary>
        /// <param name="page">Número da página atual.</param>
        /// <param name="limit">Quantidade de itens por página.</param>
        /// <param name="name">Filtro pelo nome do anime.</param>
        /// <param name="description">Filtro pela descrição do anime.</param>
        /// <param name="author">Filtro pelo autor do anime.</param>
        /// <returns>Uma lista paginada de animes.</returns>
        [HttpGet]
        public async Task<ActionResult<PaginatedResponse<AnimeResponseDto>>> GetAll(
            int page = 1,
            int limit = 10,
            string? name = null,
            string? description = null,
            string? author = null)
        {
            Expression<Func<Anime, bool>> predicate = a => true;

            if (!string.IsNullOrEmpty(name))
            {
                predicate = predicate.And(a => a.Name.Equals(name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                predicate = predicate.And(a => a.Description.Contains(description));
            }

            if (!string.IsNullOrEmpty(author))
            {
                predicate = predicate.And(a => a.Author.Equals(author));
            }

            var animes = await animeService.GetAll(page, limit, predicate);
            var animesDto = _mapper.Map<IEnumerable<AnimeResponseDto>>(animes);

            var nextPageUrl = Url.Action("GetAll", new
            {
                page = page + 1,
                limit,
                name,
                description,
                author
            });

            var PaginatedResponse = new PaginatedResponse<AnimeResponseDto>(
                animesDto,
                page,
                animes.Count,
                animes.Count() == limit ? nextPageUrl : null
            );

            _logger.LogInformation("Listando animes");
            return Ok(PaginatedResponse);
        }

        /// <summary>
        /// Obtém um anime pelo ID.
        /// </summary>
        /// <param name="id">ID do anime.</param>
        /// <returns>O anime correspondente ou um erro 404 se não encontrado.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AnimeResponseDto>> GetBy(int id)
        {
            var anime = await animeService.GetBy(u => u.Id == id);
            if (anime == null)
            {
                _logger.LogWarning($"Anime com id {id} não encontrado");
                return NotFound();
            }
            var animeDto = _mapper.Map<AnimeResponseDto>(anime);
            _logger.LogInformation($"Anime com id {id} encontrado");
            return Ok(animeDto);
        }

        /// <summary>
        /// Cria um novo anime.
        /// </summary>
        /// <param name="anime">Dados do anime a ser criado.</param>
        /// <returns>O anime criado com seu ID.</returns>
        [HttpPost]
        public async Task<ActionResult<AnimeResponseDto>> Create([FromBody] AnimeRequestDto anime)
        {
            if (anime == null)
            {
                _logger.LogWarning("Anime nulo ao tentar criar");
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo inválido ao tentar criar");
                return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            var animeEntity = _mapper.Map<Anime>(anime);
            var createdAnime = await animeService.Create(animeEntity);
            _logger.LogInformation($"Anime criado com id {createdAnime.Id}");
            return CreatedAtAction(nameof(GetBy), new { id = createdAnime.Id }, createdAnime);
        }

        /// <summary>
        /// Atualiza um anime existente pelo ID.
        /// </summary>
        /// <param name="id">ID do anime a ser atualizado.</param>
        /// <param name="anime">Dados atualizados do anime.</param>
        /// <returns>O anime atualizado.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<AnimeResponseDto>> Update(int id, [FromBody] AnimeUpdateDto anime)
        {
            if (!ModelState.IsValid || anime == null)
            {
                _logger.LogWarning("Modelo inválido ao tentar atualizar");
                return BadRequest(ModelState);
            }

            var animeEntity = _mapper.Map<Anime>(anime);
            animeEntity.Id = id;

            var updatedAnime = await animeService.Update(animeEntity);
            _logger.LogInformation($"Anime com id {id} atualizado");
            return Ok(updatedAnime);
        }

        /// <summary>
        /// Deleta um anime pelo ID.
        /// </summary>
        /// <param name="id">ID do anime a ser deletado.</param>
        /// <returns>Status de sucesso ou erro.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await animeService.Delete(id);
            _logger.LogInformation($"Anime com id {id} deletado");
            return NoContent();
        }
    }
}
