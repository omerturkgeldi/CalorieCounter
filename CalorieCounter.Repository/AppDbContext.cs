using CalorieCounter.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;
using System.Security.Claims;
using CalorieCounter.Core.Services;

namespace CalorieCounter.Repository
{
    public class AppDbContext : IdentityDbContext<UserApp, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Log> Loglar { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        

        public override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.Entity)
                    {
                        case EntityState.Added:
                            {
                                entityReference.OlusturulanTarih = DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                entityReference.GuncellemeTarihi = DateTime.Now;
                                break;
                            }

                    }
                }
            }
            return base.SaveChanges();
        }

        public Task<int> SaveChangesWithUserAsync(string id, CancellationToken cancellationToken = default)
        {

            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.OlusturulanTarih=DateTime.Now;
                                entityReference.EkleyenKullaniciFk = id;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.OlusturulanTarih).IsModified = false;
                                entityReference.SonGuncellemeYapanKullaniciFk = id;
                                entityReference.GuncellemeTarihi = DateTime.Now;
                                break;
                            }
                    }
                }

               else if (item.Entity is BaseEntityGuid entityReferenceGuid)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReferenceGuid.OlusturulanTarih=DateTime.Now;
                                entityReferenceGuid.EkleyenKullaniciFk = id;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReferenceGuid).Property(x => x.OlusturulanTarih).IsModified = false;
                                entityReferenceGuid.SonGuncellemeYapanKullaniciFk = id;
                                entityReferenceGuid.GuncellemeTarihi = DateTime.Now;
                                break;
                            }
                    }
                }



            }
            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var item in ChangeTracker.Entries())
            {
                if (item.Entity is BaseEntity entityReference)
                {
                    switch (item.State)
                    {
                        case EntityState.Added:
                            {
                                entityReference.OlusturulanTarih=DateTime.Now;
                                break;
                            }
                        case EntityState.Modified:
                            {
                                Entry(entityReference).Property(x => x.OlusturulanTarih).IsModified = false;
                                //entityReference.SonGuncellemeYapanKullaniciFk = id;
                                entityReference.GuncellemeTarihi = DateTime.Now;
                                break;
                            }
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

        }


    }

}
