using System;

namespace BacaratWeb.ViewModel.Transfert
{
    [Serializable]
    public class DocumentShareViewModel
    {
        public int Id { get; set; }

        public int DocumentId { get; set; }

        public string Email { get; set; }

        public string CreateurName { get; set; }

        public DateTimeOffset SharedDate { get; set; }

        public DateTimeOffset ExpiryDate { get; set; }
    }
}
