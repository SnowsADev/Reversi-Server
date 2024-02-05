﻿using System;

namespace ReversiMvcApp.Models.Abstract
{
    public abstract class Auditable
    {
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.MinValue;
        public bool IsDeleted { get; set; } = false;
    }


}
