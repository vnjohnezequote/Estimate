using LiteDB;
using Prism.Mvvm;

namespace AppModels
{
    public class DesignInfor : BindableBase
    {
        [BsonId]
        public int Id { get; set; }
        public string InfoType { get; set; }
        
        private string _content;
        public string Content
        {
            get => this._content;
            set => this.SetProperty(ref this._content, value);
        }

        private string _header;

        public string Header
        {
            get => this._header;
            set => this.SetProperty(ref this._header, value);
        }
        
    }
}
