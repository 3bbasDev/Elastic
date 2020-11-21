using System;
using System.Collections.Generic;
using System.Dynamic;

namespace ElasticApi.Models
{
    public class DynamicClass : DynamicObject
    {
        private Dictionary<string, KeyValuePair<Type, object>> _fields;

        public DynamicClass(List<Field> fields)
        {
            _fields = new Dictionary<string, KeyValuePair<Type, object>>();
            fields.ForEach(x => _fields.Add(x.FieldName,
                new KeyValuePair<Type, object>(x.FieldType, null)));
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_fields.ContainsKey(binder.Name))
            {
                var type = _fields[binder.Name].Key;
                if (value.GetType() == type)
                {
                    _fields[binder.Name] = new KeyValuePair<Type, object>(type, value);
                    return true;
                }
                else throw new Exception("Value " + value + " is not of type " + type.Name);
            }
            return false;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = _fields[binder.Name].Value;
            return true;
        }
    }
    public class Field
    {
        public Field(string name, Type type)
        {
            this.FieldName = name;
            this.FieldType = type;
        }

        public string FieldName;

        public Type FieldType;
    }

    public class CreateClass
    {
        public CreateClass()
        {
            var fields = new List<Field>() {
            new Field("CustomerName", typeof(string)),
            new Field("Email", typeof(string)),
            new Field("Phone1", typeof(string)),
            new Field("Phone2", typeof(string)),
            new Field("Parent", typeof(string)),
        };

            dynamic obj = new DynamicClass(fields);

            //set
            obj.CustomerName = "123456";
            obj.Email = "John";
            obj.Phone1 = "13216546541";
            obj.Phone2 = "894654564151";
            obj.Parent = null;

            //obj.Age = 25;             //Exception: DynamicClass does not contain a definition for 'Age'
            //obj.EmployeeName = 666;   //Exception: Value 666 is not of type String

            //get
            Console.WriteLine(obj.CustomerName);     //123456
            Console.WriteLine(obj.Email);   //John
            Console.WriteLine(obj.Phone1);
            Console.WriteLine(obj.Phone2);

        }

    }
}
