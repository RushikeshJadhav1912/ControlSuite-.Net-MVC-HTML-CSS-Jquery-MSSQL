// EditUserViewModel.cs
using System.ComponentModel.DataAnnotations;

public class EditUserViewModel
{
    public string Id { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    // Add other properties as needed for editing
}
