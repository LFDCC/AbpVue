﻿using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace AbpVue.EntityFrameworkCore
{
    public static class AbpVueDbContextModelCreatingExtensions
    {
        public static void ConfigureAbpVue(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(AbpVueConsts.DbTablePrefix + "YourEntities", AbpVueConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
        }
    }
}