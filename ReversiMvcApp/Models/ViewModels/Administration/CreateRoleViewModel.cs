using System.ComponentModel.DataAnnotations;

namespace ReversiMvcApp.Models.ViewModels.Administration
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
