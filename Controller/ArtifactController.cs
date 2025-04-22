using Microsoft.AspNetCore.Mvc;
using Gallery.Models;
using Gallery.Interfaces;

namespace Gallery.Controllers;

/// <summary>
/// Controller for managing artifacts in the Gallery API.
/// Provides endpoints for CRUD operations and additional utilities for artifacts.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ArtifactController : ControllerBase
{
    private readonly IArtifactRepository _artifactRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArtifactController"/> class.
    /// </summary>
    /// <param name="artifactRepository">The artifact repository to interact with the database.</param>
    public ArtifactController(IArtifactRepository artifactRepository)
    {
        _artifactRepository = artifactRepository;
    }

    /// <summary>
    /// Retrieve all artifacts.
    /// </summary>
    /// <returns>A list of all artifacts.</returns>
    /// <response code="200">Returns the list of artifacts</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet()]
    public async Task<IActionResult> GetAllArtifacts()
    {
        var artifacts = await _artifactRepository.GetAllAsync();
        return Ok(artifacts);
    }

    /// <summary>
    /// Retrieve a specific artifact by its ID.
    /// </summary>
    /// <param name="id">The ID of the artifact to retrieve.</param>
    /// <returns>The artifact with the specified ID.</returns>
    /// <response code="200">Returns the artifact with the specified ID</response>
    /// <response code="404">If the artifact is not found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetArtifactById(int id)
    {
        var artifact = await _artifactRepository.GetByIdAsync(id);
        if (artifact == null)
        {
            return NotFound();
        }
        return Ok(artifact);
    }

    /// <summary>
    /// Create a new artifact.
    /// </summary>
    /// <param name="artifact">The artifact to create.</param>
    /// <returns>The created artifact with its ID.</returns>
    /// <remarks>
    /// Sample Request:
    ///     POST /api/artifact
    ///     {
    ///         "title": "Artifact 1",
    ///         "description": "A sample artifact",
    ///         "artistId": 1,
    ///         "exhibitionId": 2
    ///     }
    /// </remarks>
    /// <response code="201">Returns the newly created artifact</response>
    /// <response code="400">If the artifact is null or invalid</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> CreateArtifact([FromBody] Artifact artifact)
    {
        if (artifact == null)
        {
            return BadRequest("Artifact is required.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _artifactRepository.AddAsync(artifact);
        var createdArtifact = await _artifactRepository.GetByIdAsync(artifact.Id);

        return CreatedAtAction(nameof(GetArtifactById), new { id = artifact.Id }, createdArtifact);
    }

    /// <summary>
    /// Update an existing artifact.
    /// </summary>
    /// <param name="id">The ID of the artifact to update.</param>
    /// <param name="artifact">The updated artifact data.</param>
    /// <returns>The updated artifact.</returns>
    /// <remarks>
    /// Sample Request:
    ///     PUT /api/artifact/1
    ///     {
    ///         "id": 1,
    ///         "title": "Updated Artifact",
    ///         "description": "Updated description",
    ///         "artistId": 1,
    ///         "exhibitionId": 2
    ///     }
    /// </remarks>
    /// <response code="200">Returns the updated artifact</response>
    /// <response code="400">If the artifact data is invalid</response>
    /// <response code="404">If the artifact is not found</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateArtifact(int id, [FromBody] Artifact artifact)
    {
        if (id < 0)
        {
            return BadRequest("Id cannot be negative");
        }

        if (artifact == null)
        {
            return BadRequest("Artifact data is required.");
        }

        var existingArtifact = await _artifactRepository.GetByIdAsync(id);
        if (existingArtifact == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _artifactRepository.UpdateAsync(artifact);
        return Ok(artifact);
    }

    /// <summary>
    /// Delete an artifact by its ID.
    /// </summary>
    /// <param name="id">The ID of the artifact to delete.</param>
    /// <returns>No content if the deletion is successful.</returns>
    /// <response code="204">If the artifact is successfully deleted</response>
    /// <response code="400">If the ID is invalid</response>
    /// <response code="404">If the artifact is not found</response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArtifact(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }
        var existingArtifact = await _artifactRepository.GetByIdAsync(id);
        if (existingArtifact == null)
        {
            return NotFound();
        }
        await _artifactRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Check if an artifact exists by its ID.
    /// </summary>
    /// <param name="id">The ID of the artifact to check.</param>
    /// <returns>A boolean indicating whether the artifact exists.</returns>
    /// <response code="200">Returns true if the artifact exists, false otherwise</response>
    /// <response code="400">If the ID is invalid</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("exists/{id}")]
    public async Task<IActionResult> ArtifactExists(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }
        var exists = await _artifactRepository.ExistsAsync(id);
        return Ok(exists);
    }
}