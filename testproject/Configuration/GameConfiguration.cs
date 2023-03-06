using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using testproject.Entities;

namespace testproject.Configuration
{
    public class GameConfiguration:IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasOne(p=>p.FirstPlayer).WithMany(p=>p.FirstPlayers).HasForeignKey(p=>p.FirstPlayerid);
            builder.HasOne(p => p.SecondPlayer).WithMany(p => p.SecondPlayers).HasForeignKey(p => p.SecondPlayerid);

        }
    }
}
