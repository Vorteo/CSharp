using System;

namespace projekt.Models
{
    class PrimaryKeyAttribute : Attribute
    {
        public bool Skip { get; set; } = false;
    }

    class ForeignKeyAttribute : Attribute
    {
        public bool Skip { get; set; } = false;
    }

    class TableAttribute : Attribute
    {
        public string Name { get; set; }
    }

}
