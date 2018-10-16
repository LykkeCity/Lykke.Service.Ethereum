﻿using System.Numerics;
using Lykke.AzureStorage.Tables.Entity.Metamodel;
using Lykke.AzureStorage.Tables.Entity.Metamodel.Providers;
using Lykke.Service.Ethereum.AzureRepositories.Serializers;


namespace Lykke.Service.Ethereum.AzureRepositories
{
    public abstract class RepositoryBase
    {
        static RepositoryBase()
        {
            var provider = new CompositeMetamodelProvider()
                .AddProvider
                (
                    new AnnotationsBasedMetamodelProvider()
                )
                .AddProvider
                (
                    new ConventionBasedMetamodelProvider()
                        .AddTypeSerializerRule
                        (
                            t => t == typeof(BigInteger),
                            s => new BigIntegerSerializer()
                        )
                );

            EntityMetamodel.Configure(provider);
        }
    }
}
