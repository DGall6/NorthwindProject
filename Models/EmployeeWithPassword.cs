using System.ComponentModel.DataAnnotations;
public class EmployeeWithPassword : Employee
{
    [UIHint("password"), Required]
    public string Password { get; set; }
}