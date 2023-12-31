﻿using RaspberryPi.Domain.Models;

namespace RaspberryPi.Domain.Interfaces.Repositories
{
    public interface ISqlLiteKeyValueRepository
    {
        bool Create(SqlLiteKeyValue keyValue);

        SqlLiteKeyValue? Get(int id);

        IEnumerable<SqlLiteKeyValue> List();

        bool Update(SqlLiteKeyValue keyValue);

        bool Delete(int id);

        int Truncate();
    }
}