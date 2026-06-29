using System;

namespace BacaratWeb.ViewModel.Transfert
{
    [Serializable]
    public class DocumentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public DateTimeOffset UploadDate { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
        public string OwnerName { get; set; }
        public string Email { get; set; }
        public string StatutDocumentNameFr { get; set; }
        public string StatutDocumentNameEn { get; set; }
    }
}