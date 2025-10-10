using DataLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public  class RolService
    {

        private readonly DL_Rol _rolDL;
        public RolService(DL_Rol rolDL)
        {

            _rolDL = rolDL;
        }

        public async Task<List<Rol>> Listar()
        {
            return await _rolDL.Listar();
        }
    }
}
