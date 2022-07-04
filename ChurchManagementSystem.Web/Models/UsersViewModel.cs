

using System;

namespace ChurchManagementSystem.Web.Models
{
    public class UsersViewModel
    {
        public bool HasData { set; get; }
        public int TotalUsers { get; set; }
        public int IndividualDonors { get; set; }
        public int IndividualReceivers { get; set; }
        public int ParticipatedInBoth { get; set; }
        public int FeelsSent { get; set; }
        public int FeelsReceived { get; set; }
        public int FeelsDonated { get; set; }
        public int CharitiesDonatedTo { get; set; }
        public string TotalAmountGifted { get; set; }
        public string TotalAmountToCharity { get; set; }
        public string TotalAmountReceived { get; set; }
        public int Today { get; set; }
        public int ThisWeek { get; set; }
        public int ThisMonth { get; set; }
        public int CodesCreated { get; set; }
        public int RegisteredRetailers { get; set; }

        public int GiftsBought { get; set; }
        public int GiftsReceived { get; set; }
        public int DonationsMade { get; set; }
        public int CharitiesImpacted { get; set; }
        public string LastGiftReceivedDate { get; set; }
        public string LastGiftPurchasedDate { get; set; }
        public string TotalBalance { get; set; }
        public string AvailableBalance { get; set; }
        public string MoneyInQR { get; set; }
        public DateTime MembershipDate { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}