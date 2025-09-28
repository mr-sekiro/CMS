using DataAccess.Data;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Classes
{
    internal class ProjectRepo(ApplicationDbContext dbContext) : GenericRepo<Project>(dbContext), IProjectRepo
    {

    }
}
