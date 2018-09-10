using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using EWUS_Expertdatabase.Data;
using EWUS_Expertdatabase.Model;
using System.Net;
using System.Data.Entity;
using Microsoft.Practices.ServiceLocation;

namespace EWUS_Expertdatabase.Common
{
    public static class ItemExtension
    {
        public static object ConvertTo(this Item it, object to, ICollection<IValueConverter> converters = null)
        {
            Type targetType = to.GetType();

            foreach (var field in it.Fields)
            {
                if (field == null || string.IsNullOrWhiteSpace(field.Name))
                    continue;

                PropertyInfo toProp = null;
                try
                {
                    //Get the matching property from the target
                    toProp =
                        targetType.GetProperty(field.Name.Trim());
                }
                catch (AmbiguousMatchException)
                {
                    try
                    {
                        toProp =
                                targetType.GetProperty(field.Name.Trim(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                    }
                    catch
                    {

                    }
                }

                if (toProp == null)
                    continue;

                if (field.Name.Trim().ToLower() == "name")
                {
                    object nameValue = field.Value;
                    string trimName = string.Empty;

                    if (nameValue != null)
                        trimName = nameValue.ToString();

                    if (string.IsNullOrWhiteSpace(trimName) == false)
                    {
                        trimName = trimName.Trim();

                        field.Value = trimName;
                    }
                }
                
                //If it exists and it's writeable
                if (toProp != null && toProp.CanWrite)
                {
                    if (toProp.PropertyType.InheritsFrom<CoreObject>())
                    {
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        Item NewItemObj = null;
                        try
                        {
                            NewItemObj = JsonConvert.DeserializeObject(field.Value.ToString(), typeof(Item)) as Item;
                        }
                        catch
                        {
                            NewItemObj = null;
                        }

                        if (NewItemObj != null)
                        {
                            long Id = NewItemObj.ObjectId.ToLong(0);

                            if (Id == 0)
                                Id = NewItemObj.Id;

                            if (Id != 0)
                            {
                                //object updateObject = repository.GetByTypeAndId(toProp.PropertyType, Id, null, false, false);                                
                                using (IRepository repo = ServiceLocator.Current.TryGet<IRepository>(toProp.PropertyType.Name + "Repository"))
                                {
                                    object updateObject = repo.GetById(Id);

                                    if (updateObject != null)
                                    {
                                        var currenValue = toProp.GetValue(to);

                                        updateObject = NewItemObj.ConvertTo(updateObject);

                                        //DataContext.RegisterCoreForChange(updateObject as CoreObject, context);
                                        toProp.SetValue(to, updateObject);
                                    }
                                }
                            }
                            else
                            {
                                // new object - insert
                                if (toProp.PropertyType.IsAbstract)
                                {
                                    // target klasa je abstraktne
                                    string TargetClassTypeFullName = string.Format("{0}.{1}, {2}",
                                        toProp.PropertyType.Namespace, field.FieldType, toProp.PropertyType.Namespace);
                                    Type TargetClassType = Type.GetType(TargetClassTypeFullName);

                                    if (TargetClassType != null)
                                    {
                                        var newObject = Activator.CreateInstance(TargetClassType);

                                        newObject = NewItemObj.ConvertTo(newObject);

                                        if (newObject != null)
                                            toProp.SetValue(to, newObject, null);
                                    }
                                }
                                else
                                {
                                    // target klasa nije abstraktne
                                    var newObject = Activator.CreateInstance(toProp.PropertyType);

                                    newObject = NewItemObj.ConvertTo(newObject);

                                    if (newObject != null)
                                        toProp.SetValue(to, newObject, null);
                                }
                            }
                        }
                    }
                    else if ((toProp.PropertyType == typeof(Reference)) || field.FieldType == typeof(Reference).Name)
                    {
                        Reference value = null;

                        if (field.Value is Reference)
                        {
                            value = field.Value as Reference;
                        }
                        else
                        {
                            Reference oldValue = toProp.GetValue(to) as Reference;

                            var oldId = (oldValue != null) ? oldValue.Id : 0;

                            Reference newValue = field.TryGetValueAsType<Reference>();

                            if (newValue != null)
                            {
                                value = newValue;
                            }
                            else
                            {
                                var id = field.Value.ToLong(0);

                                if (id != 0 && id != oldId)
                                    value = id;
                            }
                        }

                        if (value != null)
                        {
                            toProp.SetValue(to, value, null);
                        }
                    }
                    else if (field.DataType == "AttachmentCollection")
                    {
                        //Collection<Attachment> currentValue = toProp.GetValue(to) as Collection<Attachment>;
                        Collection<Attachment> currentValue = new Collection<Attachment>();
                        IEnumerable<Item> items = field.GetValueAsCollectionOfItems();
                        if (items != null)
                        {
                            foreach (Item ine in items)
                            {
                                Attachment updateObject = ine.Fields[0].TryGetValueAsType<Attachment>();
                                if (!updateObject.IsDeleted)
                                    currentValue.Add(updateObject);
                            }
                            toProp.SetValue(to, currentValue, null);
                        }
                    }
                    else if (field.FieldType == typeof(ItemCollection).Name)
                    {
                        Type inner = toProp.PropertyType;

                        object list = Activator.CreateInstance(inner);

                        var currentList = toProp.GetValue(to) as IEnumerable;

                        bool hasCurrentList = currentList != null && currentList.Count() > 0;

                        if (inner.IsGenericType && inner.GetInterface("IEnumerable`1") != null)
                        {
                            inner = inner.GetGenericArguments()[0];
                        }

                        if (hasCurrentList == false)
                            toProp.SetValue(to, list, null);

                        var currenValue = toProp.GetValue(to);

                        IEnumerable<Item> items = field.GetValueAsCollectionOfItems();
                        if (items != null)
                        {
                            foreach (Item ine in items)
                            {
                                object updateObject = null;

                                long Id = ine.ObjectId.ToLong(0);

                                if (Id != 0)
                                {
                                    if (hasCurrentList)
                                    {
                                        updateObject = (from cr in currentList.Cast<CoreObject>()
                                                        where cr.Id == Id
                                                        select cr).FirstOrDefault();
                                    }

                                    if (updateObject == null)
                                    {
                                        //Load from db 
                                        //if (repository != null)
                                        //    updateObject = repository.GetByTypeAndId(inner, Id, null, false, false);
                                        using (IRepository repo = ServiceLocator.Current.TryGet<IRepository>(toProp.PropertyType.Name + "Repository"))
                                        {
                                            updateObject = repo.GetById(Id);

                                            if (updateObject != null)
                                            {
                                                toProp.PropertyType.GetMethod("Remove").Invoke(currenValue, new[] { updateObject });
                                                updateObject = ine.ConvertTo(updateObject);
                                                //DataContext.RegisterCoreForChange(updateObject as CoreObject, context);
                                                toProp.PropertyType.GetMethod("Add").Invoke(currenValue, new[] { updateObject });
                                            }
                                        }
                                        
                                    }
                                    else
                                    {
                                        toProp.PropertyType.GetMethod("Remove").Invoke(currenValue, new[] { updateObject });
                                        updateObject = ine.ConvertTo(updateObject);
                                        //DataContext.RegisterCoreForChange(updateObject as CoreObject, context);
                                        toProp.PropertyType.GetMethod("Add").Invoke(currenValue, new[] { updateObject });
                                    }
                                }
                                else
                                {
                                    //Create new instance
                                    updateObject = Activator.CreateInstance(inner);
                                    updateObject = ine.ConvertTo(updateObject);
                                    
                                    toProp.PropertyType.GetMethod("Add").Invoke(currenValue, new[] { updateObject });
                                }

                            }
                        }
                    }
                    else if (string.IsNullOrWhiteSpace(field.FieldType) == false && (toProp.PropertyType.Name == field.FieldType || toProp.PropertyType.FullName == field.FieldType))
                    {
                        try
                        {
                            Object value = field.GetValue();

                            toProp.SetValue(to, value, null);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else if (toProp.PropertyType == typeof(Attachment))
                    {
                        Attachment value = null;

                        if (field.Value is Attachment)
                        {
                            value = field.Value as Attachment;
                        }
                        else
                        {
                            value = field.TryGetValueAsType<Attachment>();
                        }

                        toProp.SetValue(to, value, null);
                    }
                    else if (toProp.PropertyType == typeof(String))
                    {
                        var value = field.GetValue(); 

                        toProp.SetValue(to, value, null);
                    }
                    else if (toProp.PropertyType == typeof(int)
                         || toProp.PropertyType == typeof(uint)
                         || toProp.PropertyType == typeof(long)
                         || toProp.PropertyType == typeof(ulong)
                         || toProp.PropertyType == typeof(short)
                         || toProp.PropertyType == typeof(ushort)
                         || toProp.PropertyType == typeof(double)
                         || toProp.PropertyType == typeof(float)
                        || toProp.PropertyType == typeof(decimal))
                    {
                        Object v = field.GetValue();

                        string value = (v != null) ? v.ToString() : string.Empty;

                        if (string.IsNullOrWhiteSpace(value) == false)
                        {
                            try
                            {
                                if (toProp.PropertyType == typeof(decimal))
                                {
                                    toProp.SetValue(to, Convert.ToDecimal(value), null);
                                }
                                else if (toProp.PropertyType == typeof(float))
                                {
                                    toProp.SetValue(to, Convert.ToSingle(value), null);
                                }
                                else if (toProp.PropertyType == typeof(double))
                                {
                                    toProp.SetValue(to, Convert.ToDouble(value), null);
                                }
                                else if (toProp.PropertyType == typeof(short))
                                {
                                    toProp.SetValue(to, Convert.ToInt16(value), null);
                                }
                                else if (toProp.PropertyType == typeof(int))
                                {
                                    toProp.SetValue(to, Convert.ToInt32(value), null);
                                }
                                else if (toProp.PropertyType == typeof(long))
                                {
                                    toProp.SetValue(to, Convert.ToInt64(value), null);
                                }
                                else if (toProp.PropertyType == typeof(ushort))
                                {
                                    toProp.SetValue(to, Convert.ToUInt16(value), null);
                                }
                                else if (toProp.PropertyType == typeof(uint))
                                {
                                    toProp.SetValue(to, Convert.ToUInt32(value), null);
                                }
                                else if (toProp.PropertyType == typeof(ulong))
                                {
                                    toProp.SetValue(to, Convert.ToUInt64(value), null);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    else if (toProp.PropertyType == typeof(int) || toProp.PropertyType == typeof(int?))
                    {
                        Object current = field.GetValue();
                        int i;
                        int? value = int.TryParse(current.ToString(), out i) ? (int?)i : null;

                        toProp.SetValue(to, value);

                    }
                    else if (toProp.PropertyType == typeof(long) || toProp.PropertyType == typeof(long?))
                    {
                        Object current = field.GetValue();
                        long i;
                        long? value = long.TryParse(current.ToString(), out i) ? (long?)i : null;

                        toProp.SetValue(to, value);

                    }
                    else if (toProp.PropertyType == typeof(decimal) || toProp.PropertyType == typeof(decimal?))
                    {
                        Object current = field.GetValue();
                        decimal i;
                        decimal? value = decimal.TryParse(current.ToString(), out i) ? (decimal?)i : null;

                        toProp.SetValue(to, value);

                    }
                    else if (toProp.PropertyType == typeof(bool) || toProp.PropertyType == typeof(bool?))
                    {
                        Object current = field.GetValue();

                        if (current != null)
                        {
                            object value = null;

                            string stringBool = current.ToString();

                            if (string.IsNullOrWhiteSpace(stringBool) == false)
                            {
                                if (toProp.PropertyType == typeof(bool?))
                                {
                                    bool flag;
                                    if (Boolean.TryParse(stringBool, out flag))
                                    {
                                        value = new Nullable<bool>(flag);
                                    }
                                    else
                                    {
                                        value = null;
                                    }

                                }
                                else
                                {
                                    value = stringBool.ToBool(false);
                                }
                            }

                            if (value != null)
                                toProp.SetValue(to, value, null);
                        }

                    }
                    else if (toProp.PropertyType == typeof(byte[]))
                    {
                        Object value = field.GetValue();
                        if (field.Value is String)
                        {
                            value = Convert.FromBase64String(value as String);
                        }

                        toProp.SetValue(to, value, null);
                    }
                    else if (toProp.PropertyType == typeof(String[]))
                    {
                        Object value = Utils.JsonStringToObject(field.GetValue().ToString(), typeof(String[]));

                        toProp.SetValue(to, value, null);
                    }
                    else if (field.IsEnumerable == false && string.IsNullOrWhiteSpace(field.FieldType))
                    {
                        //Copy the value from the source to the target
                        Object value = field.GetValue();

                        toProp.SetValue(to, value, null);
                    }
                    else if (field.IsEnumerable == false && string.IsNullOrWhiteSpace(field.FieldType))
                    {
                        //Copy the value from the source to the target
                        Object value = field.GetValue();

                        toProp.SetValue(to, value, null);
                    }
                }
            }
            return to;
        }
    }
    [Serializable]
    [DataContract]
    public class Item
    {
        public Item()
        {
            //this.StoreValue = new StoreValue();
            this.Fields = new FieldCollection();
        }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string LocalId { get; set; }

        [DataMember]
        public string TypeId { get; set; }

        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public string RefersToId { get; set; }

        [DataMember]
        public string RefersToName { get; set; }

        [DataMember]
        public int Updatability { get; set; }


        [DataMember]
        public string TypeName { get; set; }

        [DataMember]
        public string Name { get; set; }

        //[XmlIgnore]
        //public IRolePlayer Role { get; set; }

        //[XmlIgnore]
        //protected StoreValue StoreValue { get; set; }

        [DataMember]
        public string ObjectId { get; set; }

        [DataMember]
        public FieldCollection Fields { get; set; }

        [DataMember]
        public string Include { get; set; }

        [DataMember]
        public string ObjectNumber { get; set; }

        public bool HasFields()
        {
            return this.Fields != null && this.Fields.Count > 0;
        }

        public object GetValue(string name, object defaultValue)
        {
            object f = GetValue(name);

            if (f != null)
                return f;
            else
                return defaultValue;
        }

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
    }

    [Serializable]
    [CollectionDataContract(Name = "ItemCollection", ItemName = "ItemItem")]
    public class ItemCollection : Collection<Item>
    {
    }

    [Serializable]
    [CollectionDataContract(Name = "FieldCollection", ItemName = "FieldItem")]
    public class FieldCollection : Collection<Field>
    {
        public FieldCollection()
        {

        }

        public FieldCollection(IList<Field> list)
            : base(list)
        {

        }

        public bool Contains(string name)
        {
            var found = (from it in this
                            where it.Name.ToLower() == name.ToLower()
                            select it).FirstOrDefault();

            return found != null;
        }

        protected override void InsertItem(int index, Field item)
        {
            if (item != null)
                item.Order = index;

            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, Field item)
        {
            if (item != null)
                item.Order = index;

            base.SetItem(index, item);
        }


    }

    [Serializable]
    [DataContract]
    [KnownType(typeof(Field))]
    [KnownType(typeof(Item))]
    [KnownType(typeof(ItemCollection))]
    [KnownType(typeof(FieldCollection))]
    public class Field
    {
        public Field()
        {
            //this.StoreValue = new StoreValue();
            this.IsEnumerable = false;
            this.IsXml = false;
        }

        public Field(string name, object value)
        {
            this.Name = name;
            this.SetValue(value);
            //this.StoreValue = new StoreValue();
            this.IsEnumerable = false;
            this.IsXml = false;
        }


        [XmlIgnore]
        private bool _IsNull;

        public object SetValue(object value, bool serialize = false)
        {
            _IsNull = true;

            this.Value = null;

            this.FieldType = string.Empty;

            if (value == null)
                return null;

            _IsNull = false;

            this.IsEnumerable = (value is IEnumerable);

            this.FieldType = value.GetType().Name;

            this.Value = value;
            
            return value;
        }

        /// <summary>
        ///     Get or Set data type of the specified filed
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        ///     Gets name of the data type of the specified filed
        /// </summary>
        [DataMember]
        public string FieldType { get; set; }

        [DataMember]
        public string DataType { get; set; }

        [DataMember]
        public bool IsEnumerable { get; set; }


        [DataMember]
        public bool IsXml { get; set; }

        //[DataMember]
        //public IEnumerable<Item> Values { get; set; }

        [DataMember]
        public object Value { get; set; }

        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public long Id { get; set; }

        [DataMember]
        public string LocalId { get; set; }
        
        [DataMember]
        public string Name { get; set; }

        //protected StoreValue StoreValue { get; set; }

        public object GetValue()
        {
            if (Value != null)
            {
                if (this.IsEnumerable && (this.Value is IEnumerable) == false)
                {
                    object output = null;

                    if (FieldType == typeof(ItemCollection).Name)
                        output = GetValueAsCollectionOfItems();
                    else if (FieldType == typeof(FieldCollection).Name)
                        output = GetValueAsCollectionOfField();

                    if (output != null)
                    {
                        return output;
                    }
                }

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

        public IEnumerable<Item> GetValueAsCollectionOfItems()
        {
            try
            {
                return Utils.JsonStringToObject(Value.ToString(), typeof(ItemCollection)) as IEnumerable<Item>;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable<Field> GetValueAsCollectionOfField()
        {
            try
            {
                return Utils.JsonStringToObject(Value.ToString(), typeof(FieldCollection)) as IEnumerable<Field>;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public T TryGetValueAsType<T>()
        {
            var result = TryGetValueAs(typeof(T));

            if (result is T)
                return (T)result;
            else
                return default(T);
            
        }

        public object TryGetValueAs(Type t)
        {
            Exception first = null;
            try
            {
                var value = GetValue();

                if (value == null)
                    return null;

                if (string.IsNullOrWhiteSpace(value.ToString()))
                    return null;

                string strValue = value.ToString();

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var json = serializer.Deserialize(strValue, t);

                return json;
                
            }
            catch (Exception ex)
            {
                first = ex;
            }

            try
            {
                var value = GetValue();
                var json = JsonConvert.DeserializeObject(Value.ToString(), t);

                return json;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
