using System;

namespace XmlUtil.Models
{
    public abstract class IXmlEntity
    {
        public virtual string HideId { get; set; } = Guid.NewGuid().ToString();
    }
}