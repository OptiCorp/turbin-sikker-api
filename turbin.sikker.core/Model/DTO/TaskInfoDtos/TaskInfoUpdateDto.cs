using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace turbin.sikker.core.Model.DTO
{
    public class TaskInfoUpdate
    {
        public string TaskId { get; set; }
        public string Status { get; set; }
    }
}
