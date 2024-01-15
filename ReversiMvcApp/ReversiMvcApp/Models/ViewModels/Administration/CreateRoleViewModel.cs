using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Models.ViewModels.Administration
{
    public class CreateRoleViewModel
    {
        [required]
        public string RoleName { get; set; }
    }
}
