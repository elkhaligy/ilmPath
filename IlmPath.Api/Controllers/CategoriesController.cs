using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using System.Collections.Generic;
using IlmPath.Application.Categories.Queries.GetAllCategories;
using IlmPath.Application.Categories.Queries.GetCategoryById;
using IlmPath.Application.Categories.Queries.GetCategoryBySlug;
using IlmPath.Application.Categories.Commands.UpdateCategory;
using IlmPath.Application.Categories.Commands.DeleteCategory;
using IlmPath.Application.Categories.Commands.CreateCategory;
using AutoMapper;
using IlmPath.Application.Categories.DTOs.Requests;
using IlmPath.Application.Categories.DTOs.Responses;

namespace IlmPath.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

// End Points for Categories
// GET: api/categories
// GET: api/categories/{id}
// GET: api/categories/by-slug/{slug}
// POST: api/categories
// PUT: api/categories/{id}
// DELETE: api/categories/{id}

public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CategoriesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAll()
    {
        var query = new GetAllCategoriesQuery();
        var categories = await _mediator.Send(query);
        return Ok(_mapper.Map<List<CategoryResponse>>(categories));
    }

    // GET: api/categories/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryResponse>> GetById(int id)
    {
        var query = new GetCategoryByIdQuery(id);
        var category = await _mediator.Send(query);
        
        if (category == null)
            return NotFound();

        return Ok(_mapper.Map<CategoryResponse>(category));
    }

    // GET: api/categories/by-slug/{slug}
    [HttpGet("by-slug/{slug}")]
    public async Task<ActionResult<CategoryResponse>> GetBySlug(string slug)
    {
        var query = new GetCategoryBySlugQuery(slug);
        var category = await _mediator.Send(query);
        
        if (category == null)
            return NotFound();

        return Ok(_mapper.Map<CategoryResponse>(category));
    }

    // POST: api/categories
    [HttpPost]
    public async Task<ActionResult<CategoryResponse>> Create(CreateCategoryRequest request)
    {
        var command = _mapper.Map<CreateCategoryCommand>(request);

        var category = await _mediator.Send(command);
        var categoryResponse = _mapper.Map<CategoryResponse>(category);

        return CreatedAtAction(nameof(GetById), new { id = categoryResponse.Id }, categoryResponse);
    }


    // PUT: api/categories/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryResponse>> Update(int id, UpdateCategoryRequest request)
    {
        var command = _mapper.Map<UpdateCategoryCommand>((request,id));

        var category = await _mediator.Send(command);
        
        if (category == null)
            return NotFound();

        return Ok(_mapper.Map<CategoryResponse>(category));
    }

    // DELETE: api/categories/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteCategoryCommand(id);
        var result = await _mediator.Send(command);
        
        if (!result)
            return NotFound();

        return NoContent();
    }
}
