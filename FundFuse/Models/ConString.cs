using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
namespace TMP.Models
{
    public partial class ConString : DbContext
    {
        public ConString()
            : base("name=ConString")
        {
        }
    }
}
