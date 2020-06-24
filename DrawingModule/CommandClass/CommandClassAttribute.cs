using System;

namespace DrawingModule.CommandClass
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public class CommandClassAttribute : Attribute
    {
        // Token: 0x06000D5D RID: 3421 RVA: 0x00016F0C File Offset: 0x0001630C
        public CommandClassAttribute(Type name)
        {
            this.type_ = name;
        }

        // Token: 0x170001EA RID: 490
        // (get) Token: 0x06000D5E RID: 3422 RVA: 0x00016F28 File Offset: 0x00016328
        public Type Type
        {
            get
            {
                return this.type_;
            }
        }

        // Token: 0x0400073E RID: 1854
        private Type type_;

    }
}
