using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using XmlUtil.Models;

namespace XmlUtil
{
    /// <summary>
    /// 针对列表集合的操作（线性）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class XmlHelper<T> where T : IXmlEntity
    {
        private readonly string _path;

        public XmlHelper(string path)
        {
            _path = path;
        }

        public void Add(T obj)
        {
            if (!File.Exists(_path))
            {
                CreateXmlFile();
            }

            var doc = XDocument.Load(_path);
            var root = doc.Root;
            var type = typeof(T);
            AddElementToRoot(root, obj, type);
            doc.Save(_path);
        }

        public void AddRange(ICollection<T> objs)
        {
            if (!File.Exists(_path))
            {
                CreateXmlFile();
            }

            var doc = XDocument.Load(_path);
            var root = doc.Root;
            var type = typeof(T);
            foreach (var item in objs)
            {
                AddElementToRoot(root, item, type);
            }

            doc.Save(_path);
        }

        private void AddElementToRoot(XContainer root, T obj, Type type)
        {
            var element = new XElement(typeof(T).Name);
            var properties = type.GetProperties();
            foreach (var propertyInfo in properties)
            {
                if(propertyInfo.PropertyType.GetInterface("ICollection", false) != null)
                    throw new Exception("暂不支持直接存储集合");
                element.SetAttributeValue(propertyInfo.Name, propertyInfo.GetValue(obj, null));
            }
            root.Add(element);
        }

        private void CreateXmlFile()
        {
            var xdoc = new XDocument();
            //创建根节点  
            var root = new XElement("Root");
            xdoc.AddFirst(root);
            xdoc.Save(_path);
        }

        public ICollection<T> Finds()
        {
            var doc = XDocument.Load(_path);
            var root = doc.Root;
            ICollection<T> result = new List<T>();
            var type = typeof(T);
            var properties = type.GetProperties();
            var nodes = root.Elements();
            foreach (var xNode in nodes)
            {
                var obj = Activator.CreateInstance<T>();

                foreach (var propertyInfo in properties)
                {
                    var value = xNode.Attribute(propertyInfo.Name)?.Value;
                    propertyInfo.SetValue(obj, Convert.ChangeType(value, propertyInfo.PropertyType));
                }

                result.Add(obj);
            }

            return result;
        }

        public ICollection<T> Finds(Func<T, bool> func)
        {
            return Finds().Where(func).ToList();
        }

        public T First(Func<T, bool> func)
        {
            return Finds().First(func);
        }

        public T First()
        {
            return Finds().First();
        }

        public T FirstOrDefault()
        {
            return Finds().FirstOrDefault();
        }

        public T FirstOrDefault(Func<T, bool> func)
        {
            return Finds().FirstOrDefault(func);
        }

        public void Remove(T obj)
        {
            var doc = XDocument.Load(_path);
            var root = doc.Root;
            var node = root.Elements()
                .FirstOrDefault(m => m.Attribute("HideId")
                    .Value.Equals(obj.HideId));
            node.Remove();
            doc.Save(_path);
        }

        public void RemoveRange(ICollection<T> objs)
        {
            foreach (var obj in objs)
            {
                Remove(obj);
            }
        }

        public void Update(T obj)
        {
            Remove(obj);
            Add(obj);
        }

    }
}