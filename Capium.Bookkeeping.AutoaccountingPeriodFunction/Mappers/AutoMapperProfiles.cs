using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Capium.Bookkeeping.AutoaccountingPeriodFunction.Mappers
{
    public class AutoMapperProfiles : Profile
    {
        public static string JsonConverter<T>(T value)
        {
            return value == null ? null : JsonConvert.SerializeObject(value);
        }

    }
}
