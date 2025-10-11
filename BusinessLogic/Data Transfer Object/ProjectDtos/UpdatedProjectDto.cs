using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data_Transfer_Object.ProjectDtos
{
    public class UpdatedProjectDto : CreatedProjectDto
    {
        public int Id { get; set; }
    }
}
