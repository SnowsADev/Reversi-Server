using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReversiMvcApp.Models.ViewModels.Administration
{
    public class EditRoleViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
