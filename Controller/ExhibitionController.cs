using Microsoft.AspNetCore.Mvc;
using Gallery.Models;
using Gallery.Interfaces;

namespace Gallery.Controllers;

/// <summary>
/// Controller for managing exhibitions in the Gallery API.
/// Provides endpoints for CRUD operations and additional utilities for exhibitions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ExhibitionController : ControllerBase
{
    private readonly IExhibitionRepository _exhibitionRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExhibitionController"/> class.
    /// </summary>
    /// <param name="exhibitionRepository">The exhibition repository to interact with the database.</param>
    public ExhibitionController(IExhibitionRepository exhibitionRepository)
    {
        _exhibitionRepository = exhibitionRepository;
    }

    /// <summary>
    /// Retrieve all exhibitions.
    /// </summary>
    /// <returns>A list of all exhibitions.</returns>
    /// <response code="200">Returns the list of exhibitions</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet()]
    public async Task<IActionResult> GetAllExhibitions()
    {
        var exhibitions = await _exhibitionRepository.GetAllAsync();
        return Ok(exhibitions);
    }

    /// <summary>
    /// Retrieve a specific exhibition by its ID.
    /// </summary>
    /// <param name="id">The ID of the exhibition to retrieve.</param>
    /// <returns>The exhibition with the specified ID.</returns>
    /// <response code="200">Returns the exhibition with the specified ID</response>
    /// <response code="404">If the exhibition is not found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetExhibitionById(int id)
    {
        var exhibition = await _exhibitionRepository.GetByIdAsync(id);
        if (exhibition == null)
        {
            return NotFound();
        }
        return Ok(exhibition);
    }

    /// <summary>
    /// Create a new exhibition.
    /// </summary>
    /// <param name="exhibition">The exhibition to create.</param>
    /// <returns>The created exhibition with its ID.</returns>
    /// <remarks>
    /// Sample Request:
    ///     POST /api/exhibition
    ///     {
    ///         "title": "Exhibition 1",
    ///         "startDate": "2023-10-01",
    ///         "endDate": "2023-10-15",
    ///         "location": "Gallery Hall A",
    ///         "description": "A sample exhibition"
    ///     }
    /// </remarks>
    /// <response code="201">Returns the newly created exhibition</response>
    /// <response code="400">If the exhibition is null or invalid</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> CreateExhibition([FromBody] Exhibition exhibition)
    {
        if (exhibition == null)
        {
            return BadRequest();
        }
        await _exhibitionRepository.AddAsync(exhibition);
        return CreatedAtAction(nameof(GetExhibitionById), new { id = exhibition.Id }, exhibition);
    }

    /// <summary>
    /// Update an existing exhibition.
    /// </summary>
    /// <param name="id">The ID of the exhibition to update.</param>
    /// <param name="exhibition">The updated exhibition data.</param>
    /// <returns>The updated exhibition.</returns>
    /// <remarks>
    /// Sample Request:
    ///     PUT /api/exhibition/1
    ///     {
    ///         "id": 1,
    ///         "title": "Updated Exhibition",
    ///         "startDate": "2023-10-01",
    ///         "endDate": "2023-10-20",
    ///         "location": "Gallery Hall B",
    ///         "description": "Updated description"
    ///     }
    /// </remarks>
    /// <response code="200">Returns the updated exhibition</response>
    /// <response code="400">If the exhibition data is invalid</response>
    /// <response code="404">If the exhibition is not found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExhibition(int id, [FromBody] Exhibition exhibition)
    {
        if (id < 0)
        {
            return BadRequest("Id cannot be negative");
        }
        var existingExhibition = await _exhibitionRepository.GetByIdAsync(id);
        if (existingExhibition == null)
        {
            return NotFound();
        }
        await _exhibitionRepository.UpdateAsync(exhibition);
        return Ok(exhibition);
    }

    /// <summary>
    /// Delete an exhibition by its ID.
    /// </summary>
    /// <param name="id">The ID of the exhibition to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    /// <response code="204">If the exhibition is successfully deleted</response>
    /// <response code="404">If the exhibition is not found</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExhibition(int id)
    {
        var existingExhibition = await _exhibitionRepository.GetByIdAsync(id);
        if (existingExhibition == null)
        {
            return NotFound();
        }
        await _exhibitionRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Check if an exhibition exists by its ID.
    /// </summary>
    /// <param name="id">The ID of the exhibition to check.</param>
    /// <returns>A boolean indicating whether the exhibition exists.</returns>
    /// <response code="200">Returns true if the exhibition exists, false otherwise</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("exists/{id}")]
    public async Task<IActionResult> ExhibitionExists(int id)
    {
        var exists = await _exhibitionRepository.ExistsAsync(id);
        return Ok(exists);
    }
}