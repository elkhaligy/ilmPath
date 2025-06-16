using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IlmPath.Domain.Entities;

public class CartItem
{
    public int CourseId { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string? ThumbnailImageUrl { get; set; }
}