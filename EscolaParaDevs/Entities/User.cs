using EscolaParaDevs.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace EscolaParaDevs.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        [NotMapped]
        public string ConfirmPassword { get; set; }
        [NotMapped]
        public string CurrentPassword { get; set; }
        public TypeUser TypeUser { get; set; }

    }
}
