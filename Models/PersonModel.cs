using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppZK9500.Models
{
    public class PersonModel
    {

        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public byte[] Huella { get; set; }

    }
}
