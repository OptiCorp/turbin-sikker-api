using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace turbin.sikker.core.Model.DTO.ChecklistDtos
{
    public class ChecklistViewNoUserDto
    {
        public string Id { get; set; }
        public string Title { get; set; }

        [EnumDataType(typeof(ChecklistStatus))]
        public ChecklistStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
