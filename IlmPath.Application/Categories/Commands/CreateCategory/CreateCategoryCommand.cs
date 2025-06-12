using FluentResults;
using IlmPath.Application.DTOs.Categories.Requests;
using IlmPath.Application.DTOs.Categories.Responses;
using IlmPath.Domain.Entities;
using MediatR;

namespace IlmPath.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string Name, string Slug) : IRequest<Category>;
