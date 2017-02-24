using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelBot.Models
{

    public enum BedSizeOptions
    {
        [Terms("K", "kng", "Kng", "KNG", "large", "Large", "King", "king")]
        King,
        [Terms("Q", "q", "Quin", "quin")]
        Queen,
        [Terms("Sngl", "sng", "singl")]
        Single,
        [Terms("Dobl", "doubl", "Doubl")]
        Double
    }

    public enum AmenitiesOptions
    {
        [Terms("except", "but", "not", "all", "everything")]
        Everything = 1,
        Pool,
        ExtraTowels,
        GymAccess,
        Wifi
    }

    [Serializable]
    public class RoomReservation
    {

        //[Describe(Image = @"https://placeholdit.imgix.net/~text?txtsize=16&txt=Bed&w=125&h=40&txttrack=0&txtclr=000&txtfont=bold")]    //not working in the emulator have to check in other place
        //[Describe(Image = @"https://i0.wp.com/www.tangeroutlet.com/images/ipad-map/WIFI-Icon.gif")]    //not working in the emulator have to check in other place
        [Prompt("What kind of {&} do you want?{||}")]
        public BedSizeOptions? BedSize;
        public int? NumberOfOccupants;
        public List<AmenitiesOptions> Amenities;
        public DateTime? CheckInDate;
        public int? NumberOfDaysToStay;
        public static IForm<RoomReservation> BuildForm()
        {
            var form = new FormBuilder<RoomReservation>()
                        .Message("Welcome to hotel reservation bot.")
                        .Field(nameof(BedSize))
                        .Field(nameof(Amenities),
                        validate: async (state, value) =>
                        {
                            var values = ((List<object>)value).OfType<AmenitiesOptions>();
                            var result = new ValidateResult { IsValid = true, Value = values };
                            if (values != null && values.Contains(AmenitiesOptions.Everything))
                            {
                                result.Value = (from AmenitiesOptions amenity in Enum.GetValues(typeof(AmenitiesOptions))
                                                where amenity != AmenitiesOptions.Everything && !values.Contains(amenity)
                                                select amenity).ToList();
                            }
                            return result;
                        }
                        )
                        .Message("For Amenities you have selected {Amenities}")
                        .Field(nameof(CheckInDate))
                        .Field(nameof(NumberOfDaysToStay))
                        .Confirm(@"Do you want to book a room with following detail: 
                                               1. Bedesize Options: {BedSize}
                                               2. Amenities: {Amenities}
                                               3. Check In date: {CheckInDate} 
                                               4. No. of days to stay: {NumberOfDaysToStay}? ")
                        .Build();
            try
            {
                return form;
                #region Build Form
                //return new FormBuilder<RoomReservation>()
                //    .Message("Welcome to the hotel reservation bot!")
                //    .Field(nameof(Amenities),
                //    validate: async (state, value) =>
                //    {
                //        var values = ((List<object>)value).OfType<AmenitiesOptions>();
                //        var result = new ValidateResult { IsValid = true, Value = values };
                //        if (values != null && values.Contains(AmenitiesOptions.Everything))
                //        {
                //            result.Value = (from AmenitiesOptions amenity in Enum.GetValues(typeof(AmenitiesOptions))
                //                            where amenity != AmenitiesOptions.Everything && !values.Contains(amenity)
                //                            select amenity).ToList();
                //        }
                //        return result;
                //    })

                //    .Build();
                #endregion
            }
            catch (Exception e) { }
            return null;
        }
    }
}