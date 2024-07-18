﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Senparc.Ncf.Core.Models.DataBaseModel;
using Senparc.Ncf.XncfBase.Attributes;

namespace Senparc.Xncf.DynamicData.Models
{
    [XncfAutoConfigurationMapping]
    public class DynamicData_ColorConfigurationMapping : ConfigurationMappingWithIdBase<Color, int>
    {
        public override void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.Property(e => e.Red).IsRequired();
            builder.Property(e => e.Green).IsRequired();
            builder.Property(e => e.Blue).IsRequired();
        }
    }
}
