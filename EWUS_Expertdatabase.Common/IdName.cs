using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWUS_Expertdatabase.Common
{
   public class IdName
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }

    public class Item
    {
        public Item()
        {
            this.Fields = new Collection<Field>();
        }

        public string ObjectId { get; set; }

        public string Name { get; set; }

        public Collection<Field> Fields { get; set; }

        public object GetValue(string name)
        {
            Field f = GetFieldByName(name);

            if (f != null)
                return f.GetValue();
            else
                return null;
        }

        protected virtual Field GetFieldByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return null;

                if (this.HasFields() == false)
                    return null;

                var f = from it in Fields
                        where it.Name.ToLower() == name.ToLower()
                        select it;

                if (f != null)
                    return f.FirstOrDefault();
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public bool HasFields()
        {
            return this.Fields != null && this.Fields.Count > 0;
        }
    }

    public class Field
    {
        public Field(string name, object value)
        {
            this.Name = name;
            this.Value = value;
            this.IsEnumerable = false;
        }

        public object Value { get; set; }

        public bool IsEnumerable { get; set; }

        public string Name { get; set; }

        public object GetValue()
        {
            if (Value != null)
            {
                try
                {
                    if (Value.GetType() == typeof(object[]))
                        return (Value as object[])[0];
                    else if (Value.GetType() == typeof(string[]))
                        return (Value as string[])[0];
                }
                catch
                {
                }
                return Value;
            }
            return null;
        }
    }
}
