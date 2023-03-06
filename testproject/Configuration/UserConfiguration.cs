using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using testproject.Entities;

namespace testproject.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            
            builder.HasData(new User[]
            {
                new User { Id = 1, Nickname="Igor"}
                , new User { Id = 2,Nickname="Vasya"}
            });
        }
    }
}
