namespace ComplaintManagementSystem.Database.Complaints.Types
{
    public class ComplaintsDto
    {
        public Guid Reference { get; set; }
        public string ConsumerEmail { get; set; }
        public string ConsumerPostcode { get; set; }
        public string FirstMessage { get; set; }
        public Guid BusinessReference { get; set; }
        public DateTime TimeOpened { get; set; }
        public bool IsOpen { get; set; }
        public Guid? ClosedBy { get; set; }
        public string? ClosedReason { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
