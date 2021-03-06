﻿using OrvilleX.Dependency;

namespace OrvilleX.MongoDB
{
    public interface IDocumentDBConfiguration : ISingletonDependency
    {
        string Host { get; set; }

        string UserName { get; set; }

        string Password { get; set; }

        string DataBase { get; set; }

        bool NoTotal { get; set; }

        bool SSL { get; set; }
    }
}
