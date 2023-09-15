using Nucleus.EntityFrameworkCore;

namespace Nucleus.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly NucleusDbContext _context;

        public InitialHostDbBuilder(NucleusDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}
