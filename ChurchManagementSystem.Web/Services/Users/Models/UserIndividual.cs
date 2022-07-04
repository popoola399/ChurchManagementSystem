using Newtonsoft.Json;

using System;

namespace ChurchManagementSystem.Web.Services.Users.Models
{
    public class GetUserIndividualSummaryResponse
    {
        public int UserId { set; get; }
        public string Name { set; get; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string ReasonForDeactivation { get; set; }
        public DateTime DeactivationDate { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string StripeId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class UserIndividualSummaryViewModel
    {
        public bool HasData { set; get; }
        public int UserId { set; get; }
        public string MembershipDate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    //public class UserIndividualDataViewModel
    //{
    //    [JsonProperty("transactionDate")]
    //    public string TransactionDate { get; set; }

    //    [JsonProperty("transactionType")]
    //    public string TransactionType { get; set; }

    //    [JsonProperty("paymentType")]
    //    public string PaymentType { get; set; }

    //    [JsonProperty("transactionStatus")]
    //    public string TransactionStatus { get; set; }

    //    [JsonProperty("giftBuyer")]
    //    public string GiftBuyer { get; set; }

    //    [JsonProperty("giftRecipient")]
    //    public string GiftRecipient { get; set; }

    //    [JsonProperty("fave")]
    //    public string Fave { get; set; }

    //    [JsonProperty("organization")]
    //    public string Organization { get; set; }

    //    [JsonProperty("runningBalance")]
    //    public string RunningBalance { get; set; }

    //    [JsonProperty("giftedAmount")]
    //    public string GiftedAmount { get; set; }

    //    [JsonProperty("receivedAmount")]
    //    public string ReceivedAmount { get; set; }

    //    [JsonProperty("donatedAmount")]
    //    public string DonatedAmount { get; set; }

    //    [JsonProperty("stripeFees")]
    //    public string StripeFees { get; set; }

    //    [JsonProperty("batchToRecepientDays")]
    //    public string BatchToRecepientDays { get; set; }

    //    [JsonProperty("qrCode")]
    //    public string QrCode { get; set; }
    //}
}