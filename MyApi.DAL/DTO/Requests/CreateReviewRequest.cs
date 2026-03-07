using System;
using System.ComponentModel.DataAnnotations;

namespace MyApi.DAL.DTO.Requests;

public class CreateReviewRequest
{

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    [MinLength(5, ErrorMessage = "Comment must be at least 5 characters long.")]
    public string Comment { get; set; }

}
