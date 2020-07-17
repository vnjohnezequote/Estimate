using LiteDB;
using Prism.Mvvm;
using ProtoBuf;

namespace AppModels.ResponsiveData
{
    [ProtoContract]
    public class DesignInfor : BindableBase
    {
        #region Field
        private string _content;
        private string _header;

        #endregion

        #region Property
        [BsonId]
        public int Id { get; set; }
        public string InfoType { get; set; }
        public string Content
        {
            get => this._content;
            set => this.SetProperty(ref this._content, value);
        }
        public string Header
        {
            get => this._header;
            set => this.SetProperty(ref this._header, value);
        }

        #endregion


    }
}
