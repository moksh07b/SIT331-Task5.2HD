using Microsoft.AspNetCore.Mvc;
using Gallery.Models;
using Gallery.Interfaces;

namespace Gallery.Controllers;

/// <summary>
/// Controller for managing artists in the Gallery API.
/// Provides endpoints for CRUD operations and additional utilities for artists.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ArtistController : ControllerBase
{
    private readonly IArtistRepository _artistRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArtistController"/> class.
    /// </summary>
    /// <param name="artistRepository">The artist repository to interact with the database.</param>
    public ArtistController(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;
    }

    /// <summary>
    /// Retrieve all artists.
    /// </summary>
    /// <returns>A list of all artists.</returns>
    /// <response code="200">Returns the list of artists</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet()]
    public async Task<IActionResult> GetAllArtists()
    {
        var artists = await _artistRepository.GetAllAsync();
        return Ok(artists);
    }

    /// <summary>
    /// Retrieve a specific artist by their ID.
    /// </summary>
    /// <param name="id">The ID of the artist to retrieve.</param>
    /// <returns>The artist with the specified ID.</returns>
    /// <response code="200">Returns the artist with the specified ID</response>
    /// <response code="404">If the artist is not found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetArtistById(int id)
    {
        var artist = await _artistRepository.GetByIdAsync(id);
        if (artist == null)
        {
            return NotFound();
        }
        return Ok(artist);
    }

    /// <summary>
    /// Create a new artist.
    /// </summary>
    /// <param name="artist">The artist to create.</param>
    /// <returns>The created artist with their ID.</returns>
    /// <remarks>
    /// Sample Request:
    ///     POST /api/artist
    ///     {
    ///         "name": "Artist Name",
    ///         "bio": "A brief biography",
    ///         "birthDate": "1980-01-01",
    ///         "deathDate": null,
    ///         "nationality": "American"
    ///     }
    /// </remarks>
    /// <response code="201">Returns the newly created artist</response>
    /// <response code="400">If the artist is null or invalid</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> CreateArtist([FromBody] Artist artist)
    {
        if (artist == null)
        {
            return BadRequest();
        }
        await _artistRepository.AddAsync(artist);
        return CreatedAtAction(nameof(GetArtistById), new { id = artist.Id }, artist);
    }

    /// <summary>
    /// Update an existing artist.
    /// </summary>
    /// <param name="id">The ID of the artist to update.</param>
    /// <param name="artist">The updated artist data.</param>
    /// <returns>The updated artist.</returns>
    /// <remarks>
    /// Sample Request:
    ///     PUT /api/artist/1
    ///     {
    ///         "id": 1,
    ///         "name": "Updated Artist Name",
    ///         "bio": "Updated biography",
    ///         "birthDate": "1980-01-01",
    ///         "deathDate": null,
    ///         "nationality": "American"
    ///     }
    /// </remarks>
    /// <response code="200">Returns the updated artist</response>
    /// <response code="400">If the artist data is invalid</response>
    /// <response code="404">If the artist is not found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateArtist(int id, [FromBody] Artist artist)
    {
        if (id < 0)
        {
            return BadRequest("Id cannot be negative");
        }
        var existingArtist = await _artistRepository.GetByIdAsync(id);
        if (existingArtist == null)
        {
            return NotFound();
        }
        await _artistRepository.UpdateAsync(artist);
        return Ok(artist);
    }

    /// <summary>
    /// Delete an artist by their ID.
    /// </summary>
    /// <param name="id">The ID of the artist to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    /// <response code="204">If the artist is successfully deleted</response>
    /// <response code="404">If the artist is not found</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArtist(int id)
    {
        var existingArtist = await _artistRepository.GetByIdAsync(id);
        if (existingArtist == null)
        {
            return NotFound();
        }
        await _artistRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Check if an artist exists by their ID.
    /// </summary>
    /// <param name="id">The ID of the artist to check.</param>
    /// <returns>A boolean indicating whether the artist exists.</returns>
    /// <response code="200">Returns true if the artist exists, false otherwise</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("exists/{id}")]
    public async Task<IActionResult> ArtistExists(int id)
    {
        var exists = await _artistRepository.ExistsAsync(id);
        return Ok(exists);
    }
}