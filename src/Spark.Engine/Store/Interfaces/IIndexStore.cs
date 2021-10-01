/*
 * Copyright (c) 2021, Incendi (info@incendi.no) and contributors
 * See the file CONTRIBUTORS for details.
 *
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.githubusercontent.com/FirelyTeam/spark/stu3/master/LICENSE
 */

using Spark.Engine.Core;
using Spark.Engine.Model;

namespace Spark.Engine.Store.Interfaces
{
    public interface IIndexStore
    {
        void Save(IndexValue indexValue);
        void Delete(Entry entry);
        void Clean();
    }
}
