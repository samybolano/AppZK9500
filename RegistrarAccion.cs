using AppZK9500.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppZK9500
{
    public partial class RegistrarAccion : Form
    {
        int ID;
        public RegistrarAccion(int Id)
        {
            InitializeComponent();
            ID = Id;
        }

        private void RegistrarAccion_Load(object sender, EventArgs e)
        {
            using (var context = new MiDbContext())
            {
                var Person = context.Person.Where(x => x.Id == ID).FirstOrDefault();

                if(Person != null)
                { 
                    lblNombre.Text = Person.Nombre;

                }
            }

        }
    }
}
