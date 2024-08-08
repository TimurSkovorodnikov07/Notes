using System.ComponentModel.DataAnnotations;

public class EmailVerifyQuery
{
    [Required] public Guid UserId { get; set; }
    [Required] public string Code { get; set; }
}